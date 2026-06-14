using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.DTO.Rooms;
using HospitalManagementSystem.Models.Models.Hospitals;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services.Hospitals
{
    public class HospitalService : IHospitalRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public HospitalService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response> CreateAsync(CreateHospitalDto createHospitalDto)
        {
            var HospitalDetails = _mapper.Map<HospitalMaster>(createHospitalDto);
            HospitalDetails.Id = Guid.NewGuid();
            HospitalDetails.CreatedDate = DateTime.Now;
            await _dbContext.HospitalMaster.AddAsync(HospitalDetails);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetHospitalDto> Get(Guid id)
        {
            var data = await _dbContext.HospitalMaster.FindAsync(id);
            var HospitalDetail = _mapper.Map<GetHospitalDto>(data);
            return HospitalDetail;
        }

        public async Task<GetHospitalDto> GetByEmail(string emailId)
        {
            var data = await _dbContext.HospitalMaster.Where(x => x.EmailId == emailId).FirstOrDefaultAsync();
            var HospitalDetail = _mapper.Map<GetHospitalDto>(data);
            return HospitalDetail;
        }

        public async Task<GetHospitalDto> GetByMobile(string mobile)
        {
            var data = await _dbContext.HospitalMaster.Where(x => x.ContactNo == mobile).FirstOrDefaultAsync();
            var HospitalDetail = _mapper.Map<GetHospitalDto>(data);
            return HospitalDetail;
        }

        public async Task<GetHospitalDto> GetByName(string name)
        {
            var data = await _dbContext.HospitalMaster.Where(x => x.HospitalName == name).FirstOrDefaultAsync();
            var HospitalDetail = _mapper.Map<GetHospitalDto>(data);
            return HospitalDetail;
        }

        public async Task<List<GetHospitalDto>> GetList()
        {
            var data = await _dbContext.HospitalMaster.ToListAsync();
            var HospitalDetail = _mapper.Map<List<GetHospitalDto>>(data);
            return HospitalDetail;
        }

        public async Task<Response> UpdateAsync(Guid id, UpdateHospitalDto updateHospitalDto)
        {
            var HospitalDetails = _mapper.Map<HospitalMaster>(updateHospitalDto);
            var data = _dbContext.HospitalMaster.Find(id);

            data.HospitalName = HospitalDetails.HospitalName;
            data.Address = HospitalDetails.Address;
            data.ContactNo = HospitalDetails.ContactNo;
            data.EmailId = HospitalDetails.EmailId;
            data.GSTNo = HospitalDetails.GSTNo;
            data.FaxNo = HospitalDetails.FaxNo;
            data.HospitalLogo = HospitalDetails.HospitalLogo;
            data.Description = HospitalDetails.Description;
            data.IsActive = HospitalDetails.IsActive;
            data.UpdateDate = HospitalDetails.UpdateDate;
            data.UpdateBy = HospitalDetails.UpdateBy;

            _dbContext.HospitalMaster.Update(data);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
        }

        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _dbContext.HospitalMaster.Where(x => x.Id != id && x.HospitalName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var hospital = _mapper.Map<GetHospitalDto>(data);
            return hospital;
        }
    }
}
