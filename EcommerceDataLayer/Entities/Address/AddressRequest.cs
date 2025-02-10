

namespace EcommerceDataLayer.Entities.Address
{
    public class AddressRequest
    {

        public int UserID { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required decimal Longitude { get; set; }
        public required decimal Latitude { get; set; }
    }
}
