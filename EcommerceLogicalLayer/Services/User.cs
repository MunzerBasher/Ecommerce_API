using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.Ropesitry;

namespace EcommerceLogicalLayer.Services
{
    public class UserLogic
    {

        static int GenerateVerifyCode()
        {
            Random random = new Random();
            int verifyCode = random.Next(10000, 100000); // Generates a number between 10000 and 99999
            return verifyCode;
        }
        static public int CreateUser(RegisterDTO user)
        {
            UserDTO use = new UserDTO
            {
                UserName = user.UserName,
                UserDate = DateTime.Now.ToString(),
                UserEmail = user.UserEmail,
                UserPassword = user.UserPassword,
                UserVerflyCode = GenerateVerifyCode(),
                UserPhone = user.UserPhone
            };
            return UserDataAccess.CreateUserAndReturnId(use);
        }
        static public bool ValidateVerificationCode(string email, int verifyCode)
        {
            // Call the UserServer to check if the verification code is valid
            return UserDataAccess.CheckVerifyCodeByEmail(email, verifyCode);
        }

        static public bool DeleteUser(string email)
        {
            // Call the UserServer to delete the user
            return UserDataAccess.DeleteUserByEmail(email);
        }

        public static async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
            return await UserDataAccess.GetAllUsersAsync();
        }

        public static async Task<bool> UpdateUserNameAsync(string userEmail, string userName)
        {
            return await UserDataAccess.UpdateUserNameAsync(userEmail, userName);
        }

        public static async Task<bool> UpdateUserPasswordAsync(string userEmail, string userPassword)
        {
            return await UserDataAccess.UpdateUserPasswordAsync(userEmail, userPassword);
        }

    }

}
