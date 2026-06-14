using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Doctors;

namespace HospitalManagementSystem.Models.Validations
{
    public class DoctorValidation : AbstractValidator<CreateDoctorDto>
    {
        public DoctorValidation()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(DoctorMasterConsts.FirstNameEmptyError)
            .Length(DoctorMasterConsts.UserNameMinLength, DoctorMasterConsts.UserNameMaxLength).WithMessage(DoctorMasterConsts.FirstNameLengthError);
            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage(DoctorMasterConsts.LastNameEmptyError)
               .Length(DoctorMasterConsts.UserNameMinLength, DoctorMasterConsts.UserNameMaxLength).WithMessage(DoctorMasterConsts.LastNameLengthError);
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage(DoctorMasterConsts.AgeEmptyError)
                .InclusiveBetween(DoctorMasterConsts.MinAge, DoctorMasterConsts.MaxAge).WithMessage(DoctorMasterConsts.AgeLengthError);
            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage(DoctorMasterConsts.MobileNoEmptyError)
                .Length(DoctorMasterConsts.MobileNoMaxLength).WithMessage(DoctorMasterConsts.MobileNoLengthError);
            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage(DoctorMasterConsts.EmailIdEmptyError)
                .EmailAddress().WithMessage(DoctorMasterConsts.EmailIdCorrectFormat);
        }
    }

    public class UpdateDoctorValidation : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorValidation()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(DoctorMasterConsts.FirstNameEmptyError)
            .Length(DoctorMasterConsts.UserNameMinLength, DoctorMasterConsts.UserNameMaxLength).WithMessage(DoctorMasterConsts.FirstNameLengthError);
            RuleFor(x => x.LastName)
               .NotEmpty().WithMessage(DoctorMasterConsts.LastNameEmptyError)
               .Length(DoctorMasterConsts.UserNameMinLength, DoctorMasterConsts.UserNameMaxLength).WithMessage(DoctorMasterConsts.LastNameLengthError);
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage(DoctorMasterConsts.AgeEmptyError)
                .InclusiveBetween(DoctorMasterConsts.MinAge, DoctorMasterConsts.MaxAge).WithMessage(DoctorMasterConsts.AgeLengthError);
            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage(DoctorMasterConsts.MobileNoEmptyError)
                .Length(DoctorMasterConsts.MobileNoMaxLength).WithMessage(DoctorMasterConsts.MobileNoLengthError);
            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage(DoctorMasterConsts.EmailIdEmptyError)
                .EmailAddress().WithMessage(DoctorMasterConsts.EmailIdCorrectFormat);
        }
    }

}
