
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.IServices
{
    public interface IFileServices
    {

        public Task<Result<string>> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);
        public Task<Result<bool>> Delete(string imageurl);
        


    }
}
