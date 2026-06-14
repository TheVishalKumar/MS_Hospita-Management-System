using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Diseases;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Models.Diseases;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services.Diseases
{
    public class DiseaseService : IDiseaseRepository
    {

        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;
        public DiseaseService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
                 
        }
        public async Task<Response> CreateAsyn(CreateDiseaseDto createDiseaseDto)
        {
            try
            {
                var disease = _mapper.Map<DiseaseMaster>(createDiseaseDto);
                disease.Id = Guid.NewGuid();
                disease.CreatedDate = DateTime.Now;
                await _appDbContext.AddAsync(disease);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.Success, ex.Message);
            }
        }

        public async Task<GetDiseaseListDto> GetById(Guid id)
        {
            var data = await _appDbContext.DiseaseMaster.Where(x=>x.Id==id && x.IsActive==true).FirstOrDefaultAsync();
            var disease = _mapper.Map<GetDiseaseListDto>(data);
            return disease;
        }

        public async Task<GetDiseaseListDto> GetByName(string name)
        {
            var data = await _appDbContext.DiseaseMaster.Where(x=>x.DiseaseName == name && x.IsActive==true).FirstOrDefaultAsync();
            var disease = _mapper.Map<GetDiseaseListDto>(data);
            return disease;
        }

        public async Task<List<GetDiseaseListDto>> GetListAsync()
        {
            var data = await _appDbContext.DiseaseMaster.Where(x=>x.IsActive==true).ToListAsync();
            var disease = _mapper.Map<List<GetDiseaseListDto>>(data);
            return disease;
        }

        public async Task<Response> UpdateAsyn(UpdateDiseaseDto updateDiseaseDto)
        {
            try
            { 
                var data = await _appDbContext.DiseaseMaster.FindAsync(updateDiseaseDto.Id);
                var disease = _mapper.Map<DiseaseMaster>(updateDiseaseDto);
                data.DiseaseName = disease.DiseaseName;
                data.DiseaseDescription = disease.DiseaseDescription;
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = disease.UpdateBy;
                data.IsActive=disease.IsActive;

                await _appDbContext.AddAsync(data);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.Success, ex.Message);
            }
        }

        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
             var data = await _appDbContext.DiseaseMaster.FindAsync(id);
            data.IsActive = status;
            await _appDbContext.AddAsync(data);
            await _appDbContext.SaveChangesAsync();

            return true;
        }


        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _appDbContext.DiseaseMaster.Where(x => x.Id != id && x.DiseaseName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetDiseaseListDto>(data);
            return medicine;
        }

    }
}
