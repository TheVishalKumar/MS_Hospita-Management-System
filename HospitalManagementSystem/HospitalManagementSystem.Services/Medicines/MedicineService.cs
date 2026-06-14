using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.Models.Categories;
using HospitalManagementSystem.Models.DTO;
using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Medicines
{
    public class MedicineService : IMedicineRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;
        public MedicineService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<object> CreateAsync(CreateMedicineDto createMedicineDto)
        {
            try
            {
                var medicine = _mapper.Map<MedicineMaster>(createMedicineDto);
                medicine.Id = Guid.NewGuid();
                medicine.CreatedDate = DateTime.Now;
                _dbContext.MedicineMaster.Add(medicine);
               await _dbContext.SaveChangesAsync();
               return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {

                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message.Trim().ToString());
            }
        }

        public async Task<object> GetAsync()
        {
            
            var entryPoint = await _dbContext.MedicineMaster
            .Join(_dbContext.Categories,
                MM => MM.CategoryId,
                CM => CM.Id,
                (MM, CM) => new
                {
                    MM.Id,
                    MM.MedicineName,
                    MM.MedicineDescription,
                    MM.ExpiryDate,
                    MM.ManufactureDate,
                    MM.HSNCode,
                    MM.CreatedDate,
                    MM.CreatedBy,
                    MM.UpdateBy,
                    MM.UpdateDate,
                    MM.Quantity,
                    MM.Amount,
                    MM.SellerName,
                    MM.CompanyName,
                    Total = MM.Amount*MM.Quantity,
                    CM.CategoryName
                }).ToListAsync();

            //var medicine = _mapper.Map<List<GetMedicinesDto>>(entryPoint);
            return entryPoint;
        }

        public async Task<object> GetById(Guid id)
        {
            //var medicines = await _dbContext.MedicineMaster.FindAsync(id);

            var entryPoint = await _dbContext.MedicineMaster
            .Join(_dbContext.Categories,
                MM => MM.CategoryId,
                CM => CM.Id,
                (MM, CM) => new
                {
                    MM.Id,
                    MM.MedicineName,
                    MM.MedicineDescription,
                    MM.ExpiryDate,
                    MM.ManufactureDate,
                    MM.HSNCode,
                    MM.CreatedDate,
                    MM.CreatedBy,
                    MM.UpdateBy,
                    MM.UpdateDate,
                    CM.CategoryName,
                    MM.Quantity,
                    MM.Amount,

                }).Where(x=>x.Id== id).FirstOrDefaultAsync();

            return entryPoint;
        }

        public async Task<Response> UpdateAsync(Guid id, UpdateMedicineDto updateMedicineDto)
        {
            try
            {
                var existing = await _dbContext.MedicineMaster.FindAsync(id);
                if (existing == null) return new Response(Convert.ToInt32(ResponseCode.NotFound), "Not Found", null);

                existing.CategoryId = updateMedicineDto.CategoryId;
                existing.MedicineName = updateMedicineDto.MedicineName;
                existing.MedicineDescription = updateMedicineDto.MedicineDescription;
                existing.ExpiryDate = updateMedicineDto.ExpiryDate;
                existing.ManufactureDate = updateMedicineDto.ManufactureDate;
                existing.HSNCode = updateMedicineDto.HSNCode;
                existing.CompanyName = updateMedicineDto.CompanyName;
                existing.SellerName = updateMedicineDto.SellerName;
                existing.Quantity = updateMedicineDto.Quantity;
                existing.Amount = updateMedicineDto.Amount;
                existing.UpdateDate = DateTime.Now;

                _dbContext.MedicineMaster.Update(existing);
                await _dbContext.SaveChangesAsync();
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
            }
            catch (Exception ex)
            {
                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.Exception, ex.Message.Trim().ToString());
            }
        }

        public async Task<GetMedicinesDto> GetByName(string name)
        {
            var med = await _dbContext.MedicineMaster.Where(x => x.MedicineName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            if (med == null) return null;
            return _mapper.Map<GetMedicinesDto>(med);
        }

        public async Task<GetMedicinesDto> GetByNameHSNCode(string name, string hsnCode)
        {
            var med = await _dbContext.MedicineMaster.Where(x => x.MedicineName.ToUpper() == name.ToUpper() && x.HSNCode.ToUpper() == hsnCode.ToUpper()).FirstOrDefaultAsync();
            if (med == null) return null;
            return _mapper.Map<GetMedicinesDto>(med);
        }

        public async Task<object> CheckDuplicateForEdit(Guid id, string name)
        {
            var med = await _dbContext.MedicineMaster.Where(x => x.MedicineName.ToUpper() == name.ToUpper() && x.Id != id).FirstOrDefaultAsync();
            return med;
        }

        public async Task<object> GetExpiringMedicinesAsync(int daysFromNow)
        {
            var cutoff = DateTime.Now.AddDays(daysFromNow);

            var entryPoint = await _dbContext.MedicineMaster
                .Where(m => m.ExpiryDate != null && m.ExpiryDate <= cutoff)
                .Join(_dbContext.Categories,
                    MM => MM.CategoryId,
                    CM => CM.Id,
                    (MM, CM) => new
                    {
                        MM.Id,
                        MM.MedicineName,
                        MM.MedicineDescription,
                        MM.ExpiryDate,
                        MM.ManufactureDate,
                        MM.HSNCode,
                        MM.CreatedDate,
                        MM.CreatedBy,
                        MM.UpdateBy,
                        MM.UpdateDate,
                        MM.Quantity,
                        MM.Amount,
                        MM.SellerName,
                        MM.CompanyName,
                        DaysToExpiry = EF.Functions.DateDiffDay(DateTime.Now, MM.ExpiryDate),
                        CM.CategoryName,
                        IsExpired = MM.ExpiryDate < DateTime.Now
                    }).ToListAsync();

            return entryPoint;
        }
    }
}
