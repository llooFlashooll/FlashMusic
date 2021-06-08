using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using FlashMusic.Services;
using FlashMusic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly RedisHelper _redisHelper;

        private static readonly int TIME_EXPIRE = 300;
        private static readonly int FAIL_RETRY_TIMES = 3;
        private static readonly string RedisConnectionString = "119.3.254.34, password=123456, DefaultDatabase=0";


        public LoginController(IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._authRepository = authRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
            this._redisHelper = new RedisHelper(RedisConnectionString);
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDto loginUser)
        {
            string username = loginUser.UserName;
            string password = loginUser.Password;
            User existUser = _authRepository.GetUserByName(loginUser.UserName);
            if(existUser == null)
            {
                return Ok(Message.LoginUnfindUser());
            }
            // redis 登陆失败逻辑
            string key = existUser.UserId.ToString();
            if(!_redisHelper.Exist(key))
            {
                _redisHelper.CreateKeyValue(key);
            }
            if(_redisHelper.GetStringKey(key) == 0)
            {
                _redisHelper.SetExpiry(key, TIME_EXPIRE);
            }

            if(_redisHelper.GetStringKey(key) >= FAIL_RETRY_TIMES)
            {
                int remainExpiryTime = _redisHelper.GetKeyExpiryTime(key);
                string msg = "登录失败次数已达上限，请" + remainExpiryTime.ToString() + "s后再试";
                return Ok(Message.LoginExceedTimes(msg));
            }

            if(password != existUser.Password)
            {
                _redisHelper.StringIncrement(key);
                return Ok(Message.LoginPasswordError());
            }

            // 登陆成功
            _redisHelper.KeyDelete(key);
            return Ok(new { token = _authRepository.GetToken(existUser) });
        }


        // Authorize属性：加上权限验证，会跳过AllowAnonymous方法
        [Authorize]
        [HttpGet]
        public IActionResult TestToken()
        {
            return Ok(new { id = _authRepository.GetIdFromToken() });
        }

    }
}
