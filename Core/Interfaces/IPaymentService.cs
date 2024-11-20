using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        //intent to take payment from third party using stripe
        Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
    }
}