using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using Microsoft.Owin;

namespace RedisSessionOwin
{
    public class RedisSession
    {
        //private static RedisClient client = new RedisClient("127.0.0.1", 6379);

        private IOwinContext _context;

        public RedisSession(IOwinContext context, bool IsReadOnly, int Timeout)
        {
            this._context = context;
            this.IsReadOnly = IsReadOnly;
            this.Timeout = Timeout;
            //client.ExpireEntryIn(SessionID, TimeSpan.FromMinutes(Timeout));
            RedisBase.SortedSet_SetExpire(SessionID, TimeSpan.FromMinutes(Timeout));
        }

        /// <summary>
        /// SessionId标识符
        /// </summary>
        public static string SessionName = "Redis_SessionId";

        //
        // 摘要:
        //     获取会话状态集合中的项数。
        //
        // 返回结果:
        //     集合中的项数。
        public long Count
        {
            get
            {
                return RedisBase.prcm.GetClient().GetHashCount(SessionID);
                //return client.GetHashCount(SessionID);
                //return RedisBase.prcm.GetClient().GetSetCount(SessionID);
            }
        }

        //
        // 摘要:
        //     获取一个值，该值指示会话是否为只读。
        //
        // 返回结果:
        //     如果会话为只读，则为 true；否则为 false。
        public bool IsReadOnly { get; set; }

        //
        // 摘要:
        //     获取会话的唯一标识符。
        //
        // 返回结果:
        //     唯一会话标识符。
        public string SessionID
        {
            get
            {
                return GetSessionID();
            }
        }

        //
        // 摘要:
        //     获取并设置在会话状态提供程序终止会话之前各请求之间所允许的时间（以分钟为单位）。
        //
        // 返回结果:
        //     超时期限（以分钟为单位）。
        public int Timeout { get; set; }

        /// <summary>
        /// 获取SessionID
        /// </summary>
        /// <param name="key">SessionId标识符</param>
        /// <returns>HttpCookie值</returns>
        private string GetSessionID()
        {
            RequestCookieCollection requestCookie = this._context.Request.Cookies;
            ResponseCookieCollection responseCookie = this._context.Response.Cookies;

            if (!requestCookie.Any(o => o.Key.Equals(SessionName)) || string.IsNullOrEmpty(requestCookie[SessionName]))
            {
                string newSessionID = Guid.NewGuid().ToString();
                //HttpCookie newCookie = new HttpCookie(SessionName, newSessionID);
                //newCookie.HttpOnly = IsReadOnly;
                //newCookie.Expires = DateTime.Now.AddMinutes(Timeout);
                _context.Response.Cookies.Append(SessionName, newSessionID);
                return "Session_" + newSessionID;
            }
            else
            {
                return requestCookie[SessionName];
            }
        }

        //
        // 摘要:
        //     按名称获取或设置会话值。
        //
        // 参数:
        //   name:
        //     会话值的键名。
        //
        // 返回结果:
        //     具有指定名称的会话状态值；如果该项不存在，则为 null。
        //public string this[string name]
        //{
        //    get
        //    {
        //        //return client.GetValueFromHash(SessionID, name);
        //        return RedisBase.Hash_Get<string>(SessionName, name);
        //    }
        //    set
        //    {
        //        //client.SetEntryInHash(SessionID, name, value);
        //        RedisBase.Hash_Set<string>(SessionName, name, value);
        //    }
        //}
        public object this[string name]
        {
            get
            {
                //return client.GetValueFromHash(SessionID, name);
                return RedisBase.Hash_Get<object>(SessionName, name);
            }
            set
            {
                //client.SetEntryInHash(SessionID, name, value);
                RedisBase.Hash_Set<object>(SessionName, name, value);
            }
        }

        //
        // 摘要:
        //     向会话状态集合添加一个新项。
        //
        // 参数:
        //   name:
        //     要添加到会话状态集合的项的名称。
        //
        //   value:
        //     要添加到会话状态集合的项的值。
        public void Add<T>(string name, T value)
        {
            //client.SetEntryInHash(SessionID, name, value);
            RedisBase.Hash_Set<T>(SessionName, name, value);
        }
        
        //
        // 摘要:
        //     从会话状态集合中移除所有的键和值。
        public void Clear()
        {
            //client.Remove(SessionID);
            RedisBase.Hash_Remove(SessionName);
        }

        //
        // 摘要:
        //     删除会话状态集合中的项。
        //
        // 参数:
        //   name:
        //     要从会话状态集合中删除的项的名称。
        public void Remove(string name)
        {
            //client.RemoveEntryFromHash(SessionID, name);
            RedisBase.Hash_Remove(SessionName, name);
        }
        //
        // 摘要:
        //     从会话状态集合中移除所有的键和值。
        public void RemoveAll()
        {
            Clear();
        }
    }
}