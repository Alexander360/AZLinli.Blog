﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;
using Wchl.WMBlog.WebCore;
using Webdiyer.WebControls.Mvc;

namespace Wchl.WMBlog.WebUI.Areas.admin.Controllers
{
    public class AdvertisementController : BaseController
    {
        IAdvertisementServices AdvertisementServices;
        public AdvertisementController(IAdvertisementServices AdvertisementServices)
        {
            this.AdvertisementServices = AdvertisementServices;
        }

        // GET: admin/Advertisement
        public ActionResult Index()
        {
            ViewBag.UserName = LoginUser.uRealName;
            return View();
        }

        /// <summary>
        /// 获取广告信息
        /// </summary>
        /// <returns></returns>
        public ActionResult getData()
        {
            int pageIndex = Request["start"] != null ? int.Parse(Request["start"]) : 1;
            int pageSize = Request["length"] != null ? int.Parse(Request["length"]) : 5;
            int draw = Request["draw"] != null ? int.Parse(Request["draw"]) : 1;
            int totalCount;
            int count = 0;
            var adInfoList = AdvertisementServices.QueryByBeginPage(pageIndex, pageSize, out totalCount, r =>true , r => r.Createdate, false);
            var temp = from u in adInfoList
                       select new { count= count+=1, ID = u.Id, Title = u.Title, ImgUrl = u.ImgUrl, Createdate = u.Createdate,url=u.Url, Remark = u.Remark };
            return Json(new {
                data = temp.ToList(),
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount
            },JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除广告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult del(int id)
        {
            Advertisement model = new Advertisement() {Id=id };
            AdvertisementServices.Delete(model, false);
            AdvertisementServices.SaverChanges();
            return Json(new { status = "0" },JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加广告信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(AdvertisementViewModels model)
        {
            model.Createdate = DateTime.Now;
            //AutoMapper自动映射
            Mapper.Initialize(cfg => cfg.CreateMap<AdvertisementViewModels, Advertisement>());
            Advertisement models = Mapper.Map<AdvertisementViewModels, Advertisement>(model);
            AdvertisementServices.Add(models);
            AdvertisementServices.SaverChanges();
            return Json(new { status="0" },JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 百度上传图片
        /// </summary>
        /// <param name="id">百度插件自定义对图片的命名id</param>
        /// <param name="name">图片名称</param>
        /// <param name="type">图片类型</param>
        /// <param name="lastModifiedDate">图片本身的修改时间</param>
        /// <param name="size">图片大小</param>
        /// <param name="file">文件流</param>
        /// <returns></returns>
        public ActionResult UpLoadProcess(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string filePathName = string.Empty;

            string localPath = Server.MapPath("/upload/");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "保存失败" }, id = "id" });
            }

            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;

            string datedir = DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(localPath + datedir))
            {
                Directory.CreateDirectory(localPath + datedir);
            }
            string path = localPath + datedir;
            try
            {
                file.SaveAs(Path.Combine(path, filePathName));
            }
            catch (Exception)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 103, message = "保存失败" }, id = "id" });
            }
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                filePath =  "/Upload/"+datedir + "/" + filePathName
            });

        }
    }
}