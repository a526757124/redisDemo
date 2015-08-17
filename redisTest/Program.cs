using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redisTest
{
    class Program
    {
        static RedisClient redisClient = new RedisClient("192.168.2.35", 6379);//redis服务IP和端口
        static RedisClient slaveRedisClient = new RedisClient("192.168.2.35", 6380);//redis服务IP和端口

        static void Main(string[] args)
        {

            ////会往主服务里面写入
            //RedisBase.Hash_Set<string>("PooledRedisClientManager", "one", "123");

            ////从服务里面读取信息
            //RedisBase.Hash_Get<string>("PooledRedisClientManager", "one");




            redisClient.Set("b", "5秒过期", new TimeSpan(0, 0, 5));
            Console.WriteLine("休眠前 b:{0}", Encoding.UTF8.GetString(redisClient.Get("b")));
            Thread.Sleep(4000);
            Console.WriteLine("休眠后 b:{0}", Encoding.UTF8.GetString(redisClient.Get("b")));

            User user1 = new User() { ID = 1, Name = "test1" };

            User user2 = new User() { ID = 2, Name = "test2" };
            var redisUsers = redisClient.As<User>();
            redisUsers.StoreAll(new[] { user1, user2 });
            ReadRedis(redisClient);
            ReadRedis(slaveRedisClient);
            //redisClient.SaveAsync();

            Console.WriteLine("----------------------------------------------");
            var o = redisUsers.GetAll().Where(p => p.ID == 1);
            Console.ReadKey();
        }
        static void ReadRedis(RedisClient client)
        {
            Console.WriteLine(BitConverter.ToString(client.Get("city")));

            Console.WriteLine(Encoding.UTF8.GetString(client.Get("a")));
        }
    }
}
