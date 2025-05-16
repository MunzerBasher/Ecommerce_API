using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;


namespace EcommerceLogicalLayer.Services
{
    public class ProductLogic(IProductRopesitry productRopesitry) : IProductServices
    {
        private readonly IProductRopesitry _productRopesitry = productRopesitry;


        public async Task<Result<List<ProductResponse>>> GetAll()
        {
            var result = await _productRopesitry.GetAllAsync();
            return Result<List<ProductResponse>>.Seccuss(result);
        }

        public async Task<Result<List<ProductResponse>>> GetProductsByFirstChar(string firstChar)
        {
            var result = await _productRopesitry.GetProductsByFirstCharAsync(firstChar);
            return Result<List<ProductResponse>>.Seccuss(result);
        }

        public async Task<Result<bool>> Add(ProductResponse product)
        {
            var result = await _productRopesitry.AddAsync(product);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));
        }

        public async Task<Result> Update(ProductResponse product)
        {
            var IsExist = await _productRopesitry.IsExistAsync(product.ProductID);
            if(!IsExist)
                return Result<bool>.Fialer<bool>(new Erorr("Product Is Not Found", StatusCodes.Status404NotFound));
            var result = await _productRopesitry.UpdateAsync(product);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));



        }

        public async Task<Result> Delete(int productId)
        {
            var IsExist = await _productRopesitry.IsExistAsync(productId);
            if (!IsExist)
                return Result<bool>.Fialer<bool>(new Erorr("Product Is Not Found", StatusCodes.Status404NotFound));
            var result = await _productRopesitry.DeleteAsync(productId);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Fialer<bool>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));

        }

        public async Task<bool> IsExistAsync(int ProductID)
        {
            var result = await _productRopesitry.IsExistAsync(ProductID);
            return result;
        }
    }

}
