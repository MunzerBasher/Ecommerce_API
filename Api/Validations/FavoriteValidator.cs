
namespace EcommerceLogicalLayer.Validations
{
    public class FavoriteValidator : AbstractValidator<FavoriteDTO>
    {
        public FavoriteValidator() 
        {
            RuleFor(x => x.UserID).NotEmpty();
            RuleFor(x => x.ProductID).NotEmpty().GreaterThanOrEqualTo(1);
        }


    }
}
