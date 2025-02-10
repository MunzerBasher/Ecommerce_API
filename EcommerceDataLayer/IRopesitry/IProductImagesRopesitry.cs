using EcommerceDataLayer.Entities.Products;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductImagesRopesitry
    {
        Task<int> AddAsync(ProductImageRequest productImage);

        Task<List<ProductImageResponse>> GetAllAsync(int productId);

        Task<int> DeleteAsync(int imageId);

        Task<string> GetImageURL(int @ImageId);

        Task<bool> IsExistAsync(int @ImageId);
    }
}
