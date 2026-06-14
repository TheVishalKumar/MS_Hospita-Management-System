using HospitalManagementSystem.Models.Models.Categories;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Categories
{
    public interface ICategoryRepository
    {
        Task<object> CreateCategory(CreateUpdateCategoryDto createUpdateCategory);
        Task<object> UpdateCategory(Guid id, CreateUpdateCategoryDto createUpdateCategory);
        Task<GetCategoryDto> GetCategory(Guid id);
        Task<List<GetCategoryDto>> GetCategoryList();
        Task<object> GetCategoryByName(string name);

        Task<object> CheckDuplicateForEdit(Guid id, string name);

    }
}
