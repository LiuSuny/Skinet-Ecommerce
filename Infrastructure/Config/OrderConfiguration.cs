using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner()); //this state that order owned the shipping address
            builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner()); 
            builder.Property(x => x.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );

            builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
          
          //next we configure the relationship
          builder.HasMany(x => x.OrderItems) //many items
              .WithOne() //one order 
              .OnDelete(DeleteBehavior.Cascade); //one to many --one order can have many items

              //config the datetime to give us utc and avoid wrong date coming from our db
              builder.Property(x => x.OrderDate).HasConversion(
                d => d.ToUniversalTime(), 
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
              );
        }
    }
}