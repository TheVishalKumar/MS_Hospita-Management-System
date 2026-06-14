using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Prescriptions
{
    public class CreatePatientPrescriptionDto
    {
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
        public double? Price { get; set; }
    }
}
