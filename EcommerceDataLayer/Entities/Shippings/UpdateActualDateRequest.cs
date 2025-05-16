
namespace EcommerceDataLayer.Entities.Shippings
{
    public class UpdateActualDateRequest
    {
        public int ShippingId { get; set; }

        public DateTime ActualDate { get; set; }
    }
}
