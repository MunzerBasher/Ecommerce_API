using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;


namespace MyControllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController(IOrdersServices orders) : ControllerBase
    {
        private readonly IOrdersServices _orders = orders;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.AddOrders)]
        [HttpPost("")]
       
        public async Task<IActionResult> Add([FromBody] OrderRequest order)
        {
            order.UserId = User.GetUserId()!;
            var result = await _orders.Add(order);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.DeleteOrders)]
        [HttpDelete("{orderId}")]
        
        public async Task<IActionResult> Deleter(int orderId)
        {
            var result = await _orders.Delete(orderId);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("")]
        
        public async Task<ActionResult<OrderResponse>> GetAllOrders()
        {
            var result = await _orders.GetAll();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("Recently")]
       
        public async Task<ActionResult<OrderResponse>> GetRecentlyOrders()
        {
            var result = await _orders.RecentlyOrders();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("Count")]
       
        public async Task<ActionResult<int>> GetOrderCount()
        {
            var result = await _orders.CountOrders();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("CountByStatus/{Status}")]
       
        public async Task<ActionResult<int>> GetOrderCountByStatus(int Status)
        {
            var result = await _orders.CountOrdersByStatus(Status);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("TotalPrice/{OrderId}")]
        
        public async Task<ActionResult<int>> GetOrderTotalPrices(int OrderId)
        {
            var result = await _orders.GetOrderTotalPrice(OrderId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetOrders)]
        [HttpGet("User/{UserId}")]

        public async Task<ActionResult<int>> GetUserOrders(int UserId)
        {
            var result = await _orders.GetOrderTotalPrice(UserId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }

}