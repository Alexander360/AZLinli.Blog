using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Wchl.WMBlog.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //注册区域路由规则
            AreaRegistration.RegisterAllAreas();
            //注册全局过路器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //注册网站路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //优化js、css
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //利用autofac实现MVC项目的IOC和DI
            AutofacConfig.Register();

        }
    }
}
