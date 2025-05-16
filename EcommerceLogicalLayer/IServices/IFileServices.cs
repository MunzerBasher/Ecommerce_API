
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.IServices
{
    public interface IFileServices
    {

        public Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);

    }
}
