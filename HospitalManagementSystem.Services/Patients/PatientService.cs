using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Patients
{
    public class PatientService : IPatientRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public PatientService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response> CreateAsync(CreatePatientDto createPatientDto)
        {
            var userDetails = _mapper.Map<PatientDetails>(createPatientDto);
            userDetails.Id = Guid.NewGuid();
            userDetails.CreatedDate = DateTime.Now;
            await _dbContext.PatientMaster.AddAsync(userDetails);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetPatientDto> Get(Guid id)
        {
            var data = await _dbContext.PatientMaster.FindAsync(id);
            var userDetail = _mapper.Map<GetPatientDto>(data);
            return userDetail;
        }

        public async Task<GetPatientDto> GetByEmail(string emailId)
        {
            var data = await _dbContext.PatientMaster.Where(x => x.Email == emailId).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetPatientDto>(data);
            return userDetail;
        }

        public async Task<GetPatientDto> GetByMobile(string mobile)
        {
            var data = await _dbContext.PatientMaster.Where(x => x.MobileNo == mobile).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetPatientDto>(data);
            return userDetail;
        }

        public async Task<GetPatientDto> GetByName(string name)
        {
            var data = await _dbContext.PatientMaster.Where(x => x.FirstName == name).FirstOrDefaultAsync();
            var userDetail = _mapper.Map<GetPatientDto>(data);
            return userDetail;
        }

        public async Task<List<GetPatientDto>> GetList()
        {
            var data = await _dbContext.PatientMaster.OrderByDescending(x=>x.CreatedDate).ToListAsync();
            var userDetail = _mapper.Map<List<GetPatientDto>>(data);
            return userDetail;
        }

        public async Task<List<GetPatientDto>> GetpatientByHospitalBranchList(Guid hospitalId, Guid branchId)
        {
            var data = await _dbContext.PatientMaster.Where(x=>x.HospitalId==hospitalId && x.BranchId == branchId && x.IsActive==true).OrderByDescending(x=>x.CreatedDate).ToListAsync();
            var userDetail = _mapper.Map<List<GetPatientDto>>(data);
            return userDetail;
        }

        public async Task<Response> UpdatePatient(Guid id, UpdatePatientDto updatePatientDto)
        {
            var userDetails = _mapper.Map<PatientDetails>(updatePatientDto);
            var data = await  _dbContext.PatientMaster.FindAsync(id);

            data.FirstName = updatePatientDto.FirstName;
            data.LastName = updatePatientDto.LastName;
            data.MiddleName = updatePatientDto.MiddleName;
            data.MobileNo = updatePatientDto.MobileNo;
            data.FatherName = updatePatientDto.FatherName;
            data.DOB = updatePatientDto.DOB;
            data.Email = updatePatientDto.Email;
            data.ProfileImage = updatePatientDto.ProfileImage;
            data.Age = updatePatientDto.Age;
            data.Gender = updatePatientDto.Gender;
            data.UpdateDate = DateTime.Now;
            data.UpdateBy = userDetails.UpdateBy;

            _dbContext.PatientMaster.Update(data);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
        }
    }
}
