using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.WebCore;

namespace Wchl.WMBlog.WebUI.Areas.admin.Controllers
{
    public class UserInfoController : BaseController
    {
        IsysUserInfoServices UserInfoService;

        public UserInfoController(IsysUserInfoServices UserInfoService)
        {
            this.UserInfoService = UserInfoService;
        }
        // GET: admin/UserInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(sysUserInfo model)
        {

            if (model != null)
            {
                model.uCreateTime = DateTime.Now;
                model.uLastErrTime = DateTime.Now;
                model.uUpdateTime = DateTime.Now;
                UserInfoService.Add(model);
                UserInfoService.SaverChanges();
                return Content("<script type='text/javascript'>alert('恭喜！添加成功└(^o^)┘!');window.parent.location.reload();parent.layer.close(index);</script>");
            }
            else {


                return Content("<script type='text/javascript'>alert('好像哪里出错 了，咋办 ╮(╯_╰)╭!');;</script>");
            }
        }
        public ActionResult GetData()
        {
            int pageIndex = Request["start"] != null ? int.Parse(Request["start"]) : 1;
            int pageSize = Request["length"] != null ? int.Parse(Request["length"]) : 5;
            int draw = Request["draw"] != null ? int.Parse(Request["draw"]) : 1;
            int totalCount;
            short delFlag = 0;
            var userInfoList = UserInfoService.QueryByBeginPage<int>(pageIndex, pageSize, out totalCount, r => r.uStatus == delFlag, r => r.uID, true);
            var temp = from u in userInfoList
                       select new { ID = u.uID, UserName = u.uLoginName,UserPwd = u.uLoginPWD, RealName=u.uRealName, subtime = u.uCreateTime, Remark = u.uRemark };
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