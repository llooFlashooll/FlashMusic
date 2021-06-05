using RabbitMQ.Client;
using RabbitMQProducer;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderRabbitMQProducer
{
    public class OrderProducer
    {
        const string ORDER_QUEUE = "order-queue";
        const int X_EXPIRE_SEC = 60000;     // 队列过期时间
        const int M_EXPIRE_SEC = 30000;     // 队列上消息过期时间，应小于队列过期时间，12s

        static void Main(string[] args)
        {
            Console.WriteLine("Producer 程序运行入口");
        }

        public static void PublishMsg(string message, int expiredsec = M_EXPIRE_SEC)
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
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("x-expires", X_EXPIRE_SEC);         // 队列过期时间
                    dic.Add("x-message-ttl", expiredsec);       // 队列上消息过期时间，应小于队列过期时间
                    dic.Add("x-dead-letter-exchange", "exchange-direct");       // 过期消息转向路由
                    dic.Add("x-dead-letter-routing-key", "routing-delay");      // 过期消息转向路由相匹配routingkey

                    channel.QueueDeclare(queue: ORDER_QUEUE,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: dic);

                    var messagebody = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(string.Empty, ORDER_QUEUE, null, messagebody);

                    Console.WriteLine("Producer sent the message, which is: {0}", messagebody);
                }
            }
        }
    }
}
