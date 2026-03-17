using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noon.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Repository.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(Ostatus => Ostatus.Staus).HasConversion(

                            Ostatus => Ostatus.ToString(),
                            Ostatus => (OrderStaus)Enum.Parse(typeof(OrderStaus), Ostatus)
            );

            builder.OwnsOne(O => O.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(O => O.SubTotal)
               .HasColumnType("decimal(18,2)");
        }
    }
}
