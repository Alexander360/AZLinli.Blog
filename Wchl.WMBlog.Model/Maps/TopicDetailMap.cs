using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class TopicDetailMap : EntityTypeConfiguration<TopicDetail>
    {
        public TopicDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.tdLogo).HasMaxLength(200);
            this.Property(t => t.tdName).HasMaxLength(200);
            this.Property(t => t.tdDetail).HasMaxLength(400);
            this.Property(t => t.tdSectendDetail).HasMaxLength(200);


            // Relation
            this.HasRequired(t => t.Topic).WithMany(d => d.TopicDetail).HasForeignKey(f => f.TopicId).WillCascadeOnDelete(true);

        }
    }
}
