using EcommerceDataLayer.DTOS;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.Services
{

    public class ProductImagesServices(IProductImagesRopesitry productImagesRopesitry,IProductServices productServices) : IProductImagesServices
    {
        private readonly IProductImagesRopesitry _productImagesRopesitry = productImagesRopesitry;
        private readonly IProductServices _productServices = productServices;

        public async Task<Result<int>> Add(ProductImageRequest productImage)
        {
            var IsExist = await _productServices.IsExistAsync(productImage.ProductID);
            if (!IsExist)
                return Result<int>.Fialer<int>(new Erorr("product Not Found ", StatusCodes.Status404NotFound));

            var result = await _productImagesRopesitry.AddAsync(productImage);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Fialer<int>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));
        
        }

        public async Task<Result<int>> Delete(int imageId)
        {
            var result = await _productImagesRopesitry.DeleteAsync(imageId);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Fialer<int>(new Erorr("Internal Server Error", StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<List<ProductImageResponse>>> GetAll(int productId)
        {
            var IsExist = await _productServices.IsExistAsync(productId);
            if (!IsExist)
                return Result<List<ProductImageResponse>>.Fialer<List<ProductImageResponse>>(new Erorr("product Not Found ",StatusCodes.Status404NotFound));
           
            var result  = (await _productImagesRopesitry.GetAllAsync(productId)); 
            return Result<List<ProductImageResponse>>.Seccuss(result);
        }
    }


}