

using EcommerceDataLayer.Entities.Address;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IAddressRopesitry
    {
        public Task<bool> AddAsync(AddressRequest address);

        public Task<AddressDTO?> GetByIdAsync(int addressID);

        public Task<bool> UpdateAsync(AddressDTO address);

        public Task<bool> DeleteAsync(int addressID);

        public Task<List<AddressDTO>> GetAllAddressesByUserIDAsync(int userID);




    }
}
