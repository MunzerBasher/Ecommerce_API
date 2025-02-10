

using EcommerceDataLayer.Entities.Categories;

namespace EcommerceApi.Validations
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator() 
        {
        
            RuleFor(x => x.CategoryName).NotEmpty().Length(3,50);
            RuleFor(x => x.ImageUrl).NotEmpty().Length(3,250);
        }
    }
}
