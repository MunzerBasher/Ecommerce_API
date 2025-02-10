namespace EcommerceLogicalLayer.DataValidation
{
    public class AddressValidation : AbstractValidator<AddressRequest>
    {

        public AddressValidation() 
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x =>x.City).Length(3,50).NotEmpty();
            RuleFor(x => x.AddressLine).Length(3, 50).NotEmpty();
            RuleFor(x => x.Country).Length(3, 50).NotEmpty();
            RuleFor(x => x.Longitude).NotEmpty();
            RuleFor(x => x.Latitude).NotEmpty();
            RuleFor(x =>x).Must(CheckData).WithName(nameof(AddressRequest.AddressLine)).WithMessage("Invalid Data {}");    
        }


        private bool CheckData(AddressRequest dto)
        {
            if (dto == null) return false;
            return true;
        }
    }

}