using EcommerceDataLayer.Entities.Categories;
using EcommerceDataLayer.IRopesitry;
using EcommerceLogicalLayer.Helpers;
using Microsoft.AspNetCore.Http;



namespace EcommerceLogicalLayer.Services
{
    public class ProductCategoryLogic(ICategoriesRopesitry categoriesRopesitry) : ICategoriesServices
    {
        private readonly ICategoriesRopesitry _categoriesRopesitry = categoriesRopesitry;

        public async Task<Result<bool>> Add(CategoryRequest CategoryRequest)
        {
            var name = await _categoriesRopesitry.IsExistNameAsync(CategoryRequest.CategoryName);
            if (!name)
                return Result<bool>.Failure<bool>(new Error("Duplicated Name ", StatusCodes.Status400BadRequest));

            var category = await _categoriesRopesitry.AddAsync(CategoryRequest);
            return category ? Result<bool>.Seccuss(category) : 
                Result<bool>.Failure<bool>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));
        }

        public async Task<Result<bool>> ToggleStatus(int categoryId)
        {
            if (categoryId < 1)
                return Result<bool>.Failure<bool>(new Error("Status BadRequest", StatusCodes.Status400BadRequest));
            var category = await _categoriesRopesitry.IsExistAsync(categoryId);
            if(!category)
                return Result<bool>.Failure<bool>(new Error("Not Found", StatusCodes.Status404NotFound));
            var result = await _categoriesRopesitry.ToggleStatusAsync(categoryId);
            return result ? Result<int>.Seccuss<bool>(result) :
                Result<bool>.Failure<bool>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));

        }

        public async Task<Result<List<CategoryResponse>>> GetAll()
        {
            var result = await _categoriesRopesitry.GetAllAsync();
            return Result<List<CategoryResponse>>.Seccuss(result);
        }

        public async Task<Result<CategoryResponse>> GetById(int categoryId)
        {
            if (categoryId < 1)
                return Result<CategoryResponse>.Failure<CategoryResponse>(new Error("Status BadRequest", StatusCodes.Status400BadRequest));
            var category = await _categoriesRopesitry.IsExistAsync(categoryId);
            if (!category)
                return Result<CategoryResponse>.Failure<CategoryResponse>(new Error("Not Found", StatusCodes.Status404NotFound));
            
            var result = await _categoriesRopesitry.GetByIdAsync(categoryId);
            return result is null ? Result<CategoryResponse>.Failure<CategoryResponse>(new Error("Not Found", StatusCodes.Status404NotFound))
                : Result<CategoryResponse>.Seccuss(result);                   

        }

        public async Task<Result<List<CategoryResponse>>> Search(string firstChar)
        {
            if (firstChar is null)
                return Result <List< CategoryResponse >>.Failure < List<CategoryResponse>>(new Error("No Data For Searching", StatusCodes.Status400BadRequest));
            var result = await _categoriesRopesitry.SearchAsync(firstChar); 
            return result is null ? Result<List<CategoryResponse>>.Failure<List<CategoryResponse>>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError))
                : Result<CategoryResponse>.Seccuss(result);

        }

        public async Task<Result<bool>> Update(int categoryId, CategoryRequest categoryRequest)
        {
            if (categoryId < 1 )
                return Result<bool>.Failure<bool>(new Error("Status BadRequest", StatusCodes.Status400BadRequest));
            var category = await _categoriesRopesitry.IsExistAsync(categoryId);
            if (!category)
                return Result<bool>.Failure<bool>(new Error("Not Found", StatusCodes.Status404NotFound));
        
            var IsNameExist = await _categoriesRopesitry.IsExistNameAsync(categoryRequest.CategoryName);
            if(IsNameExist)
            {
                var categor = await _categoriesRopesitry.GetByNameAsync(categoryRequest.CategoryName);
                if(categor is null)
                    Result<bool>.Failure<bool>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));
                if(categor!.CategoryID !=  categoryId)
                    return Result<bool>.Failure<bool>(new Error("Duplicated Name ", StatusCodes.Status400BadRequest));
            }
            var result = await _categoriesRopesitry.UpdateAsync(categoryId, categoryRequest);
            return result ? Result<bool>.Seccuss(result) :
                Result<bool>.Failure<bool>(new Error("Internal Server Error", StatusCodes.Status500InternalServerError));
        }

        public async Task<bool> IsExistAsync(int categoryId)
        {
            return await _categoriesRopesitry.IsExistAsync(categoryId);
        }

    }

}