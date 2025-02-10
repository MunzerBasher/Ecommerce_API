using Microsoft.AspNetCore.Http;

namespace EcommerceDataLayer.Entities.Images
{
    public class UploadImageRequest
    {
        public IFormFile Image { get; set; }
    }
}
