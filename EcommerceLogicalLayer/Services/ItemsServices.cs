using EcommerceDataLayer.Entities.Items;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;


namespace EcommerceLogicalLayer.Services
{
    public class ItemsServices(IOrderItemsRopesitry orderItemscsRopesitry, 
        IShareItemsOrdersServices shareItemsOrdersServices) : IItemsServices
    {
        private readonly IOrderItemsRopesitry _orderItemscsRopesitry = orderItemscsRopesitry;
        private readonly IShareItemsOrdersServices _shareItemsOrdersServices = shareItemsOrdersServices;
       
        public async Task<Result<int>> Add(ItemRequest item, int orderId)
        {
            if (!await _shareItemsOrdersServices.IsOrderExistAsync(orderId))
                return Result<int>.Failure<int>(new Error(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _shareItemsOrdersServices.AddItemAsync(item, orderId);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Failure<int>(new Error(ItemsError.ServerError, StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<bool>> Delete(int orderItemID)
        {
            if (!await _orderItemscsRopesitry.IsExistAsync(orderItemID))
                return Result<bool>.Failure<bool>(new Error(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.DeleteAsync(orderItemID);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Failure<bool>(new Error(ItemsError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<bool>> DeleteAll(int orderId)
        {
            if (!await _shareItemsOrdersServices.IsOrderExistAsync(orderId))
                return Result<bool>.Failure<bool>(new Error(ItemsError.NotFound, StatusCodes.Status404NotFound));
            await _orderItemscsRopesitry.DeleteAllAsync(orderId);
            return Result<bool>.Seccuss(true);
        }

        public async Task<Result<List<ItemResponse>>> GetAll(int orderId)
        {
            if (!await _shareItemsOrdersServices.IsOrderExistAsync(orderId))
                return Result<List<ItemResponse>>.Failure<List<ItemResponse>>(new Error(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.GetAllAsync(orderId);
            List<ItemResponse > items = new List< ItemResponse >();
            foreach ( var item in result )
            {
                var itemResponse = new ItemResponse
                {
                    ProductId = item.ProductID,
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
                return Result<int>.Failure<int>(new Error(ItemsError.NotFound, StatusCodes.Status404NotFound));
            var result = await _orderItemscsRopesitry.UpdateOrderItemQuantityAsync(orderItemID, quantity);  
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Failure<int>(new Error(ItemsError.ServerError, StatusCodes.Status500InternalServerError));
        }


    }
}