
using EcommerceDataLayer.DTOS;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductRopesitry
    {
        Task<List<ProductResponse>> GetAllAsync();

        Task<List<ProductResponse>> GetProductsByFirstCharAsync(string firstChar);

        Task<bool> AddAsync(ProductResponse product);

        Task<bool> UpdateAsync(ProductResponse product);

        Task<bool> DeleteAsync(int id);

        Task<ProductResponse> GetByIdAsync(int id);

        Task<bool> IsExistAsync(int id);

    }
}
