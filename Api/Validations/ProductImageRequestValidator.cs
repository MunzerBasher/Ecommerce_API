

using EcommerceDataLayer.Entities.Products;

namespace EcommerceApi.Validations
{
    public class ProductImageRequestValidator : AbstractValidator<ProductImageRequest>
    {
        public ProductImageRequestValidator() 
        {
            RuleFor(x => x.ProductID).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.ImageURL).NotEmpty().Length(10, 250);
        }

    }
}
