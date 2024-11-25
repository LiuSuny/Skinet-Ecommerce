using API.Extensions;
using API.SignalR;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace API.Controllers
{
    /// <summary>
    /// this controller is reponsible to get pull request
    ///from stripe to our backend before sending request to the frontend
    ///Note: run this command on cmd stripe listen --forward-to https://localhost:5001/api/payment/webhook -e payment_intent.succeeded
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    /// <param name="hubContext"></param>
    public class PaymentController(
        IUnitOfWork unitOfWork, ILogger<PaymentController> logger,
        IConfiguration config, IHubContext<NotificationHub> hubContext) :BaseApiController
    {
        private readonly string _webHookSecret = config["StripeSettings:WebHookSecret"]!;


        //trying to get notice fron stripe
        [HttpPost("{webhook}")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = ConstructStripeEvent(json);
                if (stripeEvent.Data.Object is not PaymentIntent intent)
                {
                    return BadRequest("Invalid event data");
                }

               await HandlePaymentIntentSucceeded(intent);

                return Ok();

            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }

        }

        private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {

            if (intent.Status == "succeeded")
            {
                var spec = new OrderSpecification(intent.Id, true);
                var order = await unitOfWork.Repository<Order>().GetEntityWithSpecificationPattern(spec)
                    ?? throw new Exception("Order not found");
                if ((long)order.GetTotalOrder() * 100 != intent.Amount)
                {
                    order.Status = OrderStatus.PaymentMismatch;
                }
                else
                {
                    order.Status = OrderStatus.PaymentReceived;
                }

                await unitOfWork.Complete();

                //TODO: SignalR

                var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
                if (!string.IsNullOrEmpty(connectionId))
                {

                    await hubContext.Clients.Client(connectionId)
                    .SendAsync("OrderCompleteNotification", order.ToDto());
                }

            }

        }

        private Event ConstructStripeEvent(string json)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webHookSecret);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to construct an event");
                throw new StripeException("Invalid signature");
            }
        }
    }
}
