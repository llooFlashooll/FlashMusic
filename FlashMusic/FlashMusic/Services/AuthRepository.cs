using FlashMusic.Database;
using FlashMusic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FlashMusic.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._config = configuration;
            this._httpContextAccessor = httpContextAccessor;
        }

        public bool Register(User UserInfo)
        {
            _context.User.Add(new User
            {
                UserName = UserInfo.UserName,
                Password = UserInfo.Password,
                Email = UserInfo.Email,
                Avatar = UserInfo.Avatar
            });
            return _context.SaveChanges() > 0;
        }

        public User GetUserByName(string UserName)
        {
            return _context.User.FirstOrDefault(t => t.UserName == UserName);
        }

        public string GetToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 创建用户身份标识，可按需添加更多信息
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.UserId.ToString(), ClaimValueTypes.Integer32),     // 用户id
                new Claim("name", user.UserName)    // 用户名
            };

            // 创建令牌
            var token = new JwtSecurityToken(
                issuer: _config["jwt:Issuer"],
                audience: _config["jwt:Audience"],
                signingCredentials: credentials,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(30)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        public int GetIdFromToken()
        {
            var nameId = _httpContextAccessor.HttpContext.User.FindFirst("id");
            return nameId != null ? Convert.ToInt32(nameId.Value) : 0;
        }

        public User GetUserById(int UserId)
        {
            return _context.User.FirstOrDefault(t => t.UserId == UserId);
        }

    }
}
