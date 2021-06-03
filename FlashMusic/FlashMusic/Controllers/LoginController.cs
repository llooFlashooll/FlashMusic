using AutoMapper;
using FlashMusic.Dtos;
using FlashMusic.Models;
using FlashMusic.Services;
using FlashMusic.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Controllers
{
    [Route("/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public LoginController(IAuthRepository authRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this._authRepository = authRepository;
            this._httpContextAccessor = httpContextAccessor;
            this._mapper = mapper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserAddDto loginUser)
        {
            string username = loginUser.UserName;
            string password = loginUser.Password;
            User existUser = _authRepository.GetUserByName(loginUser.UserName);
            if(existUser != null && existUser.Password == password)
            {
                return Ok(new { token = _authRepository.GetToken(existUser) });
            } else
            {
                return Ok(Message.LoginFail());
            }
        }
    }
}
