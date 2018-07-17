using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wchl.WMBlog.Model.Models;

namespace Wchl.WMBlog.Model.Maps
{
    public class AdvertisementMap: EntityTypeConfiguration<Advertisement>
    {
        public AdvertisementMap()
        {
            this.HasKey(p => p.Id);
            this.Property(p => p.ImgUrl).HasMaxLength(512);
            this.Property(p => p.Title).HasMaxLength(64);
            this.Property(p => p.Url).HasMaxLength(256);
        }
    }
}
