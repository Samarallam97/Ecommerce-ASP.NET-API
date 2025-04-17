using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket_Module;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.ServiceInterfaces;

namespace Talabat.APIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPayementService _payementService;
		private readonly ILogger<PaymentController> _logger;

		public PaymentController(IPayementService payementService , ILogger<PaymentController> logger)
        {
			_payementService=payementService;
			_logger=logger;
		}

		[Authorize]

		[HttpPost("{basketId}")]
		[ProducesResponseType(typeof(Basket) , StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _payementService.CreateOrUpdatePayementIntent(basketId);

			if (basket is null) return BadRequest(new BaseErrorResponse(400, "An error has occurred with the basket"));

			return Ok(basket);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> StripeWebHook()
		{

			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ParseEvent(json);

			var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

			Order order;
				
			// Handle the event

			if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
			{
				order = await _payementService.HandlePaymentIntentSucceededOrFailed(paymentIntent.Id , true);
				_logger.LogInformation("Payment Succeeded" , paymentIntent.Id);
			}
			else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
			{
				order = await _payementService.HandlePaymentIntentSucceededOrFailed(paymentIntent.Id, false);
				_logger.LogInformation("Payment Failed", paymentIntent.Id);

			}
			else
			{
				Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
			}

			return new EmptyResult();
		}

    }
}
