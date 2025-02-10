using EcommerceDataLayer.Entities.Users;
using EcommerceLogicalLayer.Helpers;

namespace Api.Auth
{
    public interface IAuthService
    {


        Task<Result<string>> Register(RegisterRequest registerRequest, CancellationToken cancellationToken = default);

        Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);


        Task<Result<AuthResponse>> GetReFreshTokenAsnyc(ReFreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);


        public Task<Result<bool>> ReVokeReFreshTokenAsnyc(ReFreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);


        Task<Result> ConformEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);


        Task<Result> ResendConfirmationEmailAsync(string Email, CancellationToken cancellationToken = default);



        Task<Result> SendResetPasswordCodeAsync(string email);



        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
