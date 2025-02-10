using EcommerceDataLayer.AppDbContex;
using EcommerceDataLayer.Entities.Address;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;



namespace EcommerceLogicalLayer.Services
{
    public class AddressServices(IAddressRopesitry addressRopesitry,ApplicationDbContext applicationDbContext) : IAddressServices
    {
        private readonly IAddressRopesitry _addressRopesitry = addressRopesitry;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<Result<bool>> AddAsync(AddressRequest address)
        {
            if(!await _applicationDbContext.Users.AnyAsync(x => x.Id == address.UserId))
                return Result<bool>.Failure<bool>(new Error(AddressesError.UserNotFound, StatusCodes.Status400BadRequest));
            var result = await _addressRopesitry.AddAsync(address);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Failure<bool>(new Error(AddressesError.ServerError, StatusCodes.Status500InternalServerError));
        
        }

        public async Task<Result<AddressResponse>> GetByIdAsync(int addressId)
        {
            if (addressId < 1)
            {
                return Result<AddressResponse>.Failure<AddressResponse>(new Error(AddressesError.NotFound, StatusCodes.Status400BadRequest));
            }
           var Address = await _addressRopesitry.GetByIdAsync(addressId);
            return Address is null ? Result<AddressResponse>.Failure<AddressResponse>(new Error(AddressesError.NotFound, StatusCodes.Status404NotFound)) :
                Result<AddressResponse>.Seccuss(Address);
        }

        public async Task<Result<bool>> UpdateAsync(AddressRequest address, int AdressId)
        {
            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == address.UserId))
                return Result<bool>.Failure<bool>(new Error(AddressesError.UserNotFound, StatusCodes.Status400BadRequest));
            var result = await _addressRopesitry.UpdateAsync(address, AdressId);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Failure<bool>(new Error(AddressesError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<bool>> DeleteAsync(int addressId)
        {
            var Address = await _addressRopesitry.IsExistAsync(addressId);
            if (!Address)
                return Result<bool>.Failure<bool>(new Error(AddressesError.NotFound, StatusCodes.Status404NotFound));
            var result = await _addressRopesitry.DeleteAsync(addressId);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Failure<bool>(new Error(AddressesError.ServerError, StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<List<AddressResponse>>> GetAllUserAddressesAsync(string UserId)
        {
            if (!await _applicationDbContext.Users.AnyAsync(x => x.Id == UserId))
                return Result<List<AddressResponse>>.Failure<List<AddressResponse>>(new Error(AddressesError.UserNotFound, StatusCodes.Status400BadRequest));
            var address = await _addressRopesitry.GetAllAddressesByUserIDAsync(UserId);
            return Result<List<AddressResponse>>.Seccuss(address);
        }


        public async Task<Result<List<AddressResponse>>> GetAllAsync() => Result<List<AddressResponse>>.Seccuss(await _addressRopesitry.GetAllAsync());
        
    }
}