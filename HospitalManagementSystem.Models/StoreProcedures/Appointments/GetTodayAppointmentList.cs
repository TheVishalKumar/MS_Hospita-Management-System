using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.StoreProcedures.Appointments
{
    [NotMapped]
    public class GetTodayAppointmentList
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public Guid WardId { get; set; }
        public Guid DoctorAssigned { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
        public string? DoctorName { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string? Disease { get; set; }
        public string? WardName { get; set; }
        public string? RoomNo { get; set; }
        public string? Prescription { get; set; }
        public Guid PrescriptionId { get; set; }
        public string? Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? PatientMobileNo { get; set; }
    }
}
