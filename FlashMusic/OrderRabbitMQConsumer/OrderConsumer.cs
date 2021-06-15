using AutoMapper;
using FlashMusic.Dtos;
using Microsoft.AspNetCore.Http;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQProducer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderRabbitMQConsumer
{
    public class OrderConsumer
    {
        static HttpClient client = new HttpClient();


        private static void PrintSep()
        {
            Console.WriteLine("====================");
        }

        static void ThreadProcess(Object msg)
        {
            string message = (string)msg;
            HandleExpiredOrderAsync(message);
        }

        static async void HandleExpiredOrderAsync(string message)
        {
            PrintSep();
            Console.WriteLine("接收到的消息是：");
            Console.WriteLine(message);
            string[] res = message.Split(" ");
            string email = res[0];
            string token = res[1];
            // 获取此时 cart 的数量，为0 return，不为0 发邮件
            
            try
            {
                int num = await GetCartNumAsync(token);
                if (num == 0)
                    return;
                else
                {
                    Console.WriteLine("订单支付超时");
                    SendOrderEmail(email);
                    Console.WriteLine("Successfully cancel the order");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void SendOrderEmail(string email)
        {
            try
            {
                string title = "订单支付超时";
                string content = "您的一笔订单由于超时未付款已经被取消.😭";

                MailService.QQMailSender.SendMail(email, title, content);
                Console.WriteLine("发送订单支付超时邮件成功");
            }
            catch(Exception e)
            {
                Console.WriteLine("发送邮件失败");
                Console.WriteLine(e.Message);
            }
        }

        // 从response中读取num
        static async Task<int> GetCartNumWithUrlAsync(string path)
        {
            int num = 0;
            HttpResponseMessage response = await client.GetAsync(path);
            if(response.IsSuccessStatusCode)
            {

                var res = await response.Content.ReadAsAsync<int>();

                num = res;
            }
            return num;
        }

        // 传入token
        static async Task<int> GetCartNumAsync(string token)
        {
            int num = 0;
            client.BaseAddress = new Uri("http://47.103.56.113:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            num = await GetCartNumWithUrlAsync("cart/num");

            return num;
        }


        public static void DelayMessageConsumeByMessageTTL()
        {
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
                    channel.ExchangeDeclare(exchange: "exchange-direct", type: "direct");
                    string name = channel.QueueDeclare().QueueName;     // 匿名队列/死信队列
                    channel.QueueBind(queue: name, exchange: "exchange-direct", routingKey: "routing-delay");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, e) =>
                    {
                        var body = e.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        
                        // Console.WriteLine("接收到的消息是：");
                        // Console.WriteLine(message);

                        // 使用线程池，将工作函数排入线程池，每排入一个工作函数，就相当于请求一个线程
                        // 此处相当于一个线程
                        ThreadPool.QueueUserWorkItem(ThreadProcess, message);
                    };

                    channel.BasicConsume(queue: name, 
                                        autoAck: true, 
                                        consumer: consumer);

                    Console.WriteLine("Please press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("消费者Two：开始处理超时订单");
            DelayMessageConsumeByMessageTTL();
        }
    }
}
