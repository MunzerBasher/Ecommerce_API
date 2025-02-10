using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.Services
{
    public class FileServices(IWebHostEnvironment webHostEnvironment) : IFileServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";





        public async Task<Result<string>> UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
        {     
            var Extension = Path.GetExtension(image.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{Extension}";
            var path = Path.Combine(_imagesPath, fileName);
            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);
            return Result<string>.Seccuss($"images/{fileName}");
        }



        public async Task<Result<bool>> Delete(string imageurl)
        {
            var value = imageurl.Replace("images/", "");
            var path = Path.Combine(_imagesPath, value);
            if (!System.IO.File.Exists(path))
            {
                return Result<bool>.Failure<bool>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            }
            try
            {
                System.IO.File.Delete(imageurl);
            } 
            catch (Exception ex)
            {
                return Result<bool>.Failure<bool>(new Error(ex.Message, StatusCodes.Status500InternalServerError));
            }
            return Result<bool>.Seccuss(true);
        }




    }
}
