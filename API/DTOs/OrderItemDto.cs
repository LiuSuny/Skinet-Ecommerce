using Core.Entities.OrderAggregate;

namespace API.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; } 
        public required string ProductName { get; set; }
        public required string  PictureUrl { get; set; }
        public Decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
