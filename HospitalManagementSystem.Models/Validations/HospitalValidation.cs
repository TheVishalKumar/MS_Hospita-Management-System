using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Hospitals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class HospitalValidation : AbstractValidator<CreateHospitalDto>
    {
        public HospitalValidation()
        {
            RuleFor(x => x.HospitalName)
                .NotEmpty().WithMessage(HospitalConst.HospitalNameEmptyError)
                .MaximumLength(HospitalConst.HospitalNameMaxLength);

            RuleFor(x => x.ContactNo)
               .NotEmpty().WithMessage(HospitalConst.MobileNoBlankError)
               .Length(HospitalConst.MobileNoMaxLength).WithMessage(HospitalConst.MobileNoLengthError);

            RuleFor(x => x.FaxNo)
                .NotEmpty().WithMessage(HospitalConst.FaxNoBlankError)
                .Length(HospitalConst.FaxNoMaxLength).WithMessage(HospitalConst.FaxNoLengthError);

            RuleFor(x => x.GSTNo)
                .NotEmpty().WithMessage(HospitalConst.GSTNoBlankError)
                .Length(HospitalConst.GSTNoMaxLength).WithMessage(HospitalConst.GSTNoLengthError);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(HospitalConst.DescriptionEmptyError)
                .MaximumLength(HospitalConst.DescriptionMaxLength).WithMessage(HospitalConst.DescriptionLengthError);

            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError)
                //.EmailAddress().WithMessage(HospitalConst.EmailIdCorrectFormat)
                .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError);
        }
    }

    public class UpdateHospitalValidation : AbstractValidator<UpdateHospitalDto>
    {
        public UpdateHospitalValidation()
        {
            RuleFor(x => x.HospitalName)
                .NotEmpty().WithMessage(HospitalConst.HospitalNameEmptyError)
                .MaximumLength(HospitalConst.HospitalNameMaxLength);

            RuleFor(x => x.ContactNo)
               .NotEmpty().WithMessage(HospitalConst.MobileNoBlankError)
               .Length(HospitalConst.MobileNoMaxLength).WithMessage(HospitalConst.MobileNoLengthError);

            RuleFor(x => x.FaxNo)
                .NotEmpty().WithMessage(HospitalConst.FaxNoBlankError)
                .Length(HospitalConst.FaxNoMaxLength).WithMessage(HospitalConst.FaxNoLengthError);

            RuleFor(x => x.GSTNo)
                .NotEmpty().WithMessage(HospitalConst.GSTNoBlankError)
                .Length(HospitalConst.GSTNoMaxLength).WithMessage(HospitalConst.GSTNoLengthError);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(HospitalConst.DescriptionEmptyError)
                .MaximumLength(HospitalConst.DescriptionMaxLength).WithMessage(HospitalConst.DescriptionLengthError);

            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError)
                //.EmailAddress().WithMessage(HospitalConst.EmailIdCorrectFormat)
                .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$")
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError);
        }
    }
}
