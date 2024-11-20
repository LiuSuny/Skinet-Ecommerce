using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService(IConfiguration config, 
       ICartService cartService, IGenericRepository<Core.Entities.Product> productRepo,
        IGenericRepository<DeliveryMethod> deliveryRepo) : IPaymentService
    {
        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
           StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"]; //config strip payment

           //get hold of the cart
           var cart = await cartService.GetCartAsync(cartId);

           //we check if the cart is empty or not
           if(cart == null) return null;

          //create variable to store shipping price
           var shippingPrice = 0m; //0m this represent a decimal

           //check if the cart has a delivery method id
           if(cart.DeliveryMethodId.HasValue){
               //if we do then
               var deliveryMethod = await deliveryRepo.GetByIdAsync((int)cart.DeliveryMethodId);

               if(deliveryMethod == null) return null; //if it is null we return null

               //if it is not 
               shippingPrice = deliveryMethod.Price;
           }
           
            //next we validate and update the items in the cart
                foreach(var item in cart.Items){
                    var productItem = await productRepo.GetByIdAsync(item.ProductId);

                      if(productItem == null) return null; //if it is null we return null

                       //check if the user is paying for right product
                      if(item.Price != productItem.Price)
                      {
                        item.Price = productItem.Price;
                      }
                }

            // create a new service variable for the PaymentIntentService and PaymentIntent
             var service = new PaymentIntentService(); //this is from stripe  ---PaymentIntentService();--
             PaymentIntent? intent = null;  //PaymentIntent? also from stripe and we set it to null initially

             //next we check if we gotten a payment intent with id in our shopping cart
             if(string.IsNullOrEmpty(cart.PaymentIntentId))
             {
                 //if we did not have payment intent already in our shopping cart we try to create one
                 var options = new PaymentIntentCreateOptions //PaymentIntentCreateOptions also from stripe docs 
                 {
                    Amount=(long)(cart.Items.Sum(x => x.Quantity * (x.Price * 100))) 
                    + (long)shippingPrice * 100, //convert from long to decimal
                    //next we specify the currency
                    Currency = "usd",
                    //next we specify the payment type
                    PaymentMethodTypes = ["card"]

                 };
               
               //then we can create our payment intent
               intent = await service.CreateAsync(options);
             
                //Next we going to update our payment card as we need to store payment intent id 
                 cart.PaymentIntentId = intent.Id;
                 
                // and client secret we get after creating the payment
                  cart.ClientSecret = intent.ClientSecret;
             }
             //and if we already have payment intent id which  (!string.IsNullOrEmpty(cart.PaymentIntentId))
             else{
                  var options = new PaymentIntentUpdateOptions //PaymentIntentUpdateOptions also class from stripe library
                  {
                      Amount=(long)(cart.Items.Sum(x => x.Quantity * (x.Price * 100))) 
                    + (long)shippingPrice * 100, //convert from long to decimal
                  };

                    //then we can update our payment intent
                   intent = await service.UpdateAsync(cart.PaymentIntentId, options);
             }
             
             //we set the cart 
             await cartService.SetCartAsync(cart);

             //and return the cart
             return cart;

        }
    }
}