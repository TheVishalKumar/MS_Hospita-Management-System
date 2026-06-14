using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Appointments
{
    public class GetAppointmentDto : CommonEntity
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        
        // Patient Details
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public string? PatientMiddleName { get; set; }
        public string? PatientEmail { get; set; }
        public string? PatientMobileNo { get; set; }
        public int? PatientAge { get; set; }
        public string? PatientGender { get; set; }
        public string? PatientAddress { get; set; }
        
        // Doctor Details
        public Guid DoctorAssigned { get; set; }
        public string? DoctorFirstName { get; set; }
        public string? DoctorLastName { get; set; }
        public string? DoctorMiddleName { get; set; }
        public string? DoctorEmail { get; set; }
        public string? DoctorMobileNo { get; set; }
        
        // Appointment Details
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public Guid WardId { get; set; }
        
        // Ward Details
        public string? WardName { get; set; }
        public string? WardDescription { get; set; }
        
        public string? Disease { get; set; }
        public string? RoomNo { get; set; }
        public string? Prescription { get; set; }
        public Guid PrescriptionId { get; set; }
        public string? Status { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
    }
}
