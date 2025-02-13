namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService loginService) : ControllerBase
    {
        private readonly IAuthService _loginService = loginService;

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest registerRequest)
        {
            var result = await _loginService.Register(registerRequest);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
        {

            var response = await _loginService.LoginAsync(loginRequest);
            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("ReFresh")]
        public async Task<ActionResult<AuthResponse>> ReFresh([FromBody] ReFreshTokenRequest refreshTokenRequest,CancellationToken cancellationToken)
        {

            var response = await _loginService.GetReFreshTokenAsnyc(refreshTokenRequest, cancellationToken);
            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("ReVoke")]
        public async Task<ActionResult<AuthResponse>> ReVokeReFreshToken([FromBody] ReFreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {

            var response = await _loginService.ReVokeReFreshTokenAsnyc(refreshTokenRequest, cancellationToken);
            return response.IsSuccess ? Ok(response.Value) : response.ToProblem();
        }
        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("Conform-Email")]
        public async Task<IActionResult> ConformEmailAsync(ConfirmEmailRequest request)
        {
            var response = await _loginService.ConformEmailAsync(request);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("ResendConform-Email")]
        public async Task<IActionResult> ResendConfirmationEmailAsync(string Email, CancellationToken cancellationToken = default)
        {
            var response = await _loginService.ResendConfirmationEmailAsync(Email, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPasswordAsync(string emali)
        {
            var response = await _loginService.SendResetPasswordCodeAsync(emali);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [EnableRateLimiting(RateLimiters.UserLimiter)]
        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _loginService.ResetPasswordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }


    }
}