using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class GuestbookMap: EntityTypeConfiguration<Guestbook>
    {
        public GuestbookMap() {
            this.HasKey(p => p.id);
            //设置外键
            this.HasRequired(p => p.blogarticle).WithMany().HasForeignKey(p => p.blogId);
        }
    }
}
