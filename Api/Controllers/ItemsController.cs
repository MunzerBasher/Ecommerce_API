using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace EcommerceAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(IItemsServices orderItemsServices) : ControllerBase
    {
        private readonly IItemsServices _orderItemsServices = orderItemsServices;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.AddItems)]
        [HttpPost("{orderId}")]
        public async Task<ActionResult<int>> AddItem([FromBody] ItemRequest orderItem, int orderId)
        {
            var result = await _orderItemsServices.Add(orderItem, orderId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.DeleteItems)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _orderItemsServices.Delete(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.UpdateItems)]
        [HttpPut("QuantityinOrder")]
        public async Task<ActionResult<int>> UpdateItemQuantity([FromBody] UpdateOrderItemQuantityDTO request)
        {
            var result = await _orderItemsServices.UpdateOrderItemQuantity(request.OrderItemID, request.Quantity);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetItems)]
        [HttpGet("{OrderId}")]
        public async Task<ActionResult<List<ItemResponse>>> GetAll(int OrderId)
        {
            var result = await _orderItemsServices.GetAll(OrderId);
            return Ok(result.Value);
        }


    }

}