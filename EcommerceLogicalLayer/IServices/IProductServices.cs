using EcommerceDataLayer.Entities.Products;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductServices
    {

        public Task<Result<List<ProductResponse>>> GetAll(CancellationToken cancellationToken = default);



        public Task<Result<List<ProductResponse>>> GetProductsByFirstChar(string firstChar);


        public Task<Result<bool>> Add(ProductRequest product);



        public Task<Result<bool>> Update(ProductRequest product, int ProductId);



        public Task<Result> Delete(int productId);

        public Task<bool> IsExistAsync(int ProductID);

        public Task<Result<int>> ProductvalibaleQuantity(int ProductID);

        public Task<Result<ProductResponse>> GetByIdAsync(int id);


    }
}
