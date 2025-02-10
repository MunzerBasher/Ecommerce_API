using EcommerceDataLayer.Entities.Orders;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.Services
{
    public class OrderServices(IOrdersRepositry orders, EcommerceDataLayer.IRopesitry.IItemsServices orderItemsServices) : IOrdersServices
    {
        private readonly IOrdersRepositry _orders = orders;
        private readonly EcommerceDataLayer.IRopesitry.IItemsServices _orderItemsServices = orderItemsServices;

        public async Task<Result<AddOrdersResponse>> Add(OrderRequest orderRequest)
        {
            var order = new OrderDTO
            {
                OrderDate = DateTime.Now,
                Status = 1,
                UserID = orderRequest.UserID,
                TotalAmount = 1
            };
            AddOrdersResponse addOrdersResponse = new();
            var result = await _orders.AddAsync(order);
            if(result > 0)
            {
                addOrdersResponse.OrderID = result;
                foreach(var item in orderRequest.Items)
                {
                    ItemsResponse itemsResponse = new();
                    var ItemId = await _orderItemsServices.Add(item,result);
                    itemsResponse.ProductId = item.ProductId;
                    itemsResponse.ItemId = ItemId.Value;
                    addOrdersResponse.ItemResponses.Add(itemsResponse);
                }

            }
            return result > 0 ? Result<AddOrdersResponse>.Seccuss(addOrdersResponse) : Result<AddOrdersResponse>.Fialer<AddOrdersResponse>(new Erorr(OrdersError.ServerError, StatusCodes.Status500InternalServerError));
        }
        public async Task<bool> IsExistAsync(int OrderID)
        {
            var result = await _orders.IsExistAsync(OrderID);
            return result;
        }

        public async Task<Result<int>> CountOrders()
        {
            var result = await _orders.CountOrdersAsync();
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<int>> CountOrdersByStatus(int ordersStatus)
        {
            var result = await _orders.CountOrdersByStatusAsync(ordersStatus);                  
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<bool>> Delete(int orderId)
        {
            if( !await  IsExistAsync(orderId))
                return Result<bool>.Fialer<bool>(new Erorr(OrdersError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orders.DeleteAsync(orderId);        
            return result ? Result<bool>.Seccuss(true) : Result<int>.Fialer<bool>(new Erorr(OrdersError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<List<OrderDTOWithUserName>>> GetAll()
        {
            var result = await _orders.GetAllAsync();
            return Result<List<OrderDTOWithUserName>>.Seccuss(result);
        }

        public async Task<Result<int>> GetOrderTotalPrice(int OrderID)
        {
            if (!await IsExistAsync(OrderID))
                return Result<int>.Fialer<int>(new Erorr(OrdersError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orders.GetOrderTotalPriceAsync(OrderID);
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<List<OrderDTOWithUserName>>> RecentlyOrders()
        {
           var result = await _orders.RecentlyOrdersAsync();
            return Result<List<OrderDTOWithUserName>>.Seccuss(result);
        }

    }
}
