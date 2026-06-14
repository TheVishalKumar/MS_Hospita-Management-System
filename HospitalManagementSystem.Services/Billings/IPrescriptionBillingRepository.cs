using HospitalManagementSystem.Models.DTO.Billings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Billings
{
    public interface IPrescriptionBillingRepository
    {
        Task<object> CreateBilling(CreatePrescriptionBillingDto prescriptionBillingDto);
        Task<object> GetPrescriptionBillingByAppointmemtId(Guid billingId, Guid AppointmentId);
        Task<object> GetBillingByAppointmemtId(Guid appointmentId);
        Task<List<GetPrescriptionBillingDto>> GetBillingsByPatientId(Guid patientId);
        Task<List<GetPrescriptionBillingDto>> GetBillingsByHospitalAndBranch(Guid hospitalId, Guid branchId);
        Task<List<GetPrescriptionBillingDto>> GetPendingBillings(Guid hospitalId, Guid branchId);
    }
}
