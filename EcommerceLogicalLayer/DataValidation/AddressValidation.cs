using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLogicalLayer.DataValidation
{
    public class AddressValidation : AbstractValidator<AddressDTO>
    {

        public AddressValidation() 
        {
            RuleFor(x => x.UserID).NotEmpty();
            RuleFor(x =>x.City).NotEmpty();
            RuleFor(x =>x).Must(CheckData).WithName(nameof(AddressDTO.AddressLine)).WithMessage("Invalid Data {}");
        }


        private bool CheckData(AddressDTO dto)
        {
            if (dto == null) return false;
            return true;
        }
    }
}
