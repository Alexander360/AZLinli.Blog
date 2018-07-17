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
    public class BlogController : BaseController
    {
        IBlogArticleServices BlogArticleServive;
        IGuestbookServices GuestbookServices;

        public BlogController(IBlogArticleServices BlogArticleServive, IGuestbookServices GuestbookServices)
        {
            this.BlogArticleServive = BlogArticleServive;
            this.GuestbookServices = GuestbookServices;

        }
        // GET: Blog
        public ActionResult Index(int id = 1,string bcategory= "技术博文")
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();
            int pagesize = 6;
            var blogArticleList = BlogArticleServive.QueryOrderBy(a => a.bcategory == bcategory, a=>a.bCreateTime,false).ToPagedList(id,pagesize);
            foreach (var item in blogArticleList)
            {
                if (!string.IsNullOrEmpty(item.bcontent))
                {
                    item.bcontent = Tools.ReplaceHtmlTag(item.bcontent);
                    if (item.bcontent.Length > 200)
                    {
                        item.bcontent = item.bcontent.Substring(0, 200);
                    }
                }
            }
            //发布时间排序
            ViewBag.blogtimelist = BlogArticleServive.QueryOrderBy(c => c.bcategory == bcategory, c => c.bCreateTime, false);
            //评论排序
            ViewBag.blogtrafficlist = BlogArticleServive.QueryOrderBy(c => c.bcategory == bcategory, c => c.btraffic, false);
            //留言排序
            string sql = @"select a.*,b.btitle from (select blogId,count(1) as counts  from Guestbook group by blogId) as a
inner join BlogArticle as b
on
b.bID=a.blogId order by counts desc";

            ViewBag.blogguestbooklist = GuestbookServices.RunProc<TopgbViewModels>(sql);


            if (Request.IsAjaxRequest())
                return PartialView("_ArticleList", blogArticleList);
            return View(blogArticleList);
        }

        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();
            var model = BlogArticleServive.getBlogDetails(id);
            ViewBag.gblist = GuestbookServices.QueryOrderBy(c => c.blogId == id, c => c.createdate, false).ToPagedList(1, 5);

            //发布时间排序
            ViewBag.blogtimelist = BlogArticleServive.QueryOrderBy(c => true, c => c.bCreateTime, false);
            //评论排序
            ViewBag.blogtrafficlist = BlogArticleServive.QueryOrderBy(c => true, c => c.btraffic, false);
            //留言排序
            string sql = @"select a.*,b.btitle from (select blogId,count(1) as counts  from Guestbook group by blogId) as a
                inner join BlogArticle as b
                on
                b.bID=a.blogId order by counts desc";

            ViewBag.blogguestbooklist = GuestbookServices.RunProc<TopgbViewModels>(sql);

            return View(model);
        }

        /// <summary>
        ///提交评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addGuestbook(GuestbookViewModels model)
        {
            model.createdate = DateTime.Now;
            model.ip = Request.UserHostAddress;
            //AutoMapper自动映射
            Mapper.Initialize(cfg => cfg.CreateMap<GuestbookViewModels, Guestbook > ());
            Guestbook models = Mapper.Map< GuestbookViewModels, Guestbook> (model);
            BlogArticle blogArticle = BlogArticleServive.QueryWhere(a => a.bID == model.blogId).FirstOrDefault();
            blogArticle.bcommentNum += 1;
            BlogArticleServive.SaverChanges();
            GuestbookServices.Add(models);
            GuestbookServices.SaverChanges();
            ViewBag.gblist = GuestbookServices.QueryOrderBy(c => c.blogId == model.blogId, c => c.createdate, false).ToPagedList(1, 5);
            return PartialView("_GuestbookPage");
        }

        /// <summary>
        /// js分页实现
        /// </summary>
        /// <param name="page"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public ActionResult getGuestbook(int page,int blogId)
        {
            StringBuilder sb = new StringBuilder();
            int pages = 0;
            var modelsLists = GuestbookServices.QueryByPage(page, 5,out pages, c => c.blogId == blogId, c => c.createdate, false);
            foreach (var item in modelsLists)
            {
                sb.AppendFormat(@"<div class='comment'>
<a href = '###' class='avatar'>
<i class='icon-user icon-2x'></i>
</a>
<div class='content'>
<div class='pull-right text-muted'>{0}</div>
<div><a href = '###' ><strong > {1}</strong ></a ></div>
<div class='text'>{2}</div>
<div class='actions'>
<a href = '##' > 回复 </a>
</div >
</div >
</div >",item.createdate,item.username,item.body);
            }
            if (pages % 5 == 0)
            {
                pages = pages / 5;
            }
            else
            {
                pages = (pages / 5)+1;
            }


            return Json(new {
                content = sb.ToString(),
                pages= pages
            });
        }
    }
}