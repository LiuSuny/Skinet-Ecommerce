namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public ProductItemOrdered ItemOrdered {get; set;} = null!;
         public Decimal Price {get; set;} 
        public int Quantity {get; set;}

    }
}