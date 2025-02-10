using EcommerceLogicalLayer.Helpers;

namespace EcommerceLogicalLayer.IServices
{
    public interface IPaymentsServices
    {

        Task<string> CreateCheckoutSession(int OrderId);

        public Task<Result> StripeWebhook();

    }
}
