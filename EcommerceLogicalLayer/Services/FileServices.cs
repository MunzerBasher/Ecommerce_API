using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.Services
{
    public class FileServices(IWebHostEnvironment webHostEnvironment) : IFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";





        public async Task<string> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
        {     
            var Extension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{Extension}";
            var path = Path.Combine(_imagesPath, fileName);
            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);
            return $"images/{fileName}";
        }



    }
}
