using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Wchl.WMBlog.Common.ToolsHelper;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;
using Wchl.WMBlog.WebCore;
using Wchl.WMBlog.WebUI.Models;
using Webdiyer.WebControls.Mvc;
using Wchl.WMBlog.WebCore.Attrs;

namespace Wchl.WMBlog.WebUI.Controllers
{
    [SkipCheckLogin]
    public class HireController : BaseController
    {
        IBlogArticleServices BlogArticleServive;

        public HireController(IBlogArticleServices BlogArticleServive)
        {
            this.BlogArticleServive = BlogArticleServive;

        }
        // GET: Blog
        public ActionResult Index(int id = 1)
        {
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();

            return View();
        }

     
    }
}