using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController(IPaymentService paymentService,
        IUnitOfWork unitOfWork
        ) : BaseApiController
    {
        //private readonly string _webHookSecret = config["StripeSettings:WebHookSecret"]!;
       
        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePayementIntent( string cartId)
        {
            var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart");

            return Ok(cart);

        }

     
        [HttpGet("delivery-method")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>>
        GetDeliveryMethod()
        {

            return Ok(await unitOfWork.Repository<DeliveryMethod>().ListAllAsync());

        }


       
        ////trying to get notice fron stripe
        //[HttpPost("webhook")]
        //public async Task<IActionResult> StripeWebHook()
        //{
        //      var json = await new StreamReader(Request.Body).ReadToEndAsync();
        //        try
        //        {
        //            var stripeEvent = ConstructStripeEvent(json);
        //            if (stripeEvent.Data.Object is not PaymentIntent intent)
        //            {
        //                return BadRequest("Invalid event data");
        //            }
                    
        //            var pay = HandlePaymentIntentSucceeded(intent);

        //            return Ok(pay);                 
                   
        //        }
        //         catch (StripeException ex)
        //        {
        //        logger.LogError(ex, "Stripe webhook error");
        //       return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
                 
        //         }
        //        catch (Exception ex)
        //         {
        //        logger.LogError(ex, "An unexpected error occurred");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        //         }
                
        //}

        //private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        //{

        //  if (intent.Status == "succeeded") 
        //    {
        //    var spec = new OrderSpecification(intent.Id, true);
        //    var order = await unitOfWork.Repository<Order>().GetEntityWithSpecificationPattern(spec)
        //        ?? throw new Exception("Order not found");
        //    if ((long)order.GetTotalOrder() * 100 != intent.Amount)
        //    {
        //        order.Status = OrderStatus.PaymentMismatch;
        //    } 
        //    else
        //    {
        //        order.Status = OrderStatus.PaymentReceived;
        //    }

        //    await unitOfWork.Complete();

        //      //TODO: SignalR

        //      var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
        //      if(!string.IsNullOrEmpty(connectionId)){

        //        await hubContext.Clients.Client(connectionId)
        //        .SendAsync("OrderCompleteNotification", order.ToDto());
        //      }

        //    }

        //}

        //private Event ConstructStripeEvent(string json)
        //{
        //   try
        //   {
        //      return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webHookSecret);
        //   }
        //   catch (Exception ex)
        //   {
        //     logger.LogError(ex, "Failed to construct an event");
        //    throw new StripeException("Invalid signature");
        //   }
        //}
    }
}
