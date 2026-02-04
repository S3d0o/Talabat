using Shared.Dtos.BasketModule;

namespace Presentation.Controllers
{
    public class PaymentsController(IServiceManager _serviceManager) : ApiController
    {
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
            if (basket == null) return BadRequest("Problem with your basket");
            return Ok(basket);
        }
        [HttpPost("Webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signatureHeader = Request.Headers["Stripe-Signature"];
            await _serviceManager.PaymentService.UpdatePaymentStatusAsync(json, signatureHeader);
            return Ok();
        }
    }
}
