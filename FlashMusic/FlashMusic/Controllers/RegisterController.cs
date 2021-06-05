using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using FlashMusic.Services;
using FlashMusic.Utils;
using MailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RegisterController(IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._authRepository = authRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserAddDto UserInfo)
        {
            if(UserInfo.UserName == null)
            {
                return Ok(Message.UserNameNotFilled());
            }
            if(UserInfo.Password == null || UserInfo.Password.Length < 6)
            {
                return Ok(Message.PasswordNotFilled());
            }
            if(UserInfo.Email == null)
            {
                return Ok(Message.EmailNotFilled());
            }
            if(_authRepository.GetUserByName(UserInfo.UserName) != null)
            {
                return Ok(Message.UserNameRepeat());
            }

            var userinfo = _mapper.Map<UserAddDto, User>(UserInfo);
            string email = UserInfo.Email;
            int flag = 0;
            if(_authRepository.Register(userinfo))      // 操作数据库
                flag = 1;
            else
                flag = 0;

            if(flag == 0)
                return Ok(Message.RegisterFail());
            else
            {
                RabbitMQProducer.RabbitMQProducer.PublishMail(email);       // 发送邮件

                return Ok(Message.RegisterSuccess());
            }
        }
    }
}
