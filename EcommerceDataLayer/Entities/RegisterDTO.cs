

using EcommerceDataLayer.DataValidation;

namespace EcommerceDataLayer.DTOS
{
    public class RegisterDTO
    {
        [UserName]
        public  required string UserName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhone { get; set; }
        public required string UserPassword { get; set; }
    }
}
