using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController(IFavorites Favorites) : ControllerBase
    {
        private readonly IFavorites _favorites = Favorites;

        [HttpPost("")]
        [HasPermission(Permissions.GetFavorites)]
        public async Task<IActionResult> AddToFavorite([FromBody] FavoriteDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _favorites.Add(request.ProductID, User.GetUserId()!, true,cancellationToken);
            return result.IsSuccess ? Ok(result) : result.ToProblem();

        }

        [HttpDelete("")]
        [HasPermission(Permissions.DeleteFavorites)]
        public async Task<ActionResult<Result<bool>>> Delete([FromBody] FavoriteDTO request, CancellationToken cancellationToken = default)
        {
            var result = await _favorites.Delete(request.ProductID, User.GetUserId()!, cancellationToken);
            return result.IsSuccess ? Ok(result) : result.ToProblem();

        }
        [HasPermission(Permissions.GetFavorites)]
        [HttpGet("MyFavorites")]
        public async Task<ActionResult<Result<List<int>>>> GetAll( CancellationToken cancellationToken = default)
        {
            var result = await _favorites.GetByUserId(User.GetUserId()!);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}