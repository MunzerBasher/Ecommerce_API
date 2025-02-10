

using EcommerceDataLayer.Entities.Items;

namespace EcommerceLogicalLayer.IServices
{
    public interface IShareItemsOrdersServices
    {
        public Task<bool> IsOrderExistAsync(int orderId);


        public Task<int> AddItemAsync(ItemRequest orderItem, int orderId);

    }
}
