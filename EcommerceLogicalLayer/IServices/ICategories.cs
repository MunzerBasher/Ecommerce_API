using EcommerceDataLayer.Entities.Categories;
using EcommerceLogicalLayer.Helpers;
using System.Data;

namespace EcommerceDataLayer.IRopesitry
{
    public interface ICategoriesServices
    {

        public Task<Result<bool>> Add(CategoryRequest CategoryRequest);


        public Task<Result<CategoryResponse>> GetById(int categoryId);

        public Task<Result<bool>> Update(int categoryId, CategoryRequest categoryRequest);


        public Task<Result<bool>> ToggleStatus(int categoryId);

        public Task<Result<List<CategoryResponse>>> GetAll();

        public Task<bool> IsExistAsync(int categoryId);

        public Task<Result<List<CategoryResponse>>> Search(string firstChar);


    }
}
