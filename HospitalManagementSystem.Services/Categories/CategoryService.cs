using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models.Models.Categories;

namespace HospitalManagementSystem.Services.Categories
{
    public class CategoryService : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;
        public CategoryService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<object> CreateCategory(CreateUpdateCategoryDto createUpdateCategory)
        {
            try
            {
                var category = _mapper.Map<Category>(createUpdateCategory);
                category.CreatedDate = DateTime.Now;
                category.Id = Guid.NewGuid();
                 await _dbContext.Categories.AddAsync(category);
                 await _dbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message.Trim().ToString());
            }
        }


        public async Task<object> UpdateCategory(Guid id, CreateUpdateCategoryDto createUpdateCategory)
        {
            try
            {
                var result = await _dbContext.Categories.FindAsync(id);
                
                result.CategoryName = createUpdateCategory.CategoryName;
                result.CategoryDescription = createUpdateCategory.CategoryDescription;
                result.UpdateDate = DateTime.Now;
                result.UpdateBy=createUpdateCategory.Id;
                
                 _dbContext.Categories.Update(result);
                await _dbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message.Trim().ToString());
            }
        }

        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _dbContext.Categories.Where(x=>x.Id!=id && x.CategoryName.ToUpper()==name.ToUpper()).FirstOrDefaultAsync();
            var category = _mapper.Map<GetCategoryDto>(data);
            return category;
        }

        public async Task<GetCategoryDto> GetCategory(Guid id)
        {
            var data = await _dbContext.Categories.FindAsync(id);
            var category = _mapper.Map<GetCategoryDto>(data);
            return category;
        }

        public async Task<object> GetCategoryByName(string name)
        {
            var data = await _dbContext.Categories.Where(x=>x.CategoryName.ToUpper()==name.ToUpper()).FirstOrDefaultAsync();
            var category = _mapper.Map<GetCategoryDto>(data);
            return category;
        }

        public async Task<List<GetCategoryDto>> GetCategoryList()
        {
            var data = await _dbContext.Categories.OrderByDescending(x=>x.CreatedDate).ToListAsync();
            var category = _mapper.Map<List<GetCategoryDto>>(data);
            return category;
        }


    }
}
