

using EcommerceDataLayer.Entities.Orders;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IOrdersServices
    {

        public Task<Result<AddOrdersResponse>> Add(OrderRequest orderRequest);


        public Task<Result<bool>> Delete(int orderId);

        public  Task<bool> IsExistAsync(int OrderID);

        public Task<Result<List<OrderDTOWithUserName>>> GetAll();

        public Task<Result<List<OrderDTOWithUserName>>> RecentlyOrders();

        public Task<Result<int>> GetOrderTotalPrice(int OrderID);

        public Task<Result<int>> CountOrdersByStatus(int ordersStatus);

        public Task<Result<int>> CountOrders();

    }
}
