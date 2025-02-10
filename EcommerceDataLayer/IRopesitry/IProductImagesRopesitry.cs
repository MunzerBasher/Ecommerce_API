
using EcommerceDataLayer.DTOS;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductImagesRopesitry
    {
        Task<int> AddAsync(ProductImageRequest productImage);

        Task<List<ProductImageResponse>> GetAllAsync(int productId);

        Task<int> DeleteAsync(int imageId);
    }
}
