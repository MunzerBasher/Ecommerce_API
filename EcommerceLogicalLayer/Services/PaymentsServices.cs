using EcommerceDataLayer.Entities.NewFolder;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;


using System.Security.Claims;
using System.Text;


namespace EcommerceLogicalLayer.Services
{
    public class PaymentsServices(IOrdersServices ordersServices, IConfiguration configuration,
         IHttpContextAccessor httpContextAccessor) : IPaymentsServices
    {

        private readonly IOrdersServices _ordersServices = ordersServices;
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string> CreateCheckoutSession(int OrderId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = 200000 , // Amount in cents ($50.00)
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Product",

                        }


                    },
                    Quantity = 1,

                }
            },
                Mode = "payment",
                SuccessUrl = "https://yourdomain.com/success",
                CancelUrl = "https://yourdomain.com/cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.Url;
        }

        public async Task<Result> StripeWebhook()
        {
            var request = _httpContextAccessor.HttpContext!.Request;      
            using var reader = new StreamReader(request.Body, Encoding.UTF8);
            var json = await reader.ReadToEndAsync();

            var secret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                     _httpContextAccessor.HttpContext.Response.Headers["Stripe-Signature"],
                    secret
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session != null)
                    {
                        string customerEmail = session.CustomerEmail;
                        string paymentId = session.PaymentIntentId;

                        var Payment = new PaymentRequest
                        {
                            Amount = session.AmountTotal,
                            Date = DateTime.UtcNow,
                            UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
                        }
                        ;
                       

                    }
                }

                return Result.Seccuss();
            }
            catch (StripeException e)
            {
                return Result.Failure(new Error ($"Webhook error: {e.Message}",StatusCodes.Status500InternalServerError));
            }
        }


    }
}