using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;


namespace EcommerceLogicalLayer.Services
{
    public class ShippingServices(IShippingRopesitry shippingRopesitry) : IShippingServices
    {
        private readonly IShippingRopesitry _shippingRopesitry = shippingRopesitry;

        public async Task<Result<int>> Add(ShippingRequest Request)
        {
            var result = await _shippingRopesitry.AddAsync(Request);
            if(result > 0)
                return Result<int>.Seccuss(result);
            return Result<int>.Fialer<int>(new Erorr(ShippingsErrors.ServerError, StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<bool>> Delete(int shippingID)
        {
            var result = await _shippingRopesitry.IsExistAsync(shippingID);
            if (!result)
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.NotFound, StatusCodes.Status404NotFound));
            
            var deleted = await _shippingRopesitry.DeleteAsync(shippingID);
            if(deleted < 0)
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.ServerError, StatusCodes.Status500InternalServerError));
            return Result<bool>.Seccuss(result);
        }

        public async Task<Result<List<ShippingResponse>>> GetAll()
        {
            var result = await _shippingRopesitry.GetAllAsync();
            return Result<List<ShippingResponse>>.Seccuss(result);
        }

        public async Task<Result<ShippingResponse>> GetById(int shippingID)
        {
            var result = await _shippingRopesitry.IsExistAsync(shippingID);
            if (!result)
                return Result<ShippingResponse>.Fialer<ShippingResponse>(new Erorr(ShippingsErrors.NotFound, StatusCodes.Status404NotFound));
            var shipping = await _shippingRopesitry.GetByIdAsync(shippingID);
            if(shipping == null)
                return Result<ShippingResponse>.Fialer<ShippingResponse>(new Erorr(ShippingsErrors.ServerError, StatusCodes.Status500InternalServerError));
            return Result<ShippingResponse>.Seccuss(shipping);
        }

        public async Task<Result<ShippingOwnerResponse?>> GetShippingOwnerDetails(int ownerId)
        {
            //need to use identity to chech UserId
           var shipping = await _shippingRopesitry.GetShippingOwnerDetailsAsync(ownerId);
            return Result<ShippingResponse>.Seccuss(shipping);
        }

        public async Task<bool> IsExsist(int shippingID)
        {
            return (await _shippingRopesitry.IsExistAsync(shippingID));
        }

        public async Task<Result<bool>> Update(int ShippingID, ShippingRequest Request)
        {
           if (!await IsExsist(ShippingID))
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.NotFound, StatusCodes.Status404NotFound));
            var shipping = await _shippingRopesitry.UpdateAsync(ShippingID, Request);
            if (shipping < 0)
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.ServerError, StatusCodes.Status500InternalServerError));
            return Result<bool>.Seccuss(true);

        }

      

        public async Task<Result<bool>> UpdateActualDeliveryDate(DateTime ActualDeliveryDate, int ShippingID)
        {
            if (!await IsExsist(ShippingID))
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _shippingRopesitry.UpdateActualDeliveryDate(ActualDeliveryDate,ShippingID);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.ServerError,StatusCodes.Status500InternalServerError));
       
        }

      
        public async Task<Result<bool>> UpdateStatus(int Status, int ShippingID)
        {
            if (!await IsExsist(ShippingID))
                return Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.NotFound, StatusCodes.Status404NotFound));
            var result = await _shippingRopesitry.UpdateStatus(Status, ShippingID); 
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Fialer<bool>(new Erorr(ShippingsErrors.ServerError, StatusCodes.Status500InternalServerError));

        }
    }


}