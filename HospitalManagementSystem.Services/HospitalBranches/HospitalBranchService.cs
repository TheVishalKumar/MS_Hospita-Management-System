using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.HospitalBranches;
using HospitalManagementSystem.Models.DTO.Hospitals;
using HospitalManagementSystem.Models.Models.HospitalBranches;
using HospitalManagementSystem.Models.Models.Hospitals;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.HospitalBranches
{
    public class HospitalBranchService : IHospitalBranchRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public HospitalBranchService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response> CreateAsync(CreateHospitalBranchDto createHospitalBranchDto)
        {
            var branchMaster = _mapper.Map<BranchMaster>(createHospitalBranchDto);
            branchMaster.Id = Guid.NewGuid();
            branchMaster.CreatedDate = DateTime.Now;
            await _dbContext.BranchMaster.AddAsync(branchMaster);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<object> Get(Guid id)
        {
            var data = await _dbContext.BranchMaster.Join(_dbContext.HospitalMaster,
                BM => BM.HospitalId,
                HM => HM.Id,
                (BM, HM) => new
                {
                    BM.Id,
                    BM.HospitalId,
                    HM.HospitalName,
                    BM.BranchName,
                    BM.Address,
                    BM.ContactNo,
                    BM.EmailId,
                    BM.FaxNo,
                    BM.GSTNo,
                    BM.IsActive,
                    BM.CreatedDate,
                    BM.CreatedBy,
                    BM.UpdateBy,
                    BM.UpdateDate,
                    BM.Description
                })
                .Where(x=>x.Id==id).FirstOrDefaultAsync();
            //var getHospitalBranch = _mapper.Map<GetHospitalBranchDto>(data);
            return data;
        }

        public async Task<object> GetByEmail(string emailId)
        {
            var data = await _dbContext.BranchMaster.Join(_dbContext.HospitalMaster,
                BM => BM.HospitalId,
                HM => HM.Id,
                (BM, HM) => new
                {
                    BM.Id,
                    BM.HospitalId,
                    HM.HospitalName,
                    BM.BranchName,
                    BM.Address,
                    BM.ContactNo,
                    BM.EmailId,
                    BM.FaxNo,
                    BM.GSTNo,
                    BM.IsActive,
                    BM.CreatedDate,
                    BM.CreatedBy,
                    BM.UpdateBy,
                    BM.UpdateDate,
                    BM.Description
                })
                .Where(x => x.EmailId == emailId).FirstOrDefaultAsync();
            //var getHospitalBranch = _mapper.Map<GetHospitalBranchDto>(data);
            return data;
        }

        public async Task<object> GetByMobile(string mobile)
        {
            var data = await _dbContext.BranchMaster.Join(_dbContext.HospitalMaster,
                BM => BM.HospitalId,
                HM => HM.Id,
                (BM, HM) => new
                {
                    BM.Id,
                    BM.HospitalId,
                    HM.HospitalName,
                    BM.BranchName,
                    BM.Address,
                    BM.ContactNo,
                    BM.EmailId,
                    BM.FaxNo,
                    BM.GSTNo,
                    BM.IsActive,
                    BM.CreatedDate,
                    BM.CreatedBy,
                    BM.UpdateBy,
                    BM.UpdateDate,
                    BM.Description
                })
                .Where(x => x.ContactNo == mobile).FirstOrDefaultAsync();
            //var getHospitalBranch = _mapper.Map<GetHospitalBranchDto>(data);
            return data;
        }

        public async Task<object> GetByName(string name)
        {
            var data = await _dbContext.BranchMaster
                .Join(_dbContext.HospitalMaster,
                BM => BM.HospitalId,
                HM => HM.Id,
                (BM, HM) => new
                {
                    BM.Id,
                    BM.HospitalId,
                    HM.HospitalName,
                    BM.BranchName,
                    BM.Address,
                    BM.ContactNo,
                    BM.EmailId,
                    BM.FaxNo,
                    BM.GSTNo,
                    BM.IsActive,
                    BM.CreatedDate,
                    BM.CreatedBy,
                    BM.UpdateBy,
                    BM.UpdateDate,
                    BM.Description
                })
                .Where(x => x.BranchName == name).FirstOrDefaultAsync();
            //var getHospitalBranch = _mapper.Map<GetHospitalBranchDto>(data);
            return data;
        }

        public async Task<object> GetList()
        {
            var data = await _dbContext.BranchMaster.Join(_dbContext.HospitalMaster,
                BM=>BM.HospitalId,
                HM=>HM.Id,
                (BM, HM) => new 
                {
                    BM.Id,
                    BM.HospitalId,
                    HM.HospitalName,
                    BM.BranchName,
                    BM.Address,
                    BM.ContactNo,
                    BM.EmailId,
                    BM.FaxNo,
                    BM.GSTNo,
                    BM.IsActive,
                    BM.CreatedDate,
                    BM.CreatedBy,
                    BM.UpdateBy,
                    BM.UpdateDate,
                    BM.Description

                }
                ).ToListAsync();
            //var getHospitalBranch = _mapper.Map<List<GetHospitalBranchDto>>(data);
            return data;
        }

        public async Task<Response> UpdateAsync(Guid id, UpdateHospitalBranchDto updateHospitalBranchDto)
        {
            var branchDetails = _mapper.Map<BranchMaster>(updateHospitalBranchDto);
            var data = await _dbContext.BranchMaster.FindAsync(id);

            data.HospitalId = branchDetails.HospitalId;
            data.BranchName = branchDetails.BranchName;
            data.Address = branchDetails.Address;
            data.ContactNo = branchDetails.ContactNo;
            data.EmailId = branchDetails.EmailId;
            data.GSTNo = branchDetails.GSTNo;
            data.FaxNo = branchDetails.FaxNo;
            data.Description = branchDetails.Description;
            data.IsActive = branchDetails.IsActive;
            data.UpdateDate = branchDetails.UpdateDate;
            data.UpdateBy = branchDetails.UpdateBy;

            _dbContext.BranchMaster.Update(data);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
        }

        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _dbContext.BranchMaster.Where(x => x.Id != id && x.BranchName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var hospital = _mapper.Map<GetHospitalBranchDto>(data);
            return hospital;
        }
    }
}
