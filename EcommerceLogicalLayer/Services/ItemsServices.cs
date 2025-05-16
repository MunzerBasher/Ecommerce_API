using EcommerceDataLayer.Entities.Items;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;



namespace EcommerceLogicalLayer.Services
{
    public class ItemsServices(IOrderItemsRopesitry orderItemscsRopesitry) : IItemsServices
    {
        private readonly IOrderItemsRopesitry _orderItemscsRopesitry = orderItemscsRopesitry;
       // private readonly IOrdersServices _ordersServices = ordersServices;

        public async Task<Result<int>> Add(ItemRequest orderItem, int orderId)
        {
            //if(!await _ordersServices.IsExistAsync(orderId))
            //    return Result<int>.Fialer<int>(new Erorr(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var item = new OrderItemDTO
            {
                OrderItemID = 1,
                Price = orderItem.Price,
                ProductID = orderItem.ProductId,
                Quantity = orderItem.Quantity,
                TotalItemsPrice = orderItem.Price * orderItem.Quantity,
                OrderID = orderId
            };
            var result = await _orderItemscsRopesitry.AddAsync(item);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Fialer<int>(new Erorr(ItemsError.ServerError, StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<bool>> Delete(int orderItemID)
        {
            if (!await _orderItemscsRopesitry.IsExistAsync(orderItemID))
                return Result<bool>.Fialer<bool>(new Erorr(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.DeleteAsync(orderItemID);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Fialer<bool>(new Erorr(ItemsError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<bool>> DeleteAll(int orderID)
        {
            //if(! await _ordersServices.IsExistAsync(orderID))
            //    return Result<bool>.Fialer<bool>(new Erorr(ItemsError.NotFound, StatusCodes.Status404NotFound));
            await _orderItemscsRopesitry.DeleteAllAsync(orderID);
            return Result<bool>.Seccuss(true);
        }

        public async Task<Result<List<ItemResponse>>> GetAll(int orderID)
        {
            //if (!await _ordersServices.IsExistAsync(orderID))
            //    return Result<List<ItemResponse>>.Fialer<List<ItemResponse>>(new Erorr(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.GetAllAsync(orderID);
            List<ItemResponse > items = new List< ItemResponse >();
            foreach ( var item in result )
            {
                var itemResponse = new ItemResponse
                {
                    // ProductName = item.ProductID,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalItemsPrice
                };
                items.Add(itemResponse);  
            }
            return Result<List<ItemResponse>>.Seccuss(items);

        }


        
        public async Task<Result<int>> UpdateOrderItemQuantity(int orderItemID, int quantity)
        {
            if (!await _orderItemscsRopesitry.IsExistAsync(orderItemID))
                return Result<int>.Fialer<int>(new Erorr(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.UpdateOrderItemQuantityAsync(orderItemID, quantity);  
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Fialer<int>(new Erorr(ItemsError.ServerError, StatusCodes.Status500InternalServerError));
        }


    }
}