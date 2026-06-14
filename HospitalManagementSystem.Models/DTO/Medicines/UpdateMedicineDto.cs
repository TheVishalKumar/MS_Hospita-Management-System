using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Medicines
{
    public class UpdateMedicineDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string? MedicineName { get; set; }
        public string? MedicineDescription { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public string? HSNCode { get; set; }
        public string? CompanyName { get; set; }
        public string? SellerName { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
