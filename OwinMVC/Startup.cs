using Microsoft.Owin;
using Owin;
using RedisSessionOwin;
[assembly: OwinStartupAttribute(typeof(OwinMVC.Startup))]
namespace OwinMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.UseRedisSession();
        }
    }
}
