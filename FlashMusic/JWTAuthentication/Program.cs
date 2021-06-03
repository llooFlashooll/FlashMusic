using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace JWTAuthentication
{
    class Program
    {
        // 将 JWT 三个字母通过SHA256加密后得到
        private const string secret = "fc93cb07e1ad92898527100e58a1cf1d1e7f65e9a266a6f87f3c84feb541c7b3";

        static void Main(string[] args)
        {
            JWTEncode();        // 获取JWT  方式一
            JWTEncode_2();      // 获取JWT  方式二
            Console.ReadKey();
        }

        /// 获取JWT  方式一
        public static void JWTEncode()
        {
            //组成 JWT 的header
            //var header = new Dictionary<string, object>
            //{
            //    { "alg", "HS256"},
            //    { "typ", "JWT" },
            //};
            //定义payload中的数据  里面的数据可随意填，一般都是返回用户数据
            var payload = new Dictionary<string, object>
            {
                { "name", "张三" },
                { "time", DateTime.Now }
            };

            //加密的秘钥，这个接收端也需要有相同的。JWT 字符串进行SH256加密

            //生成JWT签名的算法
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //JSON序列化与反序列化的接口
            IJsonSerializer serializer = new JsonNetSerializer();
            //Base64编码器
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //JWT编码器
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            // extraHeaders:
            // 任意一组额外的标题。即自定义使用的签名算法和加密类型
            // payload:
            // 任意负载（必须可序列化为JSON）
            // key:
            // 用于签名令牌的密钥
            // 不加header表示使用默认的签名算法和加密类型
            var token = encoder.Encode(payload, secret);
            Console.WriteLine($"方式一生成的token为：[{token}]");
            JWTDecode(token);       // 通过第一种方式进行解码
        }

        /// 获取JWT  方式二
        public static void JWTEncode_2()
        {

            // 使用Fluent API对JWT进行编译。
            var token = new JwtBuilder()
          .WithAlgorithm(new HMACSHA256Algorithm())     // 设置JWT算法
          .WithSecret(secret)                           // 设置加密密钥
          .AddClaim("time", DateTime.Now)               // 设置时间
          .AddClaim("name", "李四")
          .Encode();                                    // 使用提供的依赖项对令牌进行编码
            Console.WriteLine($"方式二生成的token为：[{token}]");
            JWTDecode_2(token);
        }

        /// 解析JWT  方式一
        public static void JWTDecode(string token)
        {
            try
            {
                // JSON序列化与反序列化的接口
                IJsonSerializer serializer = new JsonNetSerializer();
                // UTC日期时间的提供程序。
                var provider = new UtcDateTimeProvider();
                // 给定JWT，在不引发异常的情况下验证其签名的正确性
                IJwtValidator validator = new JwtValidator(serializer, provider);
                // Base64编码器
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                // 对称 JWT签名的算法
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                // JWT解码器
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                // 解析后的json
                // token 要解析的token
                // secret 解析token需要的秘钥
                // verify 是否验证签名（默认为true）
                var json = decoder.Decode(token, secret, verify: true);
                // 输出解析后的json
                Console.WriteLine($"方式一解析后的json为：[{json}]");
            }
            catch (TokenExpiredException ex)
            {
                Console.WriteLine("令牌已过期：" + ex.ToString());
            }
            catch (SignatureVerificationException ex)
            {
                Console.WriteLine("签名有误：" + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析json时出现异常：" + ex.ToString());
            }
        }

        /// 解析JWT  方式二
        public static void JWTDecode_2(string token)
        {
            try
            {
                var json = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())       // 设置JWT算法
                .WithSecret(secret)                             // 校验的秘钥
                .MustVerifySignature()                          // 必须校验秘钥
                .Decode(token);                                 // 解析token
                // 输出解析后的json
                Console.WriteLine($"方式二解析后的json为：[{json}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("解析json时出现异常：" + ex.ToString());
            }

        }
    }
}
