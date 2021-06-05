using System;
using System.Collections.Generic;
using System.Text;

namespace MailService
{
    public class QQMailSender : MailBase
    {
        private const string host = "smtp.qq.com";
        private const int port = 25;

        public QQMailSender(string mailAddress, string password, string to = "", string cc = "", string title = "") : base(host, port, mailAddress, password, to, cc)
        {
        }

        // 使用前请开启邮箱的 SMTP 服务，并使用服务授权码作为密码
        // private static QQMailSender _defaultMailSender = new QQMailSender("example@qq.com", "这里换成邮箱对应服务码");
        private static QQMailSender _defaultMailSender = new QQMailSender("1330315641@qq.com", "yuufnukxocpobaec");


        public static void SendMail(string to, string title, string content)
        {
            _defaultMailSender.Send(to, title, content);
        }

        public static void SendMailAsync(string to, string title, string content)
        {
            _defaultMailSender.SendAsync(to, title, content);
        }
    }
}
