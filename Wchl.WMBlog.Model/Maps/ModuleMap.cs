using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class ModuleMap: EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.LinkUrl)
                .HasMaxLength(100);

            this.Property(t => t.Icon)
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .HasMaxLength(10);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            // Properties
            this.Property(t => t.CreateBy)
                .HasMaxLength(50);

            this.Property(t => t.ModifyBy)
                .HasMaxLength(50);

            // Relation
            this.HasOptional(t => t.ParentModule).WithMany(t => t.ChildModule).HasForeignKey(d => d.ParentId);
        }
    }
}
