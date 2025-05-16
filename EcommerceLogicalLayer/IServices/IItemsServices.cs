using EcommerceDataLayer.Entities.Items;
using EcommerceLogicalLayer.Helpers;



namespace EcommerceDataLayer.IRopesitry
{
    public interface IItemsServices
    {

        public Task<Result<int>> Add(ItemRequest orderItem, int orderId);


        public Task<Result<bool>> Delete(int orderItemID);


        public Task<Result<List<ItemResponse>>> GetAll(int orderID);



        public Task<Result<bool>> DeleteAll(int orderID);


        public Task<Result<int>> UpdateOrderItemQuantity(int orderItemID, int quantity);


    }
}