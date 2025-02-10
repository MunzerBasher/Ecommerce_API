namespace EcommerceApi.Validations
{
    public class ItemRequestValidator : AbstractValidator<ItemRequest>
    {
        public ItemRequestValidator() 
        {
            RuleFor(x => x.Quantity).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.ProductId).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
