using EcommerceDataLayer.Entities.Categories;
using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace EcommerceApi.Controllers
{

    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController(ICategoriesServices categoriesServices, IFileServices fileServices) : ControllerBase
    {
        private readonly ICategoriesServices _categoriesServices = categoriesServices;
        private readonly IFileServices _fileServices = fileServices;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.AddCategories)]
        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] CategoryRequest CategoryRequest, CancellationToken cancellationToken)
        {

            var result = await _categoriesServices.Add(CategoryRequest);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetCategories)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _categoriesServices.GetById(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.UpdateCategories)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequest category)
        {
            var result = await _categoriesServices.Update(id, category);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.DeleteCategories)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _categoriesServices.ToggleStatus(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetCategories)]
        [HttpGet("")]
        public async Task<ActionResult<List<CategoryResponse>>> GetAll()
        {
            var result = await _categoriesServices.GetAll();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetCategories)]
        [HttpGet("search/{firstChar}")]

        public async Task<ActionResult<List<CategoryResponse>>> SearchProductCategoriesByFirstChar(string firstChar)
        {
            var result = await _categoriesServices.Search(firstChar);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }




    }
}