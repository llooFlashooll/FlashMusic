using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlashMusic.Utils
{
    public class RedisHelper
    {
        private ConnectionMultiplexer redis = null;
        private IDatabase database = null;

        public RedisHelper(string RedisConnectionString)
        {
            // InitConnect(Configuration);
            try
            {
                // var RedisConnection = Configuration.GetConnectionString("RedisConnectionString");
                var RedisConnection = RedisConnectionString;
                redis = ConnectionMultiplexer.Connect(RedisConnection);
                database = redis.GetDatabase();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                redis = null;
                database = null;
            }
        }

        // 初始化连接connection
/*        public static void InitConnect(IConfiguration Configuration)
        {
            try
            {
                // var RedisConnection = "119.3.254.34, password=123456, DefaultDatabase=0";
                var RedisConnection = Configuration.GetConnectionString("RedisConnectionString");
                redis = ConnectionMultiplexer.Connect(RedisConnection);
                database = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                redis = null;
                database = null;
            }
        }*/

        /// <summary>
        /// 查询键值是否在Redis中存在
        /// </summary>
        public bool Exist(string key)
        {
            bool flag = true;
            if (!database.KeyExists(key))
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 创建键值对
        /// </summary>
        public bool CreateKeyValue(string key)
        {
            return database.StringSet(key, 0);
        }

        /// <summary>
        /// 获取key的值
        /// </summary>
        public int GetStringKey(string key)
        {
            int value = (int)database.StringGet(key);
            return value;
        }

        /// <summary>
        /// 设置过期时间，单位s
        /// </summary>
        public bool SetExpiry(string key, int expiryTime)
        {
            return database.KeyExpire(key, TimeSpan.FromSeconds(expiryTime));
        }

        /// <summary>
        /// 返回剩余 expiryTime，单位s
        /// </summary>
        public int GetKeyExpiryTime(string key)
        {
            var timeSpan = database.KeyTimeToLive(key);
            int expiryTime = Convert.ToInt32(TimeSpan.Parse(timeSpan.ToString()).TotalSeconds);
            return expiryTime;
        }

        /// <summary>
        /// 自增value
        /// </summary>
        public long StringIncrement(string key)
        {
            return database.StringIncrement(key);
        }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool KeyDelete(string key)
        {
            return database.KeyDelete(key);
        }
    }
}
