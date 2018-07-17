using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common;
using Wchl.WMBlog.Common.Cache;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.WebCore;
using Wchl.WMBlog.WebCore.Attrs;

namespace Wchl.WMBlog.WebUI.Areas.admin.Controllers
{

    [SkipCheckLogin]
    public class PasswordLibController : BaseController
    {
        private readonly ICacheManager cacheManager;

        public PasswordLibController(ICacheManager cacheManager)
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