using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Billings
{
    public class PrescriptionBilling : CommonEntity
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
        public int Version { get; set; } = 0;
    }
}
