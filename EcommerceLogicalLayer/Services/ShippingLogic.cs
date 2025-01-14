namespace EcommerceLogicalLayer.Services
{
    public class ShippingLogic
    {


        public static int CreateShipping(ShippingDTO shippingDTO)
        {
            return ShippingDataAccess.CreateShipping(shippingDTO);
        }


        public static int UpdateShipping(ShippingDTO shippingDTO)
        {
            return ShippingDataAccess.UpdateShipping(shippingDTO);
        }


        public static int DeleteShipping(int shippingID)
        {
            return ShippingDataAccess.DeleteShipping(shippingID);
        }



        public static List<GetShippingDTO> GetAllShippings()
        {
            return ShippingDataAccess.GetAllShippings();
        }

        public static bool FindShipping(int shippingID)
        {
            return ShippingDataAccess.FindShipping(shippingID);
        }

        public static ShippingOwnerDTO? GetShippingOwnerDetails(int ownerId)
        {
            return ShippingDataAccess.GetShippingOwnerDetails(ownerId);
        }


    }
}
