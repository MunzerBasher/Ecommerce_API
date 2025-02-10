using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Orders;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace EcommerceLogicalLayer.Services
{
    public class OrderServices(IOrdersRepositry orders,
        IShareItemsOrdersServices shareItemsOrdersServices,
        IProductServices productServices,
        ApplicationDbContext applicationDbContext
        ) : IOrdersServices
    {
        private readonly IOrdersRepositry _orders = orders;
        private readonly IShareItemsOrdersServices _shareItemsOrdersServices = shareItemsOrdersServices;
        private readonly IProductServices _productServices = productServices;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<Result<AddOrdersResponse>> Add(OrderRequest orderRequest)
        {
            if(!await _applicationDbContext.Users.AnyAsync(u => u.Id == orderRequest.UserId))
                return Result<AddOrdersResponse>.Failure<AddOrdersResponse>(new Error(UserErrors.NotFound, StatusCodes.Status404NotFound));
            var order = new OrderDTO
            {
                OrderDate = DateTime.Now,
                Status = 1,
                UserID = orderRequest.UserId,
            };
            AddOrdersResponse addOrdersResponse = new();
            var result = await _orders.AddAsync(order);
            if(result > 0)
            {
                addOrdersResponse.OrderID = result;
                foreach(var item in orderRequest.Items)
                {
                    if ( !await _productServices.IsExistAsync(item.ProductId) )
                        return Result<AddOrdersResponse>.Failure<AddOrdersResponse>(new Error(OrdersError.NotFound, StatusCodes.Status404NotFound));
                    ItemsResponse itemsResponse = new();
                    var ItemId = await _shareItemsOrdersServices.AddItemAsync(item, result);
                    if(ItemId < 0)
                        return Result<AddOrdersResponse>.Failure<AddOrdersResponse>(new Error(OrdersError.ServerError, StatusCodes.Status500InternalServerError));
                    itemsResponse.ProductId = item.ProductId;
                    itemsResponse.ItemId = ItemId;
                    addOrdersResponse.ItemResponses.Add(itemsResponse);
                }
            }
            return result > 0 ? Result<AddOrdersResponse>.Seccuss(addOrdersResponse) : Result<AddOrdersResponse>.Failure<AddOrdersResponse>(new Error(OrdersError.ServerError, StatusCodes.Status500InternalServerError));
        }
        public async Task<bool> IsExistAsync(int OrderID)
        {
            var result = await _shareItemsOrdersServices.IsOrderExistAsync(OrderID);
            return result;
        }

        public async Task<Result<int>> CountOrders()
        {
            var result = await _orders.CountOrdersAsync();
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<int>> CountOrdersByStatus(int ordersStatus)
        {
            if (ordersStatus < 1 || ordersStatus > 3)
                return Result<int>.Failure<int>(new Error(OrdersError.InValidStatus, StatusCodes.Status400BadRequest));
            var result = await _orders.CountOrdersByStatusAsync(ordersStatus);                  
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<bool>> Delete(int orderId)
        {
            if( !await  IsExistAsync(orderId))
                return Result<bool>.Failure<bool>(new Error(OrdersError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orders.DeleteAsync(orderId);        
            return result ? Result<bool>.Seccuss(true) : Result<int>.Failure<bool>(new Error(OrdersError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<List<OrderResponse>>> GetAll()
        {
            var result = await _orders.GetAllAsync();
            return Result<List<OrderResponse>>.Seccuss(result);
        }

        public async Task<Result<int>> GetOrderTotalPrice(int OrderID)
        {
            if (!await IsExistAsync(OrderID))
                return Result<int>.Failure<int>(new Error(OrdersError.NotFound, StatusCodes.Status404NotFound));
           
            var result = await _orders.GetOrderTotalPriceAsync(OrderID);
            return Result<int>.Seccuss(result);
        }

        public async Task<Result<List<OrderResponse>>> RecentlyOrders()
        {
           var result = await _orders.RecentlyOrdersAsync();
            return Result<List<OrderResponse>>.Seccuss(result);
        }

        public async Task<Result<List<OrderResponse>>> GetUserOrdersAsync(string UserId)
        {
            if (!await _applicationDbContext.Users.AnyAsync(u => u.Id == UserId))
                return Result<List<OrderResponse>>.Failure<List<OrderResponse>>(new Error(UserErrors.NotFound, StatusCodes.Status404NotFound));

            var result = await _orders.GetUserOrdersAsync(UserId);
            return result is null ? Result<List<OrderResponse>>.Seccuss(result)! : Result<List<OrderResponse>>.Failure<List<OrderResponse>>(new Error("", StatusCodes.Status404NotFound));
        }

        
    }
}
