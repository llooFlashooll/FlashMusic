using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MailService
{
    public class MailBase
    {
        // 初始化值
        private MailAddress _fromMailAddress;
        private readonly SmtpClient _mailClient;
        public readonly string MailAddress;
        public readonly string _to = string.Empty;
        public readonly string _cc = string.Empty;
        private static ConcurrentDictionary<string, SmtpClient> _mailClientDictionary = new ConcurrentDictionary<string, SmtpClient>();

        /// <summary>
        /// 初始化邮箱账号信息
        /// </summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="port">端口号</param>
        /// <param name="mailAddress">邮件账号</param>
        /// <param name="password">邮件密码</param>
        /// <param name="to">


        /// <summary>
        /// 初始化邮箱账号信息
        /// </summary>
        /// <param name="host">邮件服务器</param>
        /// <param name="port">端口号</param>
        /// <param name="mailAddress">邮件账号</param>
        /// <param name="password">邮件密码</param>
        /// <param name="to">收件人邮件账号</param>
        /// <param name="cc">抄送人，多个逗号隔开</param>
        /// <param name="title">标题</param>
        /// <param name="enableSsl"></param>
        public MailBase(string host, int port, string mailAddress, string password, 
            string to = "", string cc = "", string title = "", bool enableSsl = true)
        {
            MailAddress = mailAddress;
            _fromMailAddress = new MailAddress(mailAddress);
            _mailClient = GetSmtpClient(host, port, mailAddress, password, enableSsl);
            _to = to;
            _cc = cc;
        }

        // 取得Smtp对象
        private SmtpClient GetSmtpClient(string host, int port, string from, string password, bool enableSsl = true)
        {
            var key = $"{host}_{port}_{from}_{password}";
            SmtpClient client = null;
            if(_mailClientDictionary.ContainsKey(key))
            {
                _mailClientDictionary.TryGetValue(key, out client);
            }
            else
            {
                _fromMailAddress = new MailAddress(from);
                // 设置邮件发送服务器
                client = new SmtpClient(host, port);
                // 设置发送人的邮箱账号和密码
                client.Credentials = new NetworkCredential(from, password);
                // 启用ssl，安全发送
                client.EnableSsl = enableSsl;
                _mailClientDictionary.TryAdd(key, client);
            }
            return client;
        }

        public virtual void Send(string to, string title, string content)
        {
            Send(to, "", title, content);
        }

        // 异步版本
        public virtual void SendAsync(string to, string title, string content)
        {
            SendAsync(to, "", title, content);
        }

        // 类私有函数
        private MailMessage Preprocess(string to, string cc, string title, string content)
        {
            if(string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Mail receiver address can't be null", "to");
            }
            MailMessage message = new MailMessage();
            // 设置发件人
            message.From = _fromMailAddress;
            message.To.Add(to);
            // 设置抄送人
            if(!string.IsNullOrEmpty(cc))
            {
                message.CC.Add(to);
            }
            message.Subject = title;
            message.Body = content;
            return message;
        }

        // 类私有函数，构造函数需要预设收件人，deprecated
        private void Send(string title, string content)
        {
            if(string.IsNullOrEmpty(_to))
            {
                throw new ArgumentException("Mail receiver address can't be null. Constructor must set receiver mail address.", "_to");
            }
            Send(_to, _cc, title, content);
        }

        public void Send(string to, string cc, string title, string content)
        {
            var msg = Preprocess(to, cc, title, content);
            _mailClient.Send(msg);
        }

        public void SendAsync(string to, string cc, string title, string content)
        {
            var msg = Preprocess(to, cc, title, content);
            _mailClient.Send(msg);
        }

        // 测试回调函数
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if(e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled", token);
            }
            if(e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
        }
    }
}
