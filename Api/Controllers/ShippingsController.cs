using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace ShippingApi.Controllers
{

    [Route("api/Shippings")]
    [ApiController]
    public class ShippingsController(IShippingServices shipping) : ControllerBase
    {
        private readonly IShippingServices _shipping = shipping;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetShippings)]
        [HttpGet("")]
        public async Task<ActionResult<List<ShippingResponse>>> GetAllShippings()
        {
            var result = await _shipping.GetAll();
            return Ok(result.Value);
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.AddShippings)]
        [HttpPost("")]
        public async Task<ActionResult<int>> Add([FromBody] ShippingRequest shippingDTO)
        {
            var result = await _shipping.Add(shippingDTO);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.UpdateShippings)]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update([FromBody] ShippingRequest shippingDTO, int id)
        {
            var result = await _shipping.Update(id, shippingDTO);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.DeleteShippings)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _shipping.Delete(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.GetShippings)]
        [HttpGet("Shipping-Owner-Details/{ownerId}")]
        public async Task<ActionResult<ShippingOwnerResponse>> GetShippingOwnerDetails(int ownerId)
        {
            var result = await _shipping.GetShippingOwnerDetails(ownerId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.UpdateShippings)]
        [HttpPut("Status")]
        public async Task<ActionResult<int>> UpdateStatus([FromBody] UpdateStatusRequest Request)
        {
            var result = await _shipping.UpdateStatus(Request.Status, Request.ShippingId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HasPermission(Permissions.UpdateShippings)]
        [HttpPut("Actual-Delivery-Date")]
        public async Task<ActionResult<int>> UpdateActualDeliveryDate([FromBody] UpdateActualDateRequest Request)
        {
            var result = await _shipping.UpdateActualDeliveryDate(Request.ActualDate, Request.ShippingId);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


    }
}