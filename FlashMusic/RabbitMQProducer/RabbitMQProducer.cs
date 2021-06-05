using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQProducer
{
    public class RabbitMQProducer
    {
        // 测试的运行函数
        /*        static void Main(string[] args)
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri("amqp://guest:guest@119.3.254.34:5672")
                    };

                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();
                    channel.QueueDeclare("rabbit-queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    var message = new { Name = "Producer", Message = "Hello!" };
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    channel.BasicPublish("", "rabbit-queue", null, body);
                }*/
        static void Main(string[] args)
        {
            Console.WriteLine("Producer 程序运行入口");
        }

        public static void PublishMail(string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQConfig.Host,
                Port = RabbitMQConfig.Port,
                VirtualHost = RabbitMQConfig.VirtualHost,
                UserName = RabbitMQConfig.UserName,
                Password = RabbitMQConfig.Password
            };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    var MAIL_QUEUE = "mail-queue";
                    channel.QueueDeclare(MAIL_QUEUE,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var messagebody = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(string.Empty, MAIL_QUEUE, null, messagebody);
                }
            }
        }
    }
}
