using EcommerceDataLayer.Entities.Products;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductRopesitry
    {
        Task<List<ProductResponse>> GetAllAsync();

        Task<List<ProductResponse>> GetProductsByFirstCharAsync(string firstChar);

        Task<bool> AddAsync(ProductRequest product);

        Task<bool> UpdateAsync(ProductRequest product , int productId);

        Task<bool> DeleteAsync(int id);

        Task<ProductResponse> GetByIdAsync(int id);

        Task<bool> IsExistAsync(int id);

        Task<int> ProductvalibaleQuantity(int ProductId);

        Task<bool> IsExistNameAsync(string Name);

        Task<int> GetByNameAsync(string ProductName);

    }
}
