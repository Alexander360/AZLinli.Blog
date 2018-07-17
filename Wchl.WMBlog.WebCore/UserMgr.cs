using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common;
using Wchl.WMBlog.Common.Cache;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.WebCore
{
     public class UserMgr
    {
        public static sysUserInfo GetCurrentUserInfo()
        {
            if (HttpContext.Current.Request.Cookies[Keys.uinfo] != null)
            {
                string sessionId = HttpContext.Current.Request.Cookies[Keys.uinfo].Value;
                //从缓存中获取autofac的容器对象
                var cont = CacheMgr.GetData<IContainer>(Keys.AutofacContainer);
                //获取到依赖注入数据
                ICacheManager cacheManager = cont.Resolve<ICacheManager>();
                return cacheManager.Get<sysUserInfo>(sessionId);
            }
            return  new sysUserInfo() { };
        }
    }
}
