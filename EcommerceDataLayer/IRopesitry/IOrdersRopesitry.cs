using EcommerceDataLayer.Entities.Orders;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IOrdersRepositry
    {
        Task<int> AddAsync(OrderDTO order);

        Task<bool> UpdateAsync(OrderDTO order);

        Task<bool> DeleteAsync(int orderId);

        Task<List<OrderResponse>> GetAllAsync();

        Task<List<OrderResponse>> RecentlyOrdersAsync();

        Task<int> GetOrderTotalPriceAsync(int OrderID);

        Task<int> CountOrdersByStatusAsync(int ordersStatus);

        // public Task<bool> IsExistAsync(int ProductID);

        Task<List<OrderResponse>> GetUserOrdersAsync(string UserId);

        Task<int> CountOrdersAsync();
    }


}