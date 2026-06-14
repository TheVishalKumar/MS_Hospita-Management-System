using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Appointments
{
    public class Appointment : CommonEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public Guid DoctorAssigned { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public Guid WardId { get; set; }
        public string? Disease { get; set; }
        public string? RoomNo { get; set; }
        public string? Prescription { get; set;}
        public Guid PrescriptionId { get; set; }
        public string? Status { get; set; }
        public Guid HospitalId { get; set; }
        public Guid BranchId { get; set; }
        public int Version { get; set; } = 0;
    }
}
