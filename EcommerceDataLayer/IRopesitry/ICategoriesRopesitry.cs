using EcommerceDataLayer.Entities.Categories;


namespace EcommerceDataLayer.IRopesitry
{
    public interface ICategoriesRopesitry
    {
        Task<bool> AddAsync(CategoryRequest  categoryRequest);
        Task<CategoryResponse> GetByIdAsync(int categoryId);
        Task<bool> UpdateAsync(int categoryId, CategoryRequest categoryRequest);
        Task<bool> ToggleStatusAsync(int categoryId);

        public Task<bool> IsExistAsync(int ProductID);
        public  Task<bool> IsExistNameAsync(string ProductID);
        Task<List<CategoryResponse>> GetAllAsync();
        Task<List<CategoryResponse>> SearchAsync(string firstChar);
        public Task<CategoryResponse> GetByNameAsync(string categoryName);

    }



}
