using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class PasswordLibMap : EntityTypeConfiguration<PasswordLib>
    {
        public PasswordLibMap()
        {
            // Primary Key
            this.HasKey(t => t.PLID);

            this.Property(t => t.plURL)
                .HasMaxLength(200);
            this.Property(t => t.plPWD)
                .HasMaxLength(100);
            this.Property(t => t.plAccountName)
                .HasMaxLength(200);
            this.Property(t => t.plHintPwd)
                .HasMaxLength(200);
            this.Property(t => t.plHintquestion)
                .HasMaxLength(200);
        }
    }
}
