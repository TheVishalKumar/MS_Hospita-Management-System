using FluentValidation;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Validations
{
    public class CategoryValidation : AbstractValidator<CreateUpdateCategoryDto>
    {
        public CategoryValidation()
        {
            RuleFor(x => x.CategoryName)
                //.NotNull().WithMessage(CategoryConsts.EmptryCategoryError)
                .NotEmpty().WithMessage(CategoryConsts.EmptryCategoryError)
                .Length(CategoryConsts.CategoryNameMinLength, CategoryConsts.CategoryNameMaxLength).WithMessage(CategoryConsts.CategoryNameError);
            RuleFor(x => x.CategoryDescription).NotNull()
                .NotEmpty().WithMessage(CategoryConsts.EmptyDescriptionError)
                .Length(CategoryConsts.DescriptionMinLength, CategoryConsts.DescriptionMaxLength).WithMessage(CategoryConsts.DescriptionError);
            
        }
    }
}
