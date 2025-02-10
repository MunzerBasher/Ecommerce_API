namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController(IFileServices fileServices) : ControllerBase
    {
        private readonly IFileServices _fileServices = fileServices;

       /// <summary>
       /// [HasPermission(Permissions.UploadImage)]
       /// </summary>
       /// <param name="request"></param>
       /// <param name="cancellationToken"></param>
       /// <returns></returns>
        [HttpPost("Upload-Image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _fileServices.UploadImageAsync(request.Image, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        //wwwroot/images/0821a4ef-c35a-42ca-8747-fa2368d5da74.png

        [HttpDelete("Delete-image/{ImageUrl}")]
        public async Task<IActionResult> DeleteImage(string ImageUrl, CancellationToken cancellationToken)
        {
            var result = await _fileServices.Delete(ImageUrl);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
            
        }



    }


}