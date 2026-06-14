using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Constants
{
    public static class CategoryConsts
    {
        public const int CategoryNameMinLength = 2;
        public const int CategoryNameMaxLength = 120;

        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 500;
        public const string CategoryNameError = "Category name max length should be 120.";
        public const string DescriptionError = "Description max length should be 500.";
        public const string EmptryCategoryError = "Category name can not be blank or empty.";
        public const string EmptyDescriptionError = "Category name can not be blank or empty.";

    }
}
