using EcommerceDataLayer.Entities.Products;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{

    public interface IProductImagesServices
    {

        Task<Result<int>> Add(ProductImageRequest productImage);

        Task<Result<List<ProductImageResponse>>> GetAll(int productId);
   
        Task<Result<int>> Delete(int imageId);

        
    }

}