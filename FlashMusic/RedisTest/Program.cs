using StackExchange.Redis;
using System;
using System.Threading;

namespace RedisTest
{
    class Program
    {
        static ConnectionMultiplexer redis = null;
        static IDatabase database = null;

        // 初始化连接connection
        public static void InitConnect()
        {
            try
            {
                var RedisConnection = "47.103.56.113, password=123456, DefaultDatabase=0";
                // var RedisConnection = Configuration.GetConnectionString("RedisConnectionString");
                redis = ConnectionMultiplexer.Connect(RedisConnection);
                database = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                redis = null;
                database = null;
            }
        }

        /// <summary>
        /// 查询键值是否在Redis中存在
        /// </summary>
        public static bool Exist(string key)
        {
            bool flag = true;
            if(!database.KeyExists(key))
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 创建键值对
        /// </summary>
        public static bool CreateKeyValue(string key)
        {
            return database.StringSet(key, 0);
        }

        /// <summary>
        /// 获取key的值
        /// </summary>
        public static int GetStringKey(string key)
        {
            int value = (int)database.StringGet(key);
            return value;
        }

        /// <summary>
        /// 设置过期时间，单位s
        /// </summary>
        public static bool SetExpiry(string key, int expiryTime)
        {
            return database.KeyExpire(key, TimeSpan.FromSeconds(expiryTime));
        }


        /// <summary>
        /// 自增value
        /// </summary>
        public static long StringIncrement(string key)
        {
            return database.StringIncrement(key);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyDelete(string key)
        {
            return database.KeyDelete(key);
        }

        /// <summary>
        /// 返回剩余 expiryTime
        /// </summary>
        public static int GetKeyExpiryTime(string key)
        {

            int expiryTime = 0;
            var timeSpan = database.KeyTimeToLive(key);
            expiryTime = Convert.ToInt32(TimeSpan.Parse(timeSpan.ToString()).TotalSeconds);
            Console.WriteLine(expiryTime);

            return expiryTime;
        }

        // 登录时候使用
        // 登录：存在value为 0，创建过期时间
        // 登录：value + 1
        // 登陆成功：将 key 删除
        static void Main(string[] args)
        {
            InitConnect();
        }
    }
}
