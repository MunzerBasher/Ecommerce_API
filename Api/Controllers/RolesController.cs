using Api.Auth;
using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace SurveyManagementSystemApi.Controllers
{
 

    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpGet("")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllAsync(includeDisabled, cancellationToken);

            return Ok(roles);
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpGet("{id}")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await _roleService.GetAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPost("")]
       [HasPermission(Permissions.AddRoles)]
        public async Task<IActionResult> Add([FromBody] RoleRequest request)
        {
            var result = await _roleService.AddAsync(request);

            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value!.Id }, result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateRoles)]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleRequest request)
        {
            var result = await _roleService.UpdateAsync(id, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.Concurrency)]
        [HttpPut("{id}/toggle-status")]
        [HasPermission(Permissions.UpdateRoles)]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id)
        {
            var result = await _roleService.ToggleStatusAsync(id);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
