

namespace EcommerceDataLayer.IRopesitry
{
    public interface IOrderItemsRopesitry
    {      

        Task<bool> DeleteAsync(int orderItemID);

        Task<List<OrderItemDTO>> GetAllAsync(int orderID);

        Task DeleteAllAsync(int orderID);

        Task<int> UpdateOrderItemQuantityAsync(int orderItemID, int quantity);

        public Task<bool> IsExistAsync(int ProductID);
    }
}
