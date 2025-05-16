
using EcommerceDataLayer.Entities.Items;

namespace EcommerceDataLayer.Entities.Orders
{

    public class OrderRequest
    {
        public required int UserID { get; set; }

        public ICollection<ItemRequest> Items { get; set; } = new List<ItemRequest>();
    }


}
