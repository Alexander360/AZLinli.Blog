using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class TopicMap : EntityTypeConfiguration<Topic>
    {
        public TopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.tLogo).HasMaxLength(200);
            this.Property(t => t.tAuthor).HasMaxLength(200);
            this.Property(t => t.tName).HasMaxLength(200);
            this.Property(t => t.tDetail).HasMaxLength(400);
            this.Property(t => t.tSectendDetail).HasMaxLength(200);

        }
    }
}
