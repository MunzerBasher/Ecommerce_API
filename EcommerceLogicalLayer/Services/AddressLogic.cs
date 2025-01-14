using EcommerceLogicalLayer.IService;

namespace EcommerceLogicalLayer.Services
{
    public class AddressLogic : IAddress
    {
        private readonly AddressDataAccess _addressDataAccess;
        public AddressLogic(AddressDataAccess addressDataAccess)
        {
            _addressDataAccess = addressDataAccess;
        }

        public bool Add(AddressDTO address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");

            }
            return _addressDataAccess.AddAddress(address);
        }


        public AddressDTO? GetByID(int addressID)
        {
            if (addressID < 0)
            {
                throw new ArgumentNullException("addressID  most be grater than 0");
            }
            return _addressDataAccess.GetAddressByID(addressID);
        }

        public void Update(AddressDTO address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address is invalid");
            }
            _addressDataAccess.UpdateAddress(address);
        }


        public void Delete(int addressID)
        {

            if (addressID < 0)
            {
                throw new ArgumentNullException("addressID  most be grater than 0");
            }
            _addressDataAccess.DeleteAddress(addressID);
        }


        public List<AddressDTO> GetAllByUserID(int userID)
        {

            if (userID < 0)
            {
                throw new ArgumentNullException("userID most be grater than 0");
            }

            return _addressDataAccess.GetAllAddressesByUserID(userID);
        }


    }
}