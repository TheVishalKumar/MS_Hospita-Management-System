using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Diseases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class DiseaseValidation : AbstractValidator<CreateDiseaseDto>
    {
        public DiseaseValidation()
        {
            RuleFor(x => x.DiseaseName)
               .NotEmpty().WithMessage(DiseaseConsts.DiseaseNameBlankError)
               .MaximumLength(DiseaseConsts.DiseaseNameLength);

            RuleFor(x => x.DiseaseDescription)
              .NotEmpty().WithMessage(DiseaseConsts.DescriptionLengthError)
              .MaximumLength(DiseaseConsts.DescriptionLength);
        }
    }

    public class UpdateDiseaseValidation : AbstractValidator<UpdateDiseaseDto>
    {
        public UpdateDiseaseValidation()
        {
            RuleFor(x => x.DiseaseName)
               .NotEmpty().WithMessage(DiseaseConsts.DiseaseNameBlankError)
               .MaximumLength(DiseaseConsts.DiseaseNameLength);

            RuleFor(x => x.DiseaseDescription)
              .NotEmpty().WithMessage(DiseaseConsts.DescriptionLengthError)
              .MaximumLength(DiseaseConsts.DescriptionLength);
        }
    }
}
