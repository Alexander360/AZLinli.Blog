using JiebaNet.Segmenter;
using SearchTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common.ToolsHelper;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;
using Wchl.WMBlog.Services;
using Wchl.WMBlog.WebCore;
using Wchl.WMBlog.WebCore.Attrs;
using Webdiyer.WebControls.Mvc;
using WiteemFramework.DBWorks;

namespace Wchl.WMBlog.WebUI.Controllers
{
    [SkipCheckLogin]
    public class HomeController : BaseController
    {
        IsysUserInfoServices userinfoservice;
        IBlogArticleServices BlogArticleServive;
        IAdvertisementServices AdvertisementServices;
        IGuestbookServices GuestbookServices;
        ITopicServices TopicServive;
        ITopicDetailServices TopicDetailServive;

        public HomeController(IsysUserInfoServices userinfs, IBlogArticleServices BlogArticleServive, IAdvertisementServices AdvertisementServices, IGuestbookServices GuestbookServices, ITopicServices TopicServive, ITopicDetailServices TopicDetailServive)
        {
            this.userinfoservice = userinfs;
            this.BlogArticleServive = BlogArticleServive;
            this.AdvertisementServices = AdvertisementServices;
            this.GuestbookServices = GuestbookServices;
            this.TopicServive = TopicServive;
            this.TopicDetailServive = TopicDetailServive;
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Search(string wd)
        {

            var content = wd;
            var str = "";
            var segmenter = new JiebaSegmenter();
            str += ($"原检索语句: {content}<br >");

            if (!string.IsNullOrEmpty(content))
            {
                //PanGuLuceneHelper.instance.DeleteAll();
                var segments4 = segmenter.CutForSearch(content); // 搜索引擎模式
                str += ($"【搜索引擎模式】：{ string.Join("/ ", segments4)}<br >");
                var topicList = TopicServive.QueryOrderBy(a => !a.tIsDelete, a => a.tCreatetime, false);
                var topicDetailList = TopicDetailServive.QueryOrderBy(a => !a.tdIsDelete, a => a.tdCreatetime, false);
                var blogArticleList = BlogArticleServive.QueryOrderBy(a => true, a => a.bCreateTime, false);

                List<MySearchUnit> list = new List<MySearchUnit>();
                foreach (var item in topicList)
                {
                    list.Add(new MySearchUnit("Topic" + item.Id, item.tName, item.tDetail, new Random().Next(1, 100).ToString(), "", item.tCreatetime.ToString("yyyy年MM月dd日")));
                }
                foreach (var item in topicDetailList)
                {
                    item.tdContent = Tools.ReplaceHtmlTag(item.tdContent);
                    list.Add(new MySearchUnit("TopicDetail" + item.Id, item.tdName, item.tdContent, new Random().Next(1, 100).ToString(), "", item.tdCreatetime.ToString("yyyy年MM月dd日")));
                }
                foreach (var item in blogArticleList)
                {
                    item.bcontent = Tools.ReplaceHtmlTag(item.bcontent);
                    list.Add(new MySearchUnit("BlogArtic" + item.bID, item.btitle, item.bcontent, new Random().Next(1, 100).ToString(), "", item.bCreateTime.ToString("yyyy年MM月dd日")));
                }

                PanGuLuceneHelper.instance.CreateIndex(list);//添加索引


                int count = 0;
                int PageIndex = 1;
                int PageSize = 100;
                string html_content = "";
                List<MySearchUnit> searchlist = PanGuLuceneHelper.instance.Search("", string.Join(" ", segments4.Where(d => d.Length >= 2)), PageIndex, PageSize, out count);
                List<string> idList = new List<string>();
                if (searchlist == null || searchlist.Count == 0)
                {
                    html_content += ("未查询到数据。<br/>");
                }
                else
                {
                    foreach (MySearchUnit data in searchlist)
                    {
                        if (!idList.Contains(data.id))
                        {
                            //html_content += (string.Format("id：{0},title：{1},content：{2},flag：{3},updatetime：{4}<br/>", data.id, data.title, data.content, data.flag, data.updatetime));
                            html_content += GetSearchHtml(data.id, data.title, data.content, data.updatetime);
                            idList.Add(data.id);
                        }
                    }
                }
                html_content += ("查询结果：" + idList.Count + "条数据<br/>");
                str += html_content;

            }
            else
            {
                return View("Home");
            }


            ViewBag.html = str;
            return View();
        }

        public string GetSearchHtml(string id, string title, string content, string date)
        {
            return $" <div class=\'result c-container \' id=\'3\' srcid=\'1599\' tpl=\'se_com_default\'>" +
"            <h3 class=\'t\'>" +
"                <a href = \'" + GetUrlDif(id) + "\' target=\'_blank\'>" + title + "</a>" +
"            </h3>" +
"            <div class=\'c-row c-gap-top-small\'>" +
"                <div class=\'general_image_pic c-span6\'>" +
"                    <a class=\'c-img6\' style=\'height:75px\' href=\'#\' target=\'_blank\'>" +
"                    </a>" +
"                </div>" +
"                <div class=\'c-span18 c-span-last\'>" +
"                    <div class=\'c-abstract\'>" +
"                        <span class=\' newTimeFactor_before_abs m\'>" + date + "&nbsp;-&nbsp;</span>" +
                        "" + content +
"                    </div>" +
"                    <div class=\'f13\'>" +
"                        <a target = \'_blank\' href=\'" + GetUrlDif(id) + "\' class=\'c-showurl\' style=\'text-decoration:none;\'>" + GetUrlDif(id) + "</a>" +
"                        <span class=\'c-icons-outer\'><span class=\'c-icons-inner\'></span></span>&nbsp;-&nbsp;" +
"                    </div>" +
"                </div>" +
"            </div>" +
"        </div>";

        }

        public string GetUrlDif(string id)
        {
            string Url = "";
            if (!string.IsNullOrEmpty(id))
            {
                if (id.Contains("Topic"))
                {
                    Url = "http://blog.azlinli.com/topic/detail/" + id.Replace("Topic", "");
                }
                if (id.Contains("TopicDetail"))
                {
                    Url = "http://blog.azlinli.com/topic/articledetail/" + id.Replace("TopicDetail", "");
                }
                if (id.Contains("BlogArtic"))
                {
                    Url = "http://blog.azlinli.com/blog/Detail/" + id.Replace("BlogArtic", "");
                }
            }
            return Url;
        }

        public ActionResult Index(int pageindex = 1)
        {
            //获取控制器名称
            ViewBag.controllername = RouteData.Values["controller"].ToString().ToLower();

            int pagesize = 6;
            //获取发布博文信息
            var blogArticleList = BlogArticleServive.QueryWhere(a => true).OrderByDescending(a => a.bCreateTime).ToPagedList(pageindex, pagesize);
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
            //获取轮播广告新
            ViewBag.adList = AdvertisementServices.QueryOrderBy(a => true, a => a.Createdate, false).ToPagedList(1, 3);
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


            return View(blogArticleList);
        }
    }
}