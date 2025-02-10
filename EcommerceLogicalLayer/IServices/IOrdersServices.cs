using EcommerceDataLayer.Entities.Orders;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IOrdersServices
    {

        Task<Result<AddOrdersResponse>> Add(OrderRequest orderRequest);

        Task<Result<List<OrderResponse>>> GetUserOrdersAsync(string UserId);

        Task<Result<bool>> Delete(int orderId);

        Task<bool> IsExistAsync(int OrderID);

        Task<Result<List<OrderResponse>>> GetAll();

        Task<Result<List<OrderResponse>>> RecentlyOrders();

        Task<Result<int>> GetOrderTotalPrice(int OrderID);

        Task<Result<int>> CountOrdersByStatus(int ordersStatus);

        Task<Result<int>> CountOrders();

    }

}