using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class sysUserInfoMap: EntityTypeConfiguration<sysUserInfo>
    {
        public sysUserInfoMap()
        {
            this.HasKey(u => u.uID);
            this.Property(u => u.uLoginName).HasMaxLength(60);
            this.Property(u => u.uLoginPWD).HasMaxLength(60);
            this.Property(u => u.uRealName).HasMaxLength(60);
        }
    }
}
