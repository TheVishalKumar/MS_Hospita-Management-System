using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Prescriptions;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Prescriptions
{
    public interface IPatientPrescription
    {
        Task<Response> CreateAsync(CreatePatientPrescriptionDto createPatientDto);
        //Task<Response> UpdatePatient(Guid id, UpdatePatientPrescriptionDto updatePatientDto);
        Task<GetPatientPrescriptionDto> Get(Guid id);
        Task<List<GetPatientPrescriptionDto>> GetByAppointmentId(Guid appointmentId);
        Task<List<GetPatientPrescriptionDto>> GetByPatientId(Guid patientId);
        Task<List<GetPatientPrescriptionDto>> GetList();
        Task<List<GetPatientPrescriptionDto>> GetPrescriptionsByHospitalAndBranch(Guid hospitalId, Guid branchId);
        //Task<List<GetPatientDto>> GetPatientPrescriptionByHospitalBranchList(Guid hospitalId, Guid branchId);
    }
}
