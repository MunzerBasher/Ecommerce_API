using EcommerceDataLayer.Entities.Account;
using EcommerceDataLayer.Entities.Users;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Auth
{
    public class AccountService(UserManager<UserIdentity> userManager) : IAccountService
    {
        private readonly UserManager<UserIdentity> _userManager = userManager;

        public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<ProfileResponse>.Failure<ProfileResponse>(new Error("Try Login  ", StatusCodes.Status203NonAuthoritative));
            var result = await _userManager.ChangePasswordAsync(user!, request.OldPasswor, request.NewPasswor);

            if (result.Succeeded)
                return Result.Seccuss();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, StatusCodes.Status400BadRequest));

        }

        public async Task<Result<ProfileResponse>> GetProfileAsync(string UserId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user is null)
                return Result<ProfileResponse>.Failure<ProfileResponse>(new Error("Try Login  ", StatusCodes.Status203NonAuthoritative));
            return Result<ProfileResponse>.Seccuss(new ProfileResponse
            {
                Email = user!.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!

            });


        }

        public async Task<Result> UpdateProfileAsync(UpdateProfileRequest updateProfileRequest, string userId)
        {
            if (!await _userManager.Users.AllAsync(x => x.Id == userId))
                return Result<ProfileResponse>.Failure<ProfileResponse>(new Error("Try Login  ", StatusCodes.Status203NonAuthoritative));
            await _userManager.Users
              .Where(x => x.Id == userId)
              .ExecuteUpdateAsync(setters =>
                  setters
                      .SetProperty(x => x.FirstName, updateProfileRequest.FirstName)
                      .SetProperty(x => x.LastName, updateProfileRequest.LastName)
                      
          );
            return Result.Seccuss();
        }
    }
}
