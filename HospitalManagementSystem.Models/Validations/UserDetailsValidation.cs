using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class UserDetailsValidation: AbstractValidator<CreateUserDetailsDto>
    {
        public UserDetailsValidation()
        {
            RuleFor(x=>x.FirstName)
                //.NotNull().WithMessage(UserDetailsConsts.FirstNameEmptyError)
                .NotEmpty().WithMessage(UserDetailsConsts.FirstNameEmptyError)
                .Length(UserDetailsConsts.UserNameMinLength, UserDetailsConsts.UserNameMaxLength);
            RuleFor(x => x.LastName)
               //.NotNull().WithMessage(UserDetailsConsts.LastNameEmptyError)
               .NotEmpty().WithMessage(UserDetailsConsts.LastNameEmptyError)
               .Length(UserDetailsConsts.UserNameMinLength, UserDetailsConsts.UserNameMaxLength);
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage(UserDetailsConsts.AgeEmptyError)
                //.NotNull().WithMessage(UserDetailsConsts.AgeEmptyError)
                .InclusiveBetween(UserDetailsConsts.MinAge, UserDetailsConsts.MaxAge).WithMessage(UserDetailsConsts.AgeLengthError);
        }
    }

    public class UpdateUserDetailsValidation : AbstractValidator<UpdateUserDetailsDto>
    {
        public UpdateUserDetailsValidation()
        {
            RuleFor(x => x.FirstName)
                //.NotNull().WithMessage(UserDetailsConsts.FirstNameEmptyError)
                .NotEmpty().WithMessage(UserDetailsConsts.FirstNameEmptyError)
                .Length(UserDetailsConsts.UserNameMinLength, UserDetailsConsts.UserNameMaxLength);
            RuleFor(x => x.LastName)
               //.NotNull().WithMessage(UserDetailsConsts.LastNameEmptyError)
               .NotEmpty().WithMessage(UserDetailsConsts.LastNameEmptyError)
               .Length(UserDetailsConsts.UserNameMinLength, UserDetailsConsts.UserNameMaxLength);
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage(UserDetailsConsts.AgeEmptyError)
                //.NotNull().WithMessage(UserDetailsConsts.AgeEmptyError)
                .InclusiveBetween(UserDetailsConsts.MinAge, UserDetailsConsts.MaxAge).WithMessage(UserDetailsConsts.AgeLengthError);
        }
    }
}
