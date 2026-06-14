using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.Exceptions;
using HospitalManagementSystem.Models.Validations;
using HospitalManagementSystem.Services.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using HospitalManagementSystem.Shared.Response;
using HospitalManagementSystems.API.Helpers;

namespace HospitalManagementSystems.API.Controllers.Category
{
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        //[Authorize]
        [HttpPost]
        [Route("CreateCategory")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateCategory(CreateUpdateCategoryDto createUpdateCategory)
        {
            try
            {
                string? error = "";
                CategoryValidation validations = new CategoryValidation();
                var result = validations.Validate(createUpdateCategory);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }
                var checkDuplication = await _categoryRepository.GetCategoryByName(createUpdateCategory.CategoryName ?? "");
                if (checkDuplication != null)
                {
                    return this.BadRequestResponse<object>(null, $"Category already exist with {createUpdateCategory.CategoryName} name");
                }
                var createResult = await _categoryRepository.CreateCategory(createUpdateCategory);
                return this.CreatedResponse(createResult, "Category created successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error creating category: {ex.Message}");
            }
        }

        
        [HttpPut]
        [Route("UpdateCategory/{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateCategory(Guid id, CreateUpdateCategoryDto createUpdateCategory)
        {
            try
            {
                string? error = "";
                CategoryValidation validations = new CategoryValidation();
                var result = validations.Validate(createUpdateCategory);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        error += item.ErrorMessage + "\n";
                    }
                    return this.BadRequestResponse<object>(null, error);
                }
                var checkDuplication = await _categoryRepository.CheckDuplicateForEdit(id, createUpdateCategory.CategoryName ?? "");
                
                if (checkDuplication != null)
                {
                    return this.BadRequestResponse<object>(null, $"Category already exist with {createUpdateCategory.CategoryName} name");
                }
                var updateResult = await _categoryRepository.UpdateCategory(id, createUpdateCategory);
                return this.OkResponse(updateResult, "Category updated successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<object>(null, $"Error updating category: {ex.Message}");
            }
        }

        
        [HttpGet]
        [Route("GetCategory/{id}")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff)]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetCategory(id);
                if (category == null)
                {
                    return this.NotFoundResponse<GetCategoryDto>(null, $"Category ID {id} not found.");
                }
                return this.OkResponse(category, "Category retrieved successfully");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<GetCategoryDto>(null, $"Error retrieving category: {ex.Message}");
            }
        }


        //[Authorize]
        [HttpGet]
        [Route("GetCategoryList")]
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.PharmacyStaff + "," + UserRoles.Doctor)]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var category = await _categoryRepository.GetCategoryList();
                if (category != null && category.Count > 0)
                {
                    return this.OkResponse(category, "Category list retrieved successfully");
                }
                return this.NotFoundResponse<List<GetCategoryDto>>(null, "No categories found.");
            }
            catch (Exception ex)
            {
                return this.ServerErrorResponse<List<GetCategoryDto>>(null, $"Error retrieving category list: {ex.Message}");
            }
        }
    }
}
