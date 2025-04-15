using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.PictureUrl).IsRequired().HasMaxLength(1000);
            builder.Property(p => p.Price).HasPrecision(18, 2);

            builder.HasOne(p => p.Brand).WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category).WithMany()
                    .OnDelete(DeleteBehavior.Restrict); ;
        }
    }
}
