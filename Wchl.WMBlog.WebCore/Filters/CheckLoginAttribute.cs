using Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common;
using Wchl.WMBlog.Common.Cache;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.WebCore.Attrs;

namespace Wchl.WMBlog.WebCore.Filters
{
   public class CheckLoginAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            //从缓存中获取autofac的容器对象
            var cont = CacheMgr.GetData<IContainer>(Keys.AutofacContainer);
            //获取到依赖注入数据
            ICacheManager cacheManager = cont.Resolve<ICacheManager>();

            if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipCheckLogin), false))
            {
                return;
            }
            if (filterContext.ActionDescriptor.IsDefined(typeof(SkipCheckLogin), false))
            {
                return;
            }
            //判断session是否为null
            if (filterContext.HttpContext.Request.Cookies[Keys.uinfo] == null)
            {
                if (filterContext.HttpContext.Request.Cookies[Keys.IsMember] != null)
                {
                    //取出cookie中存入的uid的值
                    string uid = filterContext.HttpContext.Request.Cookies[Keys.IsMember].Value;
              
                    //获取到依赖注入数据
                    IsysUserInfoServices userSer = cont.Resolve<IsysUserInfoServices>();

                    int iuserid = int.Parse(uid);
                    var userinfo = userSer.QueryWhere(c => c.uID == iuserid).FirstOrDefault();
                    if (userinfo != null)
                    {
                        //将userinfo存入session
                        //filterContext.HttpContext.Session[Keys.uinfo] = userinfo;

                        //改用redis存储用户信息
                   
                        //改用redis缓存
                        string sessionId = Guid.NewGuid().ToString("N");//必须保证Memcache的key唯一
                        cacheManager.Set(sessionId, userinfo, TimeSpan.FromHours(1));
                        //filterContext.HttpContext.Request.Cookies[Keys.uinfo].Value = sessionId;
                        context.Response.Cookies[Keys.uinfo].Value = sessionId;//将自创的用户信息以Cookie的形式返回给浏览器。
                        BaseController.LoginUser = userinfo;
                    }
                    else
                    {
                        ToLogin(filterContext);
                    }
                }
                else
                {
                    ToLogin(filterContext);
                }
            }
            else
            {
                string sessionId = filterContext.HttpContext.Request.Cookies[Keys.uinfo].Value;
                sysUserInfo obj = cacheManager.Get<sysUserInfo>(sessionId);//获取Memcache中的数据.
                if (obj != null)
                {
                    BaseController.LoginUser = obj;
                    //模拟滑动过期时间。
                    cacheManager.Set(sessionId, obj, TimeSpan.FromHours(1));
                }
                else
                {
                    ToLogin(filterContext);
                }
            }
        }

        private static void ToLogin(ActionExecutingContext filterContext)
        {
            bool isAjaxRequst = filterContext.HttpContext.Request.IsAjaxRequest();
            if (isAjaxRequst)
            {
                JsonResult json = new JsonResult();
                json.Data = new { status = Enums.EAjaxState.nologin, msg = "您未登录或者登录已经失效，请重新登录" };
                json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = json;
            }
            else
            {
                ViewResult view = new ViewResult();
                view.ViewName = "/Areas/admin/Views/Login/Index.cshtml";
                filterContext.Result = view;
            }
        }
    }
}
