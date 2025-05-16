using EcommerceDataLayer.Entities.Address;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;


namespace EcommerceLogicalLayer.Services
{
    public class AddressLogic(IAddressRopesitry addressRopesitry) : IAddressServices
    {
        private readonly IAddressRopesitry _addressRopesitry = addressRopesitry;

       
        public async Task<Result<bool>> AddAsync(AddressRequest address)
        {
            if (address == null)
            {
                return Result<bool>.Fialer<bool>(new Erorr("Bad Request", StatusCodes.Status400BadRequest));

            }
            var result = await _addressRopesitry.AddAsync(address);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));
        
        }

        public async Task<Result<AddressDTO>> GetByIdAsync(int addressID)
        {
            if (addressID < 1)
            {
                throw new ArgumentNullException("addressID  most be grater than 0");
            }
           var Address = await _addressRopesitry.GetByIdAsync(addressID);
            return Address is null ? Result<AddressDTO>.Fialer<AddressDTO>(new Erorr("addressID Not Found", StatusCodes.Status404NotFound)) :
                Result<AddressDTO>.Seccuss(Address);

        }

        public async Task<Result<bool>> UpdateAsync(AddressDTO address)
        {
            if (address == null)
            {
                return Result<bool>.Fialer<bool>(new Erorr("Bad Request", StatusCodes.Status400BadRequest));
            }
            var result = await _addressRopesitry.UpdateAsync(address);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<bool>> DeleteAsync(int addressID)
        {

            if (addressID < 0)
            {
                return Result<bool>.Fialer<bool>(new Erorr("Bad Request", StatusCodes.Status400BadRequest));
            }
            var Address = await _addressRopesitry.GetByIdAsync(addressID);
            if (Address is null)
                return Result<bool>.Fialer<bool>(new Erorr("addressID Not Found", StatusCodes.Status404NotFound));
            var result = await _addressRopesitry.DeleteAsync(addressID);
            return result ? Result<bool>.Seccuss(true) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<List<AddressDTO>>> GetAllUserAddressesAsync(int userID)
        {

            if (userID < 0)
            {
                return Result<List<AddressDTO>>.Fialer<List<AddressDTO>>(new Erorr("Bad Request", StatusCodes.Status400BadRequest));
            }

            var address = await _addressRopesitry.GetAllAddressesByUserIDAsync(userID);
            return Result < List < AddressDTO >>.Seccuss(address);
        }
    }
}