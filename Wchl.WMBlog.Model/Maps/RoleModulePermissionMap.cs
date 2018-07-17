using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
   public  class RoleModulePermissionMap: EntityTypeConfiguration<RoleModulePermission>
    {
        public RoleModulePermissionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CreateBy)
                .HasMaxLength(50);

            this.Property(t => t.ModifyBy)
                .HasMaxLength(50);

            // Relation
            this.HasRequired(t => t.Role).WithMany(d => d.RoleModulePermission).HasForeignKey(f => f.RoleId).WillCascadeOnDelete(true);
            this.HasRequired(t => t.Module).WithMany(d => d.RoleModulePermission).HasForeignKey(f => f.ModuleId).WillCascadeOnDelete(true);
            this.HasOptional(t => t.Permission).WithMany(d => d.RoleModulePermission).HasForeignKey(f => f.PermissionId).WillCascadeOnDelete(true);
        }
    }
}
