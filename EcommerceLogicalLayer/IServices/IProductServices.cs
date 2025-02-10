
using EcommerceDataLayer.DTOS;
using EcommerceLogicalLayer.Helpers;

namespace EcommerceDataLayer.IRopesitry
{
    public interface IProductServices
    {

        public Task<Result<List<ProductResponse>>> GetAll();



        public Task<Result<List<ProductResponse>>> GetProductsByFirstChar(string firstChar);


        public Task<Result<bool>> Add(ProductResponse product);



        public Task<Result> Update(ProductResponse product);



        public Task<Result> Delete(int productId);

        public Task<bool> IsExistAsync(int ProductID);


    }
}
