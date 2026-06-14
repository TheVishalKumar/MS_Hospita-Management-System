using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Wards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class WardValidation : AbstractValidator<CreateWardDto>
    {
        public WardValidation()
        {
            RuleFor(x => x.WardName)
               .NotEmpty().WithMessage(WardConst.WardNameBlankError)
               .MaximumLength(WardConst.WardNameLength);

            RuleFor(x => x.WardDescription)
              .NotEmpty().WithMessage(WardConst.DescriptionLengthError)
              .MaximumLength(WardConst.DescriptionLength);
        }
    }

    public class UpdateWardValidation : AbstractValidator<UpdateWardDto>
    {
        public UpdateWardValidation()
        {
            RuleFor(x => x.WardName)
               .NotEmpty().WithMessage(WardConst.WardNameBlankError)
               .MaximumLength(WardConst.WardNameLength);

            RuleFor(x => x.WardDescription)
              .NotEmpty().WithMessage(WardConst.DescriptionLengthError)
              .MaximumLength(WardConst.DescriptionLength);
        }
    }
}
