

using EcommerceDataLayer.Entities.Address;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IAddressServices
    {
        public Task<Result<bool>> AddAsync(AddressRequest address);

        public Task<Result<AddressDTO>> GetByIdAsync(int addressID);

        public Task<Result<bool>> UpdateAsync(AddressDTO address);

        public Task<Result<bool>> DeleteAsync(int addressID);

        public Task<Result<List<AddressDTO>>> GetAllUserAddressesAsync(int userID);

    }
}
