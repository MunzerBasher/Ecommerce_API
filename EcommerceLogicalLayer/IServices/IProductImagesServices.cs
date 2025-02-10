
using EcommerceDataLayer.DTOS;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductImagesServices
    {

        public Task<Result<int>> Add(ProductImageRequest productImage);

        public Task<Result<List<ProductImageResponse>>> GetAll(int productId);
   
        public Task<Result<int>> Delete(int imageId);
    }
}
