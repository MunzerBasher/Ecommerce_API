using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Users;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
    


namespace Api.Auth
{
    public class AuthService(ApplicationDbContext context, UserManager<UserIdentity> userManager,SignInManager<UserIdentity> signInManager,
            IJwtToken jwtToken,ILogger<AuthService> logger, IHttpContextAccessor httpContextAccessor,
            IEmailService emailService) : IAuthService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<UserIdentity> _userManager = userManager;
        private readonly SignInManager<UserIdentity> _signInManager = signInManager;
        private readonly IJwtToken _jwtToken = jwtToken;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailService _emailService = emailService;

        public async Task<Result<string>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.AnyAsync(x => x.Email == registerRequest.Email);
            if(user)
                return Result<string>.Failure<string>(new Error("Duplicated Email", StatusCodes.Status409Conflict));


            var User = new UserIdentity
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
            }
           ;
            User.UserName = registerRequest.Email;
            var result = await _userManager.CreateAsync(User, registerRequest.Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                _logger.LogInformation($"before encoding  {code}");
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation($"after encoding  {code}");
                await SendConfirmationEmail(User, code);
                return Result<string>.Seccuss("");
            }
            var error = result.Errors.First();
            return Result<string>.Failure<string>(new Error(error.Description, StatusCodes.Status400BadRequest));


        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == loginRequest.Email);
            if (user is null)
            {
                return Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.Duplicated, StatusCodes.Status400BadRequest));
            }
            if (!await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.Invalid, StatusCodes.Status400BadRequest));

            if (!await _signInManager.UserManager.IsEmailConfirmedAsync(user))
                Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.Confirmed, StatusCodes.Status409Conflict));
            //await _signInManager.PasswordSignInAsync(user,loginRequest.Password,false,false); 
            var (roles, permissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            string token = _jwtToken.GenerateToken(user, roles, permissions);

            var refreshToken = GeneratorRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(10);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpires,
            });
            await _userManager.UpdateAsync(user);
            return Result<AuthResponse>.Seccuss(new AuthResponse
            { Token = token, ExpiresIn = 10, FirstName = user.FirstName, LastName = user.LastName, RefreshToken = refreshToken, RefreshTokenExpires = refreshTokenExpires });

        }

        public async Task<Result<AuthResponse>> GetReFreshTokenAsnyc(ReFreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)       
        {
            var userId = _jwtToken.ValidateToken(refreshTokenRequest.token);
            if(userId is  null)
                return Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
           var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
                return Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenRequest.rFreshToken && x.IsActive);
            if(refreshToken is null)
                return Result<AuthResponse>.Failure<AuthResponse>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
            refreshToken!.ReVokedOn = DateTime.UtcNow;
            var NewrefreshToken = GeneratorRefreshToken();
            var refreshTokenExpires = DateTime.UtcNow.AddDays(10);
            var (roles, permissions) = await GetUserRolesAndPermissions(user, cancellationToken);
            string token = _jwtToken.GenerateToken(user, roles, permissions);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewrefreshToken,
                ExpiresOn = refreshTokenExpires,
            });
            await _userManager.UpdateAsync(user);
            return Result<AuthResponse>.Seccuss(new AuthResponse
            { Token = token, ExpiresIn = 10, FirstName = user.FirstName, LastName = user.LastName, RefreshToken = NewrefreshToken, RefreshTokenExpires = refreshTokenExpires });
        }

        public async Task<Result<bool>> ReVokeReFreshTokenAsnyc(ReFreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)     
        {
            var userId = _jwtToken.ValidateToken(refreshTokenRequest.token);
            if (userId is null)
                return Result<bool>.Failure<bool>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
                return Result<bool>.Failure<bool>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenRequest.rFreshToken && x.IsActive);
            if (refreshToken is null)
                return Result<bool>.Failure<bool>(new Error(AuthErrors.InvalidRefreshToken, StatusCodes.Status400BadRequest));
            refreshToken!.ReVokedOn = DateTime.UtcNow;
            return Result<bool>.Seccuss(true);
        }

        public async Task<Result> ConformEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
                return Result.Failure(new Error("Invalid UserName Or Password ", StatusCodes.Status400BadRequest));

            if (user.EmailConfirmed)
                return Result.Failure(new Error("Email is Confirmed", StatusCodes.Status400BadRequest));


            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            }
            catch (FormatException e)
            {
                return Result.Failure(new Error(e.Message, StatusCodes.Status400BadRequest));
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                BackgroundJob.Enqueue(() => SendConfirmationEmail(user, code));
                return Result.Seccuss();
            }
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendConfirmationEmailAsync(string Email, CancellationToken cancellationToken = default)

        {
            if (await _userManager.FindByEmailAsync(Email) is not { } user)
                return Result.Seccuss();

            if (user.EmailConfirmed)
                return Result.Failure(new Error("Email is Confirmed", StatusCodes.Status400BadRequest));

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);

            await SendConfirmationEmail(user, code);

            return Result.Seccuss();
        }

        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if (await _userManager.FindByEmailAsync(email) is not { } user)
                return Result.Seccuss();

            if (!user.EmailConfirmed)
                return Result.Failure(new Error("Invalid Code", StatusCodes.Status409Conflict));

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Reset code: {code}", code);

            await SendResetPasswordEmail(user, code);

            return Result.Seccuss();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(new Error("Invalid Code", StatusCodes.Status409Conflict));

            IdentityResult result;

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if (result.Succeeded)
                return Result.Seccuss();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, StatusCodes.Status401Unauthorized));


        }


        private static string GeneratorRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(UserIdentity user, CancellationToken cancellationToken)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var userPermissions = await (from r in _context.Roles
                                         join p in _context.RoleClaims
                                         on r.Id equals p.RoleId
                                         where userRoles.Contains(r.Name!)
                                         select p.ClaimValue!)
                                         .Distinct()
                                         .ToListAsync(cancellationToken);

            return (userRoles, userPermissions);
        }


        private async Task SendConfirmationEmail(UserIdentity user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
                }
            );
            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "✅ Ecommerce Api : Email Confirmation", emailBody));
            await Task.CompletedTask;
        }

        private async Task SendResetPasswordEmail(UserIdentity user, string code)
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
                }
            );

            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "✅ Ecommerce: Change Password", emailBody));

            await Task.CompletedTask;
        }


    }

}