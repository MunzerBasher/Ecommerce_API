namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IPaymentsServices paymentsServices) : ControllerBase
    {
        private readonly IPaymentsServices _paymentsServices = paymentsServices;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPost("Create-Checkout-Session{OrderId}")]
        public async Task<ActionResult<string>> CreateCheckoutSession([FromRoute] int OrderId) => Ok(await _paymentsServices.CreateCheckoutSession(OrderId));


        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPost("")]
        public async Task<IActionResult> StripeWebhook()
        {
            var result = await _paymentsServices.StripeWebhook();
            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }
    }




}
