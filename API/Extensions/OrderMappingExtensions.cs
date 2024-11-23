using API.DTOs;
using Core.Entities.OrderAggregate;
using System.Linq;

namespace API.Extensions
{
    public static class OrderMappingExtensions
    {
        public static OrderDto? ToDto(this Order? order)
        {
            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                BuyerEmail = order.BuyerEmail,
                ShippingAddress = order.ShippingAddress,
                DeliveryMethod = order.DeliveryMethod.Description,
                PaymentSummary = order.PaymentSummary,
                ShippingPrice = order.DeliveryMethod.Price,
                //mapping a list of items 
                OrderItems = order.OrderItems.Select(x => x.ToDto()).ToList(),
                Subtotal = order.Subtotal,
                Total = order.GetTotalOrder(),
                Status = order.Status.ToString(),
                PaymentIntentId = order.PaymentIntentId
            };
        }


        public static OrderItemDto? ToDto(this OrderItem? orderItem)
        {
            if (orderItem == null) return null;

            return new OrderItemDto
            {
                ProductId = orderItem.ItemOrdered.ProductId,
                ProductName = orderItem.ItemOrdered.ProductName,
                PictureUrl = orderItem.ItemOrdered.PictureUrl,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity

            };
        }

     
        
    }
}
