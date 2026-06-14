using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Prescriptions
{
    public class GetPatientPrescriptionDto : CommonEntity
    {
        public Guid Id { get; set; }
        public Guid BillingId { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public Guid? MedicineId { get; set; }
        public string? Quantity { get; set; }
        public string? Root { get; set; }
        public string? Times { get; set; }
        public int PrescriptionEnd { get; set; }
        public string? PrescriptionEndAs { get; set; }
        public string? DoctorSignature { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
        public double? Price { get; set; }
        
        // Patient Information
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientMobileNo { get; set; }
        public int? PatientAge { get; set; }
        public string? PatientGender { get; set; }
        
        // Medicine Information
        public string? MedicineName { get; set; }
        public string? MedicineStrength { get; set; }
        
        // Doctor Information (Doctor who issued the prescription)
        public Guid? DoctorId { get; set; }
        public string? DoctorFirstName { get; set; }
        public string? DoctorLastName { get; set; }
    }
}
