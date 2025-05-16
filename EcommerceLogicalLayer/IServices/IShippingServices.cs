using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
   
    public interface IShippingServices
    {

        public Task<Result<int>> Add(ShippingRequest Request);


        public Task<Result<ShippingResponse>>  GetById(int shippingID);


        public Task<Result<bool>> Update(int ShippingID, ShippingRequest Request);


        public Task<Result<bool>> UpdateStatus(int Status, int ShippingID);

        public Task<Result<bool>> UpdateActualDeliveryDate(DateTime ActualDeliveryDate, int ShippingID);

       
        public Task<Result<bool>> Delete(int shippingID);


        public Task<Result<List<ShippingResponse>>> GetAll();


        public Task<bool> IsExsist(int shippingID);


        public Task<Result<ShippingOwnerResponse?>> GetShippingOwnerDetails(int ownerId);



    }
}