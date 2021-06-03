using FlashMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Services
{
    public interface IAuthRepository
    {
        // 注册
        public bool Register(User UserInfo);

        public User GetUserByName(string UserName);

        // 登录
        public string GetToken(User user);

        public int GetIdFromToken();

        public User GetUserById(int UserId);

    }
}
