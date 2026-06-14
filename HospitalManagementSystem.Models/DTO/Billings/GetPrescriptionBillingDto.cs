using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Billings
{
    public class GetPrescriptionBillingDto : CommonEntity
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public double Amount { get; set; }
        public bool IsDiscount { get; set; }
        public double Discount { get; set; }
        public bool IsGST { get; set; }
        public double GstAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
        public string? PaymentType { get; set; }
        public ICollection<PatientPrescription>? PatientPrescriptionDetails { get; set; }
        
        // Patient Information
        public Guid? PatientId { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientMobileNo { get; set; }
        
        // Appointment Information
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? Disease { get; set; }
        
        // Doctor Information
        public Guid? DoctorId { get; set; }
        public string? DoctorFirstName { get; set; }
        public string? DoctorLastName { get; set; }
        
        // Billing Status
        public string? BillingStatus { get; set; } // Pending, Paid, Partial
        public DateTime? PaymentDate { get; set; }
        public string? RemarksOrNotes { get; set; }
        
        // Hospital/Branch Information
        public Guid? HospitalId { get; set; }
        public Guid? BranchId { get; set; }
    }
}
