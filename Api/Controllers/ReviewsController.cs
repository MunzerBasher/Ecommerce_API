using EcommerceDataLayer.Entities.Reviews;


namespace EcommerceApi.Controllers
{
    [Route("api/Reviews")]
    [ApiController]
    public class ReviewsController(IReviewsServices reviewsServices) : ControllerBase
    {
        private readonly IReviewsServices _reviewsServices = reviewsServices;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpGet("User/{UserId}")]
        [ProducesResponseType(typeof(List<ReviewResponse>), 200)]
        public async Task<ActionResult<List<ReviewResponse>>> GetAllUserReviews(string UserId)
        {
            var result = await _reviewsServices.GetAllUserReviewsAsync(UserId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpGet("Product/{ProductId}")]
        [ProducesResponseType(typeof(List<ReviewResponse>), 200)]
        public async Task<ActionResult<List<ReviewResponse>>> GetAllProductReviews(int ProductId)
        {
            var result = await _reviewsServices.GetAllProductReviewsAsync(ProductId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpGet("{UserId}Product/{ProductId}")]
        [ProducesResponseType(typeof(List<ReviewResponse>), 200)]
        public async Task<ActionResult<List<ReviewResponse>>> GetAllProductReviews(int ProductId, string UserId)
        {
            var result = await _reviewsServices.GetAllUserReviewsInProductAsync(UserId,ProductId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPost("")]

        public async Task<IActionResult> Add([FromBody] ReviewRequest review)
        {
            var result = await _reviewsServices.AddAsync(review);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpDelete("{ReviewId}")]
       
        public async Task<IActionResult>  Delete(int ReviewId)
        {
            var result = await _reviewsServices.DeleteAsync(ReviewId);  
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPut("{ReviewId}")]
       
        public async Task<IActionResult> Update([FromBody] UpdateReviewRequest updateReviewRequest, int ReviewId)
        {
            var result = await _reviewsServices.UpdateAsync(updateReviewRequest, ReviewId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

    }
}