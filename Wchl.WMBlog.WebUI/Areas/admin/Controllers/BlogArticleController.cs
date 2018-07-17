using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.Common.Cache;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.WebCore;

namespace Wchl.WMBlog.WebUI.Areas.admin.Controllers
{
    public class BlogArticleController : BaseController
    {
        IBlogArticleServices BlogArticleServive;
        public BlogArticleController(IBlogArticleServices BlogArticleServive)
        {
            this.BlogArticleServive = BlogArticleServive;
        }
        // GET: admin/BlogArticle
        public ActionResult Index()
        {
            ViewBag.UserName = LoginUser.uRealName;

            var BlogArticles = BlogArticleServive.QueryWhere(a => true).OrderByDescending(a => a.bCreateTime);
            return View(BlogArticles);
        }

        public ActionResult Add(int id = 0)
        {
            ViewBag.UserName = LoginUser.uRealName;

            BlogArticle blogArticle = new BlogArticle();
            if (id > 0)
            {
                blogArticle = BlogArticleServive.QueryWhere(a => a.bID == id).FirstOrDefault();
            }
            return View(blogArticle);
        }

        //新增博文
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(BlogArticle blogArticle)
        {
            if (blogArticle.bID > 0)
            {
                BlogArticle model = BlogArticleServive.QueryWhere(a => a.bID == blogArticle.bID).FirstOrDefault();

                if (model!=null)
                {
                    model.btitle = blogArticle.btitle;
                    model.bcategory = blogArticle.bcategory;
                    model.bcontent = blogArticle.bcontent;

                    BlogArticleServive.Edit(model);
                    BlogArticleServive.SaverChanges();
                    return Content("<script type='text/javascript'>alert('厉害啦！更新成功!');window.location='/admin/BlogArticle/Index';</script>");
                }
                else
                {

                    return Content("<script type='text/javascript'>alert('错啦错啦，没这个数据!');window.location='/admin/BlogArticle/Add';</script>");
                }
            }
            else {

                blogArticle.bCreateTime = DateTime.Now;
                blogArticle.bsubmitter = "admin";
                blogArticle.bUpdateTime = DateTime.Now;
                blogArticle.bRemark = string.Empty;
                BlogArticleServive.Add(blogArticle);
                BlogArticleServive.SaverChanges();
                return Content("<script type='text/javascript'>alert('添加成功!');window.location='/admin/BlogArticle/Add';</script>");

            }
        }

        //图片上传
        public ActionResult upload()
        {
            //文件保存目录路径
            String savePath = "/upload/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;

            HttpPostedFileBase imgFile = Request.Files["imgFile"];
            if (imgFile == null)
            {
                return Content("error|请选择文件。");
            }

            String dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                return Content("error|上传目录不存在。");
            }

            String dirName = Request.QueryString["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return Content("error|目录名不正确。");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Content("error|上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Content("error|上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹
            dirPath += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = savePath + "image/" + ymd + "/" + newFileName;
            return Content(fileUrl);
        }

        public ActionResult GetData()
        {
            int pageIndex = Request["start"] != null ? int.Parse(Request["start"]) : 1;
            int pageSize = Request["length"] != null ? int.Parse(Request["length"]) : 5;
            int draw = Request["draw"] != null ? int.Parse(Request["draw"]) : 1;
            int totalCount;
            short delFlag = 0;
            var userInfoList = BlogArticleServive.QueryByBeginPage<int>(pageIndex, pageSize, out totalCount, r => r.bID > delFlag, r => r.bID, true);
            var temp = from u in userInfoList
                       select new { ID = u.bID, bsubmitter = u.bsubmitter, btitle = u.btitle, bcategory = u.bcategory, subtime = u.bCreateTime, Remark = u.bRemark };
            var jsonDataTemp = new
            {
                data = temp.ToList(),
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount

            };
            return Json(jsonDataTemp, JsonRequestBehavior.AllowGet);
        }
    }
}