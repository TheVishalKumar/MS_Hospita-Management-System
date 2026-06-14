using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Patients;

namespace HospitalManagementSystem.Models.Validations
{
    public class PatientDetailsValidation : AbstractValidator<CreatePatientDto>
    {
        public PatientDetailsValidation()
        {
            RuleFor(x=>x.FirstName)
                //.NotNull().WithMessage(PatientDetailsConsts.FirstNameEmptyError)
                .NotEmpty().WithMessage(PatientDetailsConsts.FirstNameEmptyError)
                .Length(PatientDetailsConsts.PatientNameMinLength, PatientDetailsConsts.PatientNameMaxLength);
            RuleFor(x => x.LastName)
               //.NotNull().WithMessage(PatientDetailsConsts.LastNameEmptyError)
               .NotEmpty().WithMessage(PatientDetailsConsts.LastNameEmptyError)
               .Length(PatientDetailsConsts.PatientNameMinLength, PatientDetailsConsts.PatientNameMaxLength);
            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage(PatientDetailsConsts.MobileNoLengthError)
                //.NotNull().WithMessage(PatientDetailsConsts.AgeEmptyError)
                .Length(PatientDetailsConsts.MobileNoMaxLength);
        }
    }

    public class UpdatePatientDetailsValidation : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientDetailsValidation()
        {
            RuleFor(x => x.FirstName)
                //.NotNull().WithMessage(PatientDetailsConsts.FirstNameEmptyError)
                .NotEmpty().WithMessage(PatientDetailsConsts.FirstNameEmptyError)
                .Length(PatientDetailsConsts.PatientNameMinLength, PatientDetailsConsts.PatientNameMaxLength);
            RuleFor(x => x.LastName)
               //.NotNull().WithMessage(PatientDetailsConsts.LastNameEmptyError)
               .NotEmpty().WithMessage(PatientDetailsConsts.LastNameEmptyError)
               .Length(PatientDetailsConsts.PatientNameMinLength, PatientDetailsConsts.PatientNameMaxLength);
            //RuleFor(x => x.Age)
            //    .NotEmpty().WithMessage(PatientDetailsConsts.AgeEmptyError)
            //    //.NotNull().WithMessage(PatientDetailsConsts.AgeEmptyError)
            //    .InclusiveBetween(PatientDetailsConsts.MinAge, PatientDetailsConsts.MaxAge).WithMessage(PatientDetailsConsts.AgeLengthError);
        }
    }
}
