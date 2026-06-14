using HospitalManagementSystem.Models.DTO.Medicines;
using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Medicines
{
    public interface IMedicineRepository
    {
        Task<object> CreateAsync(CreateMedicineDto createMedicineDto);
        Task<Response> UpdateAsync(Guid id, UpdateMedicineDto updateMedicineDto);
        Task<object> GetById(Guid id);
        Task<object> GetAsync();
        Task<GetMedicinesDto> GetByName(string name);
        Task<GetMedicinesDto> GetByNameHSNCode(string name, string hsnCode);
        Task<object> CheckDuplicateForEdit(Guid id, string name);

        // New method to get medicines expiring within specified days
        Task<object> GetExpiringMedicinesAsync(int daysFromNow);
    }
}
