#region USINGS

using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
#endregion

namespace RedisSessionOwin
{
    public class RedisSessionMiddleware : OwinMiddleware
    {
        /// <summary>
        /// 下一个“中间件”对象
        /// </summary>
        OwinMiddleware _next;

        /// <summary>
        /// 构造函数，第一个参数必须为 OwinMiddleware对象
        /// </summary>
        /// <param name="next">下一个中间件</param>
        public RedisSessionMiddleware(OwinMiddleware next)
            : base(next)
        {
            _next = next;
            //第一个参数是固定的，后边还可以添加自定义的其它参数
        }

        /// <summary>
        /// 处理用户请求的具体方法，该方法是必须的
        /// </summary>
        /// <param name="c">OwinContext对象</param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {
            if (_next != null && context.Request == null) { return _next.Invoke(context); }

            #region request
            var sessionID = context.Request.Cookies["Redis_SessionId"];
            RedisSession redisSession = new RedisSession(context, true, 300);
            string sessionStr = "未登录";
            if (!string.IsNullOrEmpty(sessionID))
            {
                if (redisSession[sessionID] != null)
                {

                    var userStr = redisSession[sessionID];
                    var user = JsonConvert.DeserializeObject<User>(userStr.ToString());
                    sessionStr = user.Name + " 已登录";
                }
                else
                {
                    redisSession.Add(sessionID, JsonConvert.SerializeObject(new User() { ID = 1, Name = "张三" }));
                }
            }
            else
            {
                //sessionID = redisSession.SessionID;
                redisSession.Add(sessionID, JsonConvert.SerializeObject(new User() { ID = 1, Name = "张三" }));
            }

            #endregion


            #region response
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.Cookies.Append("Redis_SessionId", sessionID);

            string outString = "<html><head><title>Jexus Owin Web Server</title></head><body>Jexus Owin Server!<br /><h2>" + sessionStr + "</h2>\r\n</body></html>";
            var outBytes = Encoding.UTF8.GetBytes(outString);
            context.Response.Write(outBytes, 0, outBytes.Length);
            #endregion


            return Task.FromResult<int>(0);
        }
    }
}
