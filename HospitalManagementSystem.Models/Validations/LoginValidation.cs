using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class LoginValidation : AbstractValidator<LoginDto>
    {
        public LoginValidation() 
        {
            RuleFor(x => x.UserId)
                   .NotEmpty().WithMessage(UserDetailsConsts.UserIdRequiredError);

            RuleFor(x => x.Password)
              .NotEmpty().WithMessage(UserDetailsConsts.PasswordRequiredError);
        }
    }
}
