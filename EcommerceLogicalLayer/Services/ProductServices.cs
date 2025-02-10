using EcommerceDataLayer.Entities.Products;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Errors;
using EcommerceLogicalLayer.Helpers;
using EcommerceLogicalLayer.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;




namespace EcommerceLogicalLayer.Services
{
    public class ProductServices(IProductRopesitry productRopesitry, 
        ICategoriesServices categoriesServices,
        ICacheServices distributedCache , ILogger<IProductServices> logger
        ) : IProductServices
    {
        private readonly ICacheServices _distributedCache = distributedCache;
        private readonly ILogger<IProductServices> _logger = logger;
        private const string _cachePrefix = "Products53#22";

        private readonly IProductRopesitry _productRopesitry = productRopesitry;
        private readonly ICategoriesServices _categoriesServices = categoriesServices;
       
        public async Task<Result<List<ProductResponse>>> GetAll(CancellationToken cancellationToken = default)
        {
            var value = await _distributedCache.GetTAsync<List<ProductResponse>>(_cachePrefix, cancellationToken);
            if(value is null)
            {
                _logger.LogInformation("form DataBase");
                var result = await _productRopesitry.GetAllAsync();
                await _distributedCache.SetAsync<IList<ProductResponse>>(_cachePrefix,result,cancellationToken);
                return Result<List<ProductResponse>>.Seccuss(result);
            }
               _logger.LogInformation("form Cache");
            return Result<List<ProductResponse>>.Seccuss(value);
        }

        public async Task<Result<List<ProductResponse>>> GetProductsByFirstChar(string firstChar)
        {
            var result = await _productRopesitry.GetProductsByFirstCharAsync(firstChar);
            return Result<List<ProductResponse>>.Seccuss(result);
        }

        public async Task<Result<bool>> Add(ProductRequest product)
        {
            if(await _productRopesitry.IsExistNameAsync(product.ProductName))
                Result<bool>.Failure<bool>(new Error(ProductsError.Duplicated, StatusCodes.Status400BadRequest));
            if(!await _categoriesServices.IsExistAsync(product.CategoryId))
                return Result<bool>.Failure<bool>(new Error(CategoriesError.NotFound, StatusCodes.Status400BadRequest));
            var result = await _productRopesitry.AddAsync(product);
            if (result)
                await _distributedCache.RemoveAsync(_cachePrefix);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Failure<bool>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<bool>> Update(ProductRequest product, int ProductId)
        {
            if (!await _categoriesServices.IsExistAsync(product.CategoryId))
                return Result<bool>.Failure<bool>(new Error(CategoriesError.NotFound, StatusCodes.Status400BadRequest));

            var IsExist = await _productRopesitry.IsExistAsync(ProductId);
            if(!IsExist)
                return Result<bool>.Failure<bool>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            var IsNameExist = await _productRopesitry.IsExistNameAsync(product.ProductName);
            if (IsNameExist)
            {
                var value = await  _productRopesitry.GetByNameAsync(product.ProductName);
                if(value < 0)
                    Result<bool>.Failure<bool>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));
                if(value != ProductId)
                    Result<bool>.Failure<bool>(new Error(ProductsError.Duplicated, StatusCodes.Status400BadRequest));
            }
            var result = await _productRopesitry.UpdateAsync(product, ProductId);
            if (result)
                await _distributedCache.RemoveAsync(_cachePrefix);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Failure<bool>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));



        }

        public async Task<Result> Delete(int productId)
        {
            var IsExist = await _productRopesitry.IsExistAsync(productId);
            if (!IsExist)
                return Result<bool>.Failure<bool>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            var result = await _productRopesitry.DeleteAsync(productId);
            if (result)
                await _distributedCache.RemoveAsync(_cachePrefix);
            return result ? Result<bool>.Seccuss(result) : Result<bool>.Failure<bool>(new Error(ProductsError.ServerError, StatusCodes.Status500InternalServerError));

        }

        public async Task<bool> IsExistAsync(int ProductID)
        {
            var result = await _productRopesitry.IsExistAsync(ProductID);
            return result;
        }



        public async Task<Result<int>> ProductvalibaleQuantity(int ProductId)
        {
            var IsExist = await _productRopesitry.IsExistAsync(ProductId);
            if (!IsExist)
                return Result<int>.Failure<int>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));
            
            var Quantity = await _productRopesitry.ProductvalibaleQuantity(ProductId);
            return Result<int>.Seccuss(Quantity);
        }

        public async Task<Result<ProductResponse>> GetByIdAsync(int id)
        {
            var IsExist = await _productRopesitry.IsExistAsync(id);
            if (!IsExist)
                return Result<ProductResponse>.Failure<ProductResponse>(new Error(ProductsError.ImageNotFound, StatusCodes.Status404NotFound));

            var result = await _productRopesitry.GetByIdAsync(id);
            return result is not null? Result<ProductResponse>.Seccuss(result) : Result<ProductResponse>.Failure<ProductResponse>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));

        }
    }

} 