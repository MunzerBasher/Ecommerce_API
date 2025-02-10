
using EcommerceDataLayer.Entities.Items;

namespace EcommerceDataLayer.Entities.Orders
{

    public class OrderRequest
    {
        public required string UserId { get; set; }

        public ICollection<ItemRequest> Items { get; set; } = new List<ItemRequest>();
    }


}
