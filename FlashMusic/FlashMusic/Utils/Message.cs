using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Utils
{
    public class Message
    {
        public string Msg { get; set; }
        public int Code { get; set; }
        public Message() { }

        public Message(string msg, int code)
        {
            this.Msg = msg;
            this.Code = code;
        }

        public static Message UserNameNotFilled()
        {
            return new Message("用户名未填写", -1);
        }

        public static Message PasswordNotFilled()
        {
            return new Message("密码未填写或密码太短", -1);
        }

        public static Message EmailNotFilled()
        {
            return new Message("邮箱未填写", -1);
        }

        public static Message UserNameRepeat()
        {
            return new Message("用户名已经被注册", -1);
        }

        public static Message RegisterSuccess()
        {
            return new Message("注册成功", 200);
        }

        public static Message RegisterFail()
        {
            return new Message("注册失败", -1);
        }

        public static Message LoginFail()
        {
            return new Message("未找到该用户", -1);
        }

        public static Message GetCartSuccess()
        {
            return new Message("返回购物车成功", 200);
        }

        public static Message AddCartSuccess()
        {
            return new Message("添加购物车成功", 200);
        }

        public static Message AddCartFail()
        {
            return new Message("添加购物车失败", -1);
        }

        public static Message DelCartSuccess()
        {
            return new Message("删除购物车产品成功", 200);
        }

        public static Message DelCartFail()
        {
            return new Message("删除购物车产品失败", -1);
        }

        public static Message PayCartSuccess()
        {
            return new Message("支付购物车成功", 200);
        }

        public static Message PayCartFail()
        {
            return new Message("支付购物车失败", -1);
        }

        public static Message UpdateCartSuccess()
        {
            return new Message("更新购物车成功", 200);
        }

        public static Message UpdateCartFail()
        {
            return new Message("更新购物车失败", -1);
        }

        public static Message GetHistorySuccess()
        {
            return new Message("获取历史记录成功", 200);
        }

        public static Message GetHistoryFail()
        {
            return new Message("获取历史记录失败", -1);
        }
    }
}
