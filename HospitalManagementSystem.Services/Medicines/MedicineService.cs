#nullable disable
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
            _dbContext = dbContext!;
            _mapper = mapper!;
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
                    MM.SellerName,
                    MM.CompanyName,
                    categoryId = CM.Id,

                }).Where(x=>x.Id==id).FirstOrDefaultAsync();
            //var medicine = _mapper.Map<GetMedicinesDto>(medicines);
            return entryPoint;
        }

        public async Task<GetMedicinesDto> GetByName(string name)
        {
            var medicines = await _dbContext.MedicineMaster
                .Where(x => x.MedicineName.ToUpper() == name.ToUpper())
                .FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetMedicinesDto>(medicines);
            return medicine;
        }

        public async Task<GetMedicinesDto> GetByNameHSNCode(string name, string hsnCode)
        {
            var medicines = await _dbContext.MedicineMaster
                                .Where(x => x.MedicineName.ToUpper() == name.ToUpper() && x.HSNCode
                                .ToUpper()==hsnCode.ToUpper()).FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetMedicinesDto>(medicines);
            return medicine;
        }

        public async Task<object> GetExpiryMedicines(int days)
        {
            var cutoff = DateTime.Now.AddDays(days);

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

        public async Task<Response> UpdateAsync(Guid id, UpdateMedicineDto updateMedicineDto)
        {
            try
            {
                var result =  await _dbContext.MedicineMaster.FindAsync(id);
                var medicine = _mapper.Map<MedicineMaster>(updateMedicineDto);
                
                result.UpdateDate = DateTime.Now;
                result.MedicineName = medicine.MedicineName;
                result.MedicineDescription = medicine.MedicineDescription;
                result.ExpiryDate = medicine.ExpiryDate;
                result.ManufactureDate = medicine.ManufactureDate;
                result.CompanyName = medicine.CompanyName;
                result.Quantity= medicine.Quantity;
                result.Amount= medicine.Amount;
                result.HSNCode = medicine.HSNCode;
                result.CategoryId=medicine.CategoryId;

                _dbContext.MedicineMaster.Update(result);
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
            var data = await _dbContext.MedicineMaster.Where(x => x.Id != id && x.MedicineName.ToUpper() == name.ToUpper()).FirstOrDefaultAsync();
            var medicine = _mapper.Map<GetMedicinesDto>(data);
            return medicine;
        }
    }
}
