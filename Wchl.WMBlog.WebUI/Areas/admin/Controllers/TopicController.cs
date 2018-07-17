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
    public class TopicController : BaseController
    {
        ITopicServices TopicServive;
        ITopicDetailServices TopicDetailServive;
        public TopicController(ITopicServices TopicServive, ITopicDetailServices TopicDetailServive)
        {
            this.TopicServive = TopicServive;
            this.TopicDetailServive = TopicDetailServive;
        }
        // GET: admin/Topic
        public ActionResult Index()
        {
            ViewBag.UserName = LoginUser.uRealName;
            return View();
        }

        public ActionResult TopicArticle(int id = 0)
        {
            ViewBag.UserName = LoginUser.uRealName;
            return View();
        }

        public ActionResult Add(int id = 0)
        {
            ViewBag.UserName = LoginUser.uRealName;
            Topic Topic = new Topic();
            if (id > 0)
            {
                Topic = TopicServive.QueryWhere(a => a.Id == id).FirstOrDefault();
            }
            return View(Topic);
        }
        public ActionResult AddArticle(int id = 0)
        {
            ViewBag.UserName = LoginUser.uRealName;
            TopicDetail Topic = new TopicDetail();
            ViewBag.topic = TopicServive.QueryWhere(a => !a.tIsDelete).ToList();
            if (id > 0)
            {
                Topic = TopicDetailServive.QueryWhere(a => a.Id == id).FirstOrDefault();
            }
            return View(Topic);
        }

        //新增博文
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Topic Topic)
        {
            if (Topic.Id > 0)
            {
                Topic model = TopicServive.QueryWhere(a => a.Id == Topic.Id).FirstOrDefault();

                if (model != null)
                {
                    model.tDetail = Topic.tDetail;
                    model.tName = Topic.tName;
                    model.tSectendDetail = Topic.tSectendDetail;
                    model.tLogo = Topic.tLogo;

                    TopicServive.Edit(model);
                    TopicServive.SaverChanges();
                    return Content("<script type='text/javascript'>alert('厉害啦！更新成功!');window.location='/admin/Topic/Index';</script>");
                }
                else
                {

                    return Content("<script type='text/javascript'>alert('错啦错啦，没这个数据!');window.location='/admin/Topic/Add';</script>");
                }
            }
            else
            {

                Topic.tCreatetime = DateTime.Now;
                Topic.tAuthor = LoginUser.uRealName;
                TopicServive.Add(Topic);
                TopicServive.SaverChanges();
                return Content("<script type='text/javascript'>alert('厉害啦！添加成功!');window.location='/admin/Topic/Index';</script>");

            }
        }


        //新增博文
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddArticle(TopicDetail Topic)
        {
            if (Topic.Id > 0)
            {
                TopicDetail model = TopicDetailServive.QueryWhere(a => a.Id == Topic.Id).FirstOrDefault();

                if (model != null)
                {
                    model.tdDetail = Topic.tdDetail;
                    model.tdName = Topic.tdName;
                    model.tdSectendDetail = Topic.tdSectendDetail;
                    model.tdLogo = Topic.tdLogo;
                    model.TopicId = Topic.TopicId;
                    model.tdContent = Topic.tdContent;
                    model.tdTop = Topic.tdTop;

                    TopicDetailServive.Edit(model);
                    TopicDetailServive.SaverChanges();
                    return Content("<script type='text/javascript'>alert('厉害啦！更新成功!');window.location='/admin/Topic/TopicArticle';</script>");
                }
                else
                {

                    return Content("<script type='text/javascript'>alert('错啦错啦，没这个数据!');window.location='/admin/Topic/AddArticle';</script>");
                }
            }
            else
            {

                Topic.tdCreatetime = DateTime.Now;
                TopicDetailServive.Add(Topic);
                TopicDetailServive.SaverChanges();
                return Content("<script type='text/javascript'>alert('厉害啦！添加成功!');window.location='/admin/Topic/TopicArticle';</script>");

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
            var userInfoList = TopicServive.QueryByBeginPage<int>(pageIndex, pageSize, out totalCount, r => !r.tIsDelete, r => r.Id, true);
            var temp = from u in userInfoList
                       select new { ID = u.Id, bsubmitter = u.tAuthor, btitle = u.tName, bcategory = u.tDetail, subtime = u.tCreatetime, Remark = u.tLogo };
            var jsonDataTemp = new
            {
                data = temp.ToList(),
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount

            };
            return Json(jsonDataTemp, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataDetail()
        {
            int pageIndex = Request["start"] != null ? int.Parse(Request["start"]) : 1;
            int pageSize = Request["length"] != null ? int.Parse(Request["length"]) : 5;
            int draw = Request["draw"] != null ? int.Parse(Request["draw"]) : 1;
            int tid = Request["tid"] != null ? int.Parse(Request["tid"]) : 0;
            int totalCount;
            var userInfoList = TopicDetailServive.QueryByBeginPage<int>(pageIndex, pageSize, out totalCount, r => !r.tdIsDelete && (tid > 0 ? (r.TopicId == tid) : true), r => r.Id, true);
            var temp = from u in userInfoList
                       select new { ID = u.Id, bsubmitter = u.Topic.tName, btitle = u.tdName, bcategory = u.tdDetail, subtime = u.tdCreatetime, Remark = u.tdLogo };
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