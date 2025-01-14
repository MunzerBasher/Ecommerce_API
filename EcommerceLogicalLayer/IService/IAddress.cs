namespace EcommerceLogicalLayer.IService
{


   public interface IAddress
   {

        public List<AddressDTO> GetAllByUserID(int userID);

        public void Delete(int addressID);

        public void Update(AddressDTO address);

        public AddressDTO? GetByID(int addressID);

        public bool Add(AddressDTO address);

    
   }








}
