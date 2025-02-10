using EcommerceDataLayer.Entities.Account;
using EcommerceLogicalLayer.Helpers;

namespace Api.Auth
{
    public interface IAccountService
    {
        public Task<Result<ProfileResponse>> GetProfileAsync(string UserId);


        public Task<Result> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest, string userId);


        public Task<Result> ChangePasswordAsync(ChangePasswordRequest changePasswordRequest, string userId);



    }

}
