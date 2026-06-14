using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Doctors;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Services.Doctors
{
    public class DoctorService : IDoctorRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public DoctorService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response> CreateAsync(CreateDoctorDto createDoctorDto)
        {
            var doctorDetails = _mapper.Map<DoctorMaster>(createDoctorDto);
            doctorDetails.Id = Guid.NewGuid();
            doctorDetails.CreatedDate = DateTime.Now;
            await _dbContext.DoctorMaster.AddAsync(doctorDetails);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetDoctorDto> Get(Guid id)
        {
            var data = await _dbContext.DoctorMaster.FindAsync(id);
            var doctoerDetail = _mapper.Map<GetDoctorDto>(data);
            return doctoerDetail;
        }

        public async Task<GetDoctorDto> GetByEmail(string emailId)
        {
            var data = await _dbContext.DoctorMaster.Where(x => x.EmailId == emailId).FirstOrDefaultAsync();
            var doctoerDetail = _mapper.Map<GetDoctorDto>(data);
            return doctoerDetail;
        }

        public async Task<GetDoctorDto> GetByMobile(string mobile)
        {
            var data = await _dbContext.DoctorMaster.Where(x => x.MobileNo == mobile).FirstOrDefaultAsync();
            var doctoerDetail = _mapper.Map<GetDoctorDto>(data);
            return doctoerDetail;
        }

        public async Task<GetDoctorDto> GetByName(string name)
        {
            var data = await _dbContext.DoctorMaster.Where(x => x.FirstName == name).FirstOrDefaultAsync();
            var doctoerDetail = _mapper.Map<GetDoctorDto>(data);
            return doctoerDetail;
        }

        public async Task<List<GetDoctorDto>> GetList()
        {
            var data = await _dbContext.DoctorMaster.ToListAsync();
            var doctoerDetail = _mapper.Map<List<GetDoctorDto>>(data);
            return doctoerDetail;
        }

        public async Task<List<GetDoctorDto>> GetActiveDoctorList()
        {
            var data = await _dbContext.DoctorMaster.Where(x=>x.IsActive==true).ToListAsync();
            var doctoerDetail = _mapper.Map<List<GetDoctorDto>>(data);
            return doctoerDetail;
        }
        
        public async Task<Response> UpdateAsync(Guid id, UpdateDoctorDto updateDoctorDto)
        {
            var doctorDetails = _mapper.Map<DoctorMaster>(updateDoctorDto);
            var data = _dbContext.DoctorMaster.Find(id);

            data.FirstName = doctorDetails.FirstName;
            data.MiddleName = doctorDetails.MiddleName;
            data.LastName = doctorDetails.LastName;
            data.Age = doctorDetails.Age;
            data.MobileNo = doctorDetails.MobileNo;
            data.EmailId = doctorDetails.EmailId;
            data.DOB = doctorDetails.DOB;
            data.DOJ = doctorDetails.DOJ;
            data.ProfileImage = doctorDetails.ProfileImage;
            data.Gender = doctorDetails.Gender;
            data.IsActive = doctorDetails.IsActive;
            data.UpdateDate = DateTime.Now;
            data.UpdateBy = doctorDetails.UpdateBy;
            data.Address = doctorDetails.Address;
            _dbContext.DoctorMaster.Update(data);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
        }

        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _dbContext.DoctorMaster.Where(x => x.Id != id && x.MobileNo.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var doctor = _mapper.Map<GetDoctorDto>(data);
            return doctor;
        }

        public async Task<bool> UpdateStatus(Guid id, bool verified, Guid updateBy)
        {
            var data = _dbContext.DoctorMaster.Find(id);
            data.IsActive = verified;
            data.UpdateBy = updateBy;
            data.UpdateDate = DateTime.Now;
            _dbContext.DoctorMaster.Update(data);
             await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
