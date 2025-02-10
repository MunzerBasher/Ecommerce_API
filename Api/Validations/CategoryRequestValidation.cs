

using EcommerceDataLayer.Entities.Categories;

namespace EcommerceApi.Validations
{
    public class CategoryRequestValidation : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidation() 
        {
            RuleFor(x => x.CategoryName).Length(3,20).NotEmpty();
            RuleFor(x => x.ImageUrl).Length(5, 200).NotEmpty();
        }
    }
}
