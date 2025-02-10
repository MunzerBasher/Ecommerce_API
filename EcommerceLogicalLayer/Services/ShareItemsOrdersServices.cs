using EcommerceDataLayer.Entities.Items;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.IServices;

namespace EcommerceLogicalLayer.Services
{
    public class ShareItemsOrdersServices(IShareItemsOrdersRopesitry shareItemsOrdersRopesitry) : IShareItemsOrdersServices
    {
        private readonly IShareItemsOrdersRopesitry _shareItemsOrdersRopesitry = shareItemsOrdersRopesitry;

        public Task<int> AddItemAsync(ItemRequest orderItem, int orderId)
        =>
         _shareItemsOrdersRopesitry.AddItemAsync(orderItem,orderId);
        

        public Task<bool> IsOrderExistAsync(int orderId)
       => _shareItemsOrdersRopesitry.IsOrderExistAsync(orderId);

    }
}
