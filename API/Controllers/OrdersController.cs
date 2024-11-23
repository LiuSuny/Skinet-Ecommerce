using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController(IUnitOfWork unitOfWork, ICartService cartService): BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto createOrderDto)
        {
              //first thing to get hold of is user email
              var email = User.GetUserEmail();
             

               //get hold our cart
               var cart = await cartService.GetCartAsync(createOrderDto.CartId);
                if(cart == null) return BadRequest("cart is empty");

                //check if the cart has payament id
                if(cart.PaymentIntentId == null) return BadRequest("No payment intenet for this order");

                var items = new List<OrderItem>();
                foreach (var item in cart.Items)
                {
                    var productItem = await unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if(productItem == null) return BadRequest("Problem with the order");
                    
                     var itemOrdered = new ProductItemOrdered {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        PictureUrl = item.PictureUrl
                     };

                     //creating new orderitem
                     var orderItem = new OrderItem{
                        ItemOrdered = itemOrdered,
                        Price = productItem.Price,
                        Quantity = item.Quantity
                     };

                     items.Add(orderItem);
                }

                var deliveryMethod  = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(createOrderDto.DeliveryMethodId);
                    if(deliveryMethod == null) return BadRequest("Delivery method not selected");
                
                    //MAPPING Order to dto
                    var order = new Order
                   {
                    OrderItems = items,
                    DeliveryMethod = deliveryMethod,
                    ShippingAddress = createOrderDto.ShippingAddress,
                    Subtotal = items.Sum(x => x.Price * x.Quantity),
                    PaymentSummary = createOrderDto.PaymentSummary,
                    PaymentIntentId = cart.PaymentIntentId,
                    BuyerEmail = email
                 };
                
                //adding it to db
                unitOfWork.Repository<Order>().Add(order);
                //save changes
                if (await unitOfWork.Complete())
                {
                    return order;
                }
                return BadRequest("Problem creating order");

        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrderForUser()
        {
            var spec = new  OrderSpecification(User.GetUserEmail());


            var orders = await unitOfWork.Repository<Order>().ListSpecAsync(spec);
            //returning a our toDto
              var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

            return Ok(ordersToReturn);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetUserEmail(), id);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpecificationPattern(spec);
            if (order == null) return NotFound();
            return order.ToDto();
        }

    }
}