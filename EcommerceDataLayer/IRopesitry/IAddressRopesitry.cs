

using EcommerceDataLayer.Entities.Address;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IAddressRopesitry
    {
        Task<bool> AddAsync(AddressRequest address);

        Task<AddressResponse?> GetByIdAsync(int addressId);

        Task<bool> UpdateAsync(AddressRequest address, int addressId);

        Task<bool> DeleteAsync(int addressID);

        Task<List<AddressResponse>> GetAllAddressesByUserIDAsync(string UserId);

        Task<List<AddressResponse>> GetAllAsync();

        Task<bool> IsExistAsync(int AddressId);

    }
}
