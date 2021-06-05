using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
using System;
using System.Collections.Generic;
using System.Text;

namespace TencentCos
{
    class TencentCos
    {
        static void Main(string[] args)
        {
            // 初始化 CosXmlConfig 
            string appid = "1302948545";//设置腾讯云账户的账户标识 APPID
            string region = "ap-shanghai"; //设置一个默认的存储桶地域
            CosXmlConfig config = new CosXmlConfig.Builder()
              .IsHttps(true)  //设置默认 HTTPS 请求
              .SetRegion(region)  //设置一个默认的存储桶地域
              .SetDebugLog(true)  //显示日志
              .Build();  //创建 CosXmlConfig 对象

            // 初始化 QCloudCredentialProvider
            string secretId = "AKIDiJhqkQr31kETSCiz2rQkoeFbiklbDcmh"; //"云 API 密钥 SecretId";
            string secretKey = "hANL3VhkaBZicHi15uUotjKhvviGZdS3"; //"云 API 密钥 SecretKey";
            long durationSecond = 600;  //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider cosCredentialProvider = new DefaultQCloudCredentialProvider(
              secretId, secretKey, durationSecond);

            // 初始化 CosXmlServer
            CosXml cosXml = new CosXmlServer(config, cosCredentialProvider);

            // 具体操作
            try
            {
                string bucket = "flash-1302948545"; //格式：BucketName-APPID
                GetBucketRequest request = new GetBucketRequest(bucket);
                //执行请求
                GetBucketResult result = cosXml.GetBucket(request);
                //bucket的相关信息
                ListBucket info = result.listBucket;
                Console.WriteLine(info);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                //请求失败
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                //请求失败
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
    }
}
