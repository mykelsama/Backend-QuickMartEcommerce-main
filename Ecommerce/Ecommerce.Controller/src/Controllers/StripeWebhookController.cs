using Ecommerce.Service.src.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace Ecommerce.Controller.src.Controllers
{
    [ApiController]
    [Route("api/v1/webhooks")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;

        public StripeWebhookController(IOrderService orderService, IConfiguration config)
        {
            _orderService = orderService;
            _config = config;
        }

        [HttpPost]
        [HttpPost]
public async Task<IActionResult> HandleStripeWebhook([FromHeader(Name = "Stripe-Signature")] string stripeSignature)
{
    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
    var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _config["Stripe:WhSecret"]);

    if (stripeEvent.Type == Events.CheckoutSessionCompleted)
    {
        var session = stripeEvent.Data.Object as Session;
        if (session != null)
        {
            await _orderService.MarkOrderAsPaid(session.Id);
        }
    }

    return Ok();
}

    }
}