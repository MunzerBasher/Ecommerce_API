using EcommerceDataLayer.Entities.Products;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Abstractions.Consts;
using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;


namespace EcommerceApiLayer.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController(IProductServices product, IProductImagesServices productImagesServices) : ControllerBase
    {
        private readonly IProductServices _product = product;
        private readonly IProductImagesServices _productImagesServices = productImagesServices;

        [HasPermission(Permissions.GetProducts)]
        [HttpGet("")]
        [EnableRateLimiting(RateLimiters.Concurrency)]
        public async Task<ActionResult<List<ProductResponse>>> GetAllProducts()
        {
            var result = await _product.GetAll();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HasPermission(Permissions.GetProducts)]
        [HttpGet("Search/{value}")]
        public async Task<ActionResult<List<ProductResponse>>> GetProductsByChar([FromRoute] string value)
        {
            var result = await _product.GetProductsByFirstChar(value);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HasPermission(Permissions.GetProducts)]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductResponse>>> GetProductsById([FromRoute] int id)
        {
            var result = await _product.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value): result.ToProblem();
        }
        [HasPermission(Permissions.GetProducts)]
        [HttpGet("Avalibale-Quantity/{id}")]
        public async Task<ActionResult<List<ProductResponse>>> AvalibaleQuantityById([FromRoute] int id)
        {
            var result = await _product.ProductvalibaleQuantity(id);
            return result.IsSuccess? Ok(result.Value): result.ToProblem();
        }

        [HasPermission(Permissions.AddProducts)]
        [HttpPost("")]
        public async Task<IActionResult> AddProduct([FromBody] ProductRequest product)
        {
            var result = await _product.Add(product);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HasPermission(Permissions.UpdateProducts)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest product, int id)
        {
            var result = await _product.Update(product, id);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HasPermission(Permissions.DeleteProducts)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _product.Delete(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }

        [HasPermission(Permissions.AddProductsImage)]
        [HttpPost("Image/{id}")]
        public async Task<IActionResult> AddImage([FromBody] ProductImageRequest ImageRequest)
        {

            var result = await _productImagesServices.Add(ImageRequest);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HasPermission(Permissions.GetProductsImage)]
        [HttpGet("Images/{id}")]
        public async Task<ActionResult<List<ProductImageResponse>>> GetAllImages([FromRoute] int id)
        {
            var result = await _productImagesServices.GetAll(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HasPermission(Permissions.DeleteProductsImage)]
        [HttpDelete("Image/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var result = await _productImagesServices.Delete(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
    }
}