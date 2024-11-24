using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Core.Specification
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.OrderDate);

        }

        public OrderSpecification(string email, int id) : base(x => x.BuyerEmail == email && x.Id ==id)
        {
            AddThenIncludeString("OrderItems"); //for theninclude
            AddThenIncludeString("DeliveryMethod");
        }

         public OrderSpecification(string paymentIntentId, bool isPaymentIntent) 
         : base(x => x.PaymentIntentId == paymentIntentId)
        {
            AddThenIncludeString("OrderItems"); //for theninclude
            AddThenIncludeString("DeliveryMethod");

        }
    }

}