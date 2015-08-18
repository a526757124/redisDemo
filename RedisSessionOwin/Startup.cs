using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisSessionOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.UseRedisSession();
        }
    }
}
