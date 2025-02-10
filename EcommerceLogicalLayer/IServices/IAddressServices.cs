using EcommerceDataLayer.Entities.Address;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IAddressServices
    {
        public Task<Result<bool>> AddAsync(AddressRequest address);

        public Task<Result<AddressResponse>> GetByIdAsync(int addressId);

        public Task<Result<bool>> UpdateAsync(AddressRequest address, int addressId);

        public Task<Result<bool>> DeleteAsync(int addressId);

        public Task<Result<List<AddressResponse>>> GetAllUserAddressesAsync(string UserId);

        public Task<Result<List<AddressResponse>>> GetAllAsync();

    }
}
