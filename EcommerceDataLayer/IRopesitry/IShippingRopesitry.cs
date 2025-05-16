
namespace EcommerceDataLayer.IRopesitry
{
    public interface IShippingRopesitry
    {
        Task<int> AddAsync(ShippingRequest shippingDTO);

        Task<ShippingResponse> GetByIdAsync(int shippingID);

        Task<int> UpdateAsync(int ShippingID, ShippingRequest Request);

        Task<int> DeleteAsync(int shippingID);

        Task<List<ShippingResponse>> GetAllAsync();

        Task<bool> IsExistAsync(int shippingID);

        Task<ShippingOwnerResponse?> GetShippingOwnerDetailsAsync(int ownerId);

        Task<bool> UpdateStatus(int Status, int ShippingId);

        Task<bool> UpdateActualDeliveryDate(DateTime ActualDeliveryDate , int ShippingID);

    }

}
