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
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;

        public BlogArticleServices(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlogViewModels getBlogDetails(int id)
        {
            var bloglist = dal.QueryWhere(a => a.bID > 0).OrderBy(d => d.bID).ToList();
            var idmin = bloglist.FirstOrDefault() != null ? bloglist.FirstOrDefault().bID : 0;
            var idmax = bloglist.LastOrDefault() != null ? bloglist.LastOrDefault().bID : 1;
            var idminshow = id;
            var idmaxshow = id;

            BlogArticle blogArticle = new BlogArticle();

            blogArticle = dal.QueryWhere(a => a.bID == idminshow).FirstOrDefault();

            BlogArticle prevblog = new BlogArticle();


            while (idminshow > idmin)
            {
                idminshow--;
                prevblog = dal.QueryWhere(a => a.bID == idminshow).FirstOrDefault();
                if (prevblog != null)
                {
                    break;
                }
            }

            BlogArticle nextblog = new BlogArticle();
            while (idmaxshow < idmax)
            {
                idmaxshow++;
                nextblog = dal.QueryWhere(a => a.bID == idmaxshow).FirstOrDefault();
                if (nextblog != null)
                {
                    break;
                }
            }


            blogArticle.btraffic += 1;
            dal.Edit(blogArticle, new string[] { "btraffic" });
            dal.SaverChanges();
            //AutoMapper自动映射
            Mapper.Initialize(cfg => cfg.CreateMap<BlogArticle, BlogViewModels>());
            BlogViewModels models = Mapper.Map<BlogArticle, BlogViewModels>(blogArticle);
            if (nextblog != null)
            {
                models.next = nextblog.btitle;
                models.nextID = nextblog.bID;
            }

            if (prevblog != null)
            {
                models.previous = prevblog.btitle;
                models.previousID = prevblog.bID;
            }
            models.digest = Tools.ReplaceHtmlTag(blogArticle.bcontent).Length > 100 ? Tools.ReplaceHtmlTag(blogArticle.bcontent).Substring(0, 200) : Tools.ReplaceHtmlTag(blogArticle.bcontent);
            return models;

        }


    }
}
