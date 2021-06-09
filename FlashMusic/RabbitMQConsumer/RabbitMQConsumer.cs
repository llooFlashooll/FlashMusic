using MailService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQProducer;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQConsumer
{
    public class RabbitMQConsumer
    {
        // 测试的运行函数
        /*        static void Main(string[] args)
                {
                    var factory = new ConnectionFactory
                    {
                        Uri = new Uri("amqp://guest:guest@47.103.56.113:5672")
                    };

                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();
                    channel.QueueDeclare("rabbit-queue",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    channel.BasicConsume("rabbit-queue", true, consumer);
                    Console.ReadLine();
                }*/

        private static void PrintSep()
        {
            Console.WriteLine("====================");
        }

        private static void SendMail(string message)
        {
            var title = "Welcome to Flash Music";
            var content = "您已经成功注册 Flash Music，请妥善保管密码。Enjoy your time~😊";
            QQMailSender.SendMailAsync(message, title, content);
            Console.WriteLine("Succeed sending mail");

        }

        static void Main(string[] args)
        {
            Console.WriteLine("消费者One: 开始处理邮件");

            var factory = new ConnectionFactory
            {
                HostName = RabbitMQConfig.Host,
                Port = RabbitMQConfig.Port,
                VirtualHost = RabbitMQConfig.VirtualHost,
                UserName = RabbitMQConfig.UserName,
                Password = RabbitMQConfig.Password
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var MAIL_QUEUE = "mail-queue";
                    channel.QueueDeclare(MAIL_QUEUE,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, e) =>
                    {
                        new Task(() =>
                        {
                            var body = e.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine("Send email to {0} at {1}", message, DateTime.Now);

                            SendMail(message);
                        }).Start();

                        Console.WriteLine("Command flow is here.");
                    };

                    channel.BasicConsume(MAIL_QUEUE, true, consumer);

                    Console.WriteLine("Please press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
