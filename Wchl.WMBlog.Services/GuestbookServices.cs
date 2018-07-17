using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Common.ToolsHelper;
using Wchl.WMBlog.IRepository;
using Wchl.WMBlog.IServices;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;
using Wchl.WMBlog.Services.Base;

namespace Wchl.WMBlog.Services
{
    public class GuestbookServices : BaseServices<Guestbook>, IGuestbookServices
    {
        IGuestbookRepository dal;

        public GuestbookServices(IGuestbookRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<GuestbookViewModels> getGuestbook(int id)
        {
            var models = dal.QueryOrderBy(c => c.blogId == id, c => c.createdate, false);
            //AutoMapper自动映射
            Mapper.Initialize(cfg => cfg.CreateMap<List<Guestbook>, List<GuestbookViewModels>>());
            List<GuestbookViewModels> viewmodels = Mapper.Map<List<Guestbook>, List<GuestbookViewModels>>(models);
            return  viewmodels;
        }
    }
}
