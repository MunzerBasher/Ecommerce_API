using EcommerceDataLayer.Entities.Products;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;

namespace EcommerceLogicalLayer.Services
{

    public class ProductImagesServices(IProductImagesRopesitry productImagesRopesitry,
        IProductServices productServices, IFileServices fileServices) : IProductImagesServices
    {

        private readonly IProductImagesRopesitry _productImagesRopesitry = productImagesRopesitry;
       
        private readonly IProductServices _productServices = productServices;
        private readonly IFileServices _fileServices = fileServices;

        public async Task<Result<int>> Add(ProductImageRequest productImage)
        {
            var IsExist = await _productServices.IsExistAsync(productImage.ProductID);
            if (!IsExist)
                return Result<int>.Failure<int>(new Error(ProductsError.ProductNotFound, StatusCodes.Status404NotFound));

            var result = await _productImagesRopesitry.AddAsync(productImage);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Failure<int>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));
        
        }

        public async Task<Result<int>> Delete(int imageId)
        {
            if(await _productImagesRopesitry.IsExistAsync(imageId))
                return Result<int>.Failure<int>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            var ImageUrl = await _productImagesRopesitry.GetImageURL(imageId);
            var value = await _fileServices.Delete(ImageUrl);
            var result = await _productImagesRopesitry.DeleteAsync(imageId);
            return result > 0 ? Result<int>.Seccuss(result) : Result<int>.Failure<int>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));

        }

       
        public async Task<Result<List<ProductImageResponse>>> GetAll(int productId)
        {
            var IsExist = await _productServices.IsExistAsync(productId);
            if (!IsExist)
                return Result<List<ProductImageResponse>>.Failure<List<ProductImageResponse>>(new Error(ProductsError.ProductNotFound, StatusCodes.Status404NotFound));
           
            var result  = (await _productImagesRopesitry.GetAllAsync(productId)); 
            return Result<List<ProductImageResponse>>.Seccuss(result);
        }

    }


}