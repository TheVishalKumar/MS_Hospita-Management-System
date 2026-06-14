using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Wards;
using HospitalManagementSystem.Models.Models.Wards;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Wards
{
    public class WardService : IWardRepository
    {
        private readonly AppDbContext _appDbContext;
        private IMapper _mapper;
        public WardService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;

        }
        public async Task<Response> CreateAsyn(CreateWardDto createWardDto)
        {
            try
            {
                var Ward = _mapper.Map<WardMaster>(createWardDto);
                Ward.Id = Guid.NewGuid();
                Ward.CreatedDate = DateTime.Now;
                await _appDbContext.AddAsync(Ward);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message);
            }
        }

        public async Task<GetWardDto> GetById(Guid id)
        {
            var data = await _appDbContext.WardMaster.Where(x => x.Id == id).FirstOrDefaultAsync();
            var Ward = _mapper.Map<GetWardDto>(data);
            return Ward;
        }

        public async Task<GetWardDto> GetByName(string name)
        {
            var data = await _appDbContext.WardMaster.Where(x => x.WardName == name).FirstOrDefaultAsync();
            var Ward = _mapper.Map<GetWardDto>(data);
            return Ward;
        }

        public async Task<List<GetWardDto>> GetListAsync()
        {
            var data = await _appDbContext.WardMaster.ToListAsync();
            var Ward = _mapper.Map<List<GetWardDto>>(data);
            return Ward;
        }


        public async Task<List<GetWardDto>> GetActiveListAsync()
        {
            var data = await _appDbContext.WardMaster.Where(x=>x.IsActive==true).ToListAsync();
            var Ward = _mapper.Map<List<GetWardDto>>(data);
            return Ward;
        }

        public async Task<Response> UpdateAsyn(Guid id, UpdateWardDto updateWardDto)
        {
            try
            {
                var data = await _appDbContext.WardMaster.FindAsync(id);
                var Ward = _mapper.Map<WardMaster>(updateWardDto);
                data.WardName = Ward.WardName;
                data.WardDescription = Ward.WardDescription;
                data.UpdateDate = DateTime.Now;
                data.UpdateBy = Ward.UpdateBy;
                data.IsActive = Ward.IsActive;

                 _appDbContext.Update(data);
                await _appDbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.UpdateMessage, CommonMessage.UpdateMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message);
            }
        }

        public async Task<bool> UpdateStatus(Guid id, bool status, Guid updateBy)
        {
            var data = await _appDbContext.WardMaster.FindAsync(id);
            data.IsActive = status;
            data.UpdateBy=updateBy;
            data.UpdateDate = DateTime.Now;
             _appDbContext.WardMaster.Update(data);
            await _appDbContext.SaveChangesAsync();

            return true;
        }


        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var data = await _appDbContext.WardMaster.Where(x => x.Id != id && x.WardName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetWardDto>(data);
            return medicine;
        }
    }
}
