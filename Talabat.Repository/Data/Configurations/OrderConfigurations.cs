﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(o => o.ShippingAddress , ShippingAddress => ShippingAddress.WithOwner() );

			builder.Property(o => o.Status).HasConversion
				(
				o => o.ToString(),
				o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
				);

			builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");

			builder.HasOne(o => o.DeliveryMethod)
				   .WithMany()
				   .HasForeignKey(o => o.DeliveryMethodId)
				   .OnDelete(DeleteBehavior.SetNull);
		}
	}
}
