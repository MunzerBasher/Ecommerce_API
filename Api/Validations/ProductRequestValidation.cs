using EcommerceDataLayer.Entities.Products;

namespace EcommerceApi.Validations
{
    public class ProductRequestValidation : AbstractValidator<ProductResponse>
    {
        public ProductRequestValidation()
        {         
            RuleFor(x => x.ProductName).NotEmpty().Length(3,30);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(1);
            RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(1);
        }


    }
}
