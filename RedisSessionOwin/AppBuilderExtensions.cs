using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisSessionOwin
{
    /// <summary>
    /// 这个类是为AppBuilder添加一个名叫UseRedisSession的扩展方法，目的是方便用户调用
    /// </summary>
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseRedisSession(this IAppBuilder builder)
        {
            //USE可以带多个参数，对应中间件构造函数中的第2、3、....参数;
            return builder.Use<RedisSessionMiddleware>();
            
        }
    }
}
