
namespace EcommerceDataLayer.Entities.Orders
{
    public class AddOrdersResponse
    {

        public int OrderID { get; set; }

        public List<ItemsResponse> ItemResponses { get; set; } = new List<ItemsResponse>();
    }
}
