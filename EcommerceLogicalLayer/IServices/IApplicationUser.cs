
using EcommerceDataLayer.DTOS;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IApplicationUser
    {

        public int Add(UserDTO user);


        public bool CheckVerifyCodeByEmail(string userEmail, int verifyCode);



        public bool Delete(string userEmail);


        public Task<List<GetUserDTO>> GetAll();

        public Task<bool> Update(string userEmail, string userName);


        public Task<bool> UpdateUserPasswordAsync(string userEmail, string userPassword);

    }
}
