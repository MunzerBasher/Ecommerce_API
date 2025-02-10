namespace EcommerceApi.Validations
{
    public class ShippingsValidator : AbstractValidator<ShippingRequest>
    {
        public ShippingsValidator()
        {
            RuleFor(x => x.CarrierName).NotEmpty().Length(2,50);
            RuleFor(x => x.OrderID).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.ShippingStatus).NotEmpty().InclusiveBetween<ShippingRequest,short>(1,7);
            RuleFor(x => x.EstimatedDeliveryDate).NotEmpty().GreaterThanOrEqualTo(DateTime.UtcNow);
            RuleFor(x => x).Must(CheckDate).WithMessage("Actual Delivery Date Most Be Than Or Equal Estimated Delivery Date");
        }

        private bool CheckDate(ShippingRequest shippingRequest)
        {
            if(shippingRequest == null) 
                return false;
            return shippingRequest.ActualDeliveryDate >= shippingRequest.EstimatedDeliveryDate;
        }

    }
}
