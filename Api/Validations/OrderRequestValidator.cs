namespace EcommerceApi.Validations
{
    public class OrderRequestValidator: AbstractValidator<OrderRequest>
    {

        public OrderRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleForEach(x => x.Items).SetInheritanceValidator(item => item.Add(new ItemRequestValidator()));
        }

    }
}
