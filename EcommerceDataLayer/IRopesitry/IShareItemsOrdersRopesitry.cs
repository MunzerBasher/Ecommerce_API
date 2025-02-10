

using EcommerceDataLayer.Entities.Items;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IShareItemsOrdersRopesitry
    {

        public Task<bool> IsOrderExistAsync(int orderId);


        public Task<int> AddItemAsync(ItemRequest orderItem, int orderId);

    }
}
