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
    public class TopicController : BaseController
    {
        ITopicServices TopicServive;
        ITopicDetailServices TopicDetailServive;

        public TopicController(ITopicServices TopicServive, ITopicDetailServices TopicDetailServive)
        {
            this.TopicServive = TopicServive;
            this.TopicDetailServive = TopicDetailServive;
        }

        // GET: Blog
        public ActionResult Index(int id = 1, string bcategory = "技术博文")
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();
            int pagesize = 6;
            var blogArticleList = TopicServive.QueryOrderBy(a => !a.tIsDelete, a => a.tCreatetime, false).ToPagedList(id, pagesize);

            return View(blogArticleList);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id = 0)
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();
            int pagesize = 6;
            var blogArticleList = TopicServive.QueryWhere(a => !a.tIsDelete && a.Id == id).FirstOrDefault();
            foreach (var item in blogArticleList.TopicDetail)
            {
                if (!string.IsNullOrEmpty(item.tdContent))
                {
                    item.tdContent = Tools.ReplaceHtmlTag(item.tdContent);
                    if (item.tdContent.Length > 200)
                    {
                        item.tdContent = item.tdContent.Substring(0, 200);
                    }
                }
            }
            return View(blogArticleList.TopicDetail.OrderByDescending(b => b.tdTop).ThenByDescending(c => c.Id).ToList());
        }
        public ActionResult ArticleDetail(int id = 0)
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();
            int pagesize = 6;
            var blogArticleList = TopicDetailServive.QueryWhere(a => !a.tdIsDelete && a.Id == id).FirstOrDefault();
            if (blogArticleList != null)
            {
                blogArticleList.tdRead++;
                TopicDetailServive.Edit(blogArticleList);
                TopicDetailServive.SaverChanges();
            }

            return View(blogArticleList);
        }

    }
}