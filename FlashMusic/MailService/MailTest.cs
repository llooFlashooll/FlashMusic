using System;
using System.Collections.Generic;
using System.Text;

namespace MailService
{
    public class MailTest
    {
        static void Main(string[] args)
        {
            QQMailSender.SendMailAsync("zeyangzhuang0315@gmail.com", "遥远的她", "在这半山那天，我知我知快将要别离没说话");
            Console.WriteLine("Succeed sending mail");
            Console.WriteLine("Exiting");
            Console.ReadLine();
        }
    }
}
