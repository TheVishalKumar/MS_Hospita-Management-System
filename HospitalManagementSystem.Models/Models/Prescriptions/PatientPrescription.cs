using HospitalManagementSystem.Models.Models.Billings;
using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Prescriptions
{
    public class PatientPrescription : CommonEntity
    {
        public Guid Id { get; set; }
        public Guid BillingId { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public Guid? MedicineId { get; set; }
        public int Quantity { get; set; }
        public string? Root { get; set; }
        public string? Times { get; set; }
        public double? Price { get; set; }
        public int PrescriptionEnd { get; set; }
        public string? PrescriptionEndAs { get; set; }
        public string? DoctorSignature { get; set; }
        public PrescriptionBilling? PrescriptionBilling { get; set; }
        public List<MedicineMaster>? MedicineDetails { get; set; }
        public int Version { get; set; } = 0;
    }
}
