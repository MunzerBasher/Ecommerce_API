

namespace EcommerceDataLayer.IRopesitry
{
    public interface IOrderItemsRopesitry
    {
        Task<int> AddAsync(OrderItemDTO orderItem);

        Task<bool> UpdateAsync(OrderItemDTO orderItem);

        Task<bool> DeleteAsync(int orderItemID);

        Task<List<OrderItemDTO>> GetAllAsync(int orderID);

        Task DeleteAllAsync(int orderID);

        Task<int> UpdateOrderItemQuantityAsync(int orderItemID, int quantity);

        public Task<bool> IsExistAsync(int ProductID);
    }
}
