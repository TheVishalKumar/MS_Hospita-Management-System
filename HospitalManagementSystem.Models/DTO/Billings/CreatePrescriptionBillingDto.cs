using HospitalManagementSystem.Models.Models.Prescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Billings
{
    public  class CreatePrescriptionBillingDto
    {
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
        //public ICollection<PatientPrescription>? PatientPrescriptions { get; set; }
    }
}
