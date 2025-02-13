namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController(IFileServices fileServices) : ControllerBase
    {
        private readonly IFileServices _fileServices = fileServices;

       
        
        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPost("Upload-Image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _fileServices.UploadImageAsync(request.Image, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        
        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpDelete("Delete-image/{ImageUrl}")]
        public async Task<IActionResult> DeleteImage(string ImageUrl, CancellationToken cancellationToken)
        {
            var result = await _fileServices.Delete(ImageUrl);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
            
        }



    }


}