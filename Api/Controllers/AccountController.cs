using EcommerceDataLayer.Entities.Account;
using SurveyManagementSystemApi.Abstractions.Consts;
using SurveyManagementSystemApi.Securty.Filters;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HasPermission(Permissions.GetProfile)]
        [HttpGet("Profile")]
        public async Task<ActionResult<ProfileResponse>> GetProfile()
        {
            var profile = await _accountService.GetProfileAsync(User.GetUserId()!);

            return profile.IsSuccess ? Ok(profile.Value) : profile.ToProblem();
        }

        [HasPermission(Permissions.UpdateProfile)]
        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest updateProfileRequest)
        {
            var profile = await _accountService.UpdateProfileAsync(updateProfileRequest, User.GetUserId()!);
            return profile.IsSuccess ? Ok(profile) : profile.ToProblem();
        }

        [HasPermission(Permissions.ChangePassword)]
        [HttpPut("Change-Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var profile = await _accountService.ChangePasswordAsync(changePasswordRequest, User.GetUserId()!);
            return profile.IsSuccess ? Ok(profile) : profile.ToProblem();
        }

    }
}