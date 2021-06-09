using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    public class RabbitMQConfig
    {
        public static string Host { get; set; }
        public static string VirtualHost { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static int Port { get; set; }

        static RabbitMQConfig()
        {
            Host = "47.103.56.113";
            VirtualHost = "/";
            UserName = "guest";
            Password = "guest";
            Port = 5672;
        }
    }
}
