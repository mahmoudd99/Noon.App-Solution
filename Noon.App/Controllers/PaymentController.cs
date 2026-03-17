using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noon.App.Dtos;
using Noon.App.Errors;
using Noon.Core.Entities.OrderAggregate;
using Noon.Core.Services;
using StackExchange.Redis;
using Stripe;

namespace Noon.App.Controllers
{

    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private string endpointSecret ="jshagdAJDFHAD";
        public PaymentController(IPaymentService paymentService ,ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]   // POST : /api/Payment/Id
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketid)
        {
            var basket = await _paymentService.CreateeOrUpdatePaymentIntent(basketid);
            if (basket == null) return BadRequest(new ApiResponse(400, "A Problem With Your Basket"));
            return Ok(basket);


        }


        [HttpPost("webhook")] // POST : /api/Payment/webhook
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;


                Core.Entities.OrderAggregate.Order order;
                switch (stripeEvent.Type)
                {

                    case "PaymentIntentSucceeded":
                       order=  await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                        _logger.LogInformation("Payment is succeeded", paymentIntent.Id);
                        break;
                    case "PaymentIntentPaymentFailed":
                       order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                        _logger.LogInformation("Payment is Failed", paymentIntent.Id);
                        break;
                }


                return Ok();
          
        } 
            
    }
}
