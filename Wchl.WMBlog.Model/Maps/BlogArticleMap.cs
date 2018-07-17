using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class BlogArticleMap : EntityTypeConfiguration<BlogArticle>
    {
        public BlogArticleMap()
        {
            this.HasKey(p => p.bID);
            this.Property(p => p.btitle).HasMaxLength(256);
            this.Property(p => p.bsubmitter).HasMaxLength(60);
            this.Property(p => p.bcontent).HasColumnType("Text").IsMaxLength();
        }
    }
}
