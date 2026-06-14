using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Models.Medicines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class CreateMedicineValidations : AbstractValidator<CreateMedicineDto>
    {
        public CreateMedicineValidations()
        {
            RuleFor(x=>x.MedicineName)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineNameEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineNameEmptyError)
                .Length(MedicineConsts.MedicineNameMinLength, MedicineConsts.MedicineNameMaxLength);

            RuleFor(x => x.HSNCode)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineHSNCodeEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineHSNCodeEmptyError);

            RuleFor(x => x.Amount)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineAmountEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineAmountEmptyError);
            RuleFor(x => x.Quantity)
               //.NotEmpty().WithMessage(MedicineConsts.MedicineQuantityEmptyError)
               .NotEmpty().WithMessage(MedicineConsts.MedicineQuantityEmptyError);

            RuleFor(x => x.MedicineDescription)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineDescriptionEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineDescriptionEmptyError)
                .Length(MedicineConsts.MedicineDescriptionMinLength, MedicineConsts.MedicineDescriptionMaxLength);
        }
    }

    public class UpdateMedicineValidations : AbstractValidator<UpdateMedicineDto>
    {
        public UpdateMedicineValidations()
        {
            RuleFor(x => x.MedicineName)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineNameEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineNameEmptyError)
                .Length(MedicineConsts.MedicineNameMinLength, MedicineConsts.MedicineNameMaxLength);

            RuleFor(x => x.HSNCode)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineHSNCodeEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineHSNCodeEmptyError);

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage(MedicineConsts.MedicineAmountEmptyError);


            RuleFor(x => x.Quantity)
               //.NotEmpty().WithMessage(MedicineConsts.MedicineQuantityEmptyError)
               .NotEmpty().WithMessage(MedicineConsts.MedicineQuantityEmptyError);

            RuleFor(x => x.MedicineDescription)
                //.NotEmpty().WithMessage(MedicineConsts.MedicineDescriptionEmptyError)
                .NotEmpty().WithMessage(MedicineConsts.MedicineDescriptionEmptyError)
                .Length(MedicineConsts.MedicineDescriptionMinLength, MedicineConsts.MedicineDescriptionMaxLength);
        }
    }
}
