using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.HospitalBranches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class HospitalBranchValidation : AbstractValidator<CreateHospitalBranchDto>
    {
        public HospitalBranchValidation()
        {
            RuleFor(x => x.BranchName)
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
                .EmailAddress().WithMessage(HospitalConst.EmailIdCorrectFormat)
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError);
        }
    }

    public class UpdateHospitalBranchValidation : AbstractValidator<UpdateHospitalBranchDto>
    {
        public UpdateHospitalBranchValidation()
        {
            RuleFor(x => x.BranchName)
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
                .EmailAddress().WithMessage(HospitalConst.EmailIdCorrectFormat)
                .NotEmpty().WithMessage(HospitalConst.EmailIdBlankError);
        }
    }
}
