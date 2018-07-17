using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.IServices.Base;
using Wchl.WMBlog.Model.Models;
using Wchl.WMBlog.Model.VeiwModels;

namespace Wchl.WMBlog.IServices
{
    public interface IBlogArticleServices: IBaseServices<BlogArticle>
    {
        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BlogViewModels getBlogDetails(int id);
    }
}
