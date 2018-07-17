using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wchl.WMBlog.Common
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class CacheMgr
    {
        /// <summary>
        /// 根据cacheKey获取缓存的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetData<T>(string cacheKey)
        {
            return (T)HttpRuntime.Cache[cacheKey];
        }

        public static void SetData(string cacheKey, object val)
        {
            HttpRuntime.Cache[cacheKey] = val;
        }
    }
}
