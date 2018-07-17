using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common;
using Wchl.WMBlog.Common.Cache;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.WebCore;

namespace Wchl.WMBlog.WebUI.Areas.admin.Controllers
{

    public class HomeController : BaseController
    {
        private readonly ICacheManager cacheManager;

        public HomeController(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }
        // GET: admin/Home
        public ActionResult Index()
        {
            ViewBag.UserName = LoginUser.uRealName;
            return View();
        }

    }
}