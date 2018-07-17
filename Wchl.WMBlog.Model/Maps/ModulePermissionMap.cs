using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class ModulePermissionMap:EntityTypeConfiguration<ModulePermission>
    {
        public ModulePermissionMap()
        {
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CreateBy)
                .HasMaxLength(50);

            this.Property(t => t.ModifyBy)
                .HasMaxLength(50);

            // Relation
            this.HasRequired(t => t.Module).WithMany(d => d.ModulePermission).HasForeignKey(f => f.ModuleId).WillCascadeOnDelete(true);
            this.HasRequired(t => t.Permission).WithMany(d => d.ModulePermission).HasForeignKey(f => f.PermissionId).WillCascadeOnDelete(true);
        }
    }
}
