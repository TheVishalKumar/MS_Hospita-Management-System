namespace HospitalManagementSystem.Models.Constants
{
    /// <summary>
    /// Role constants for RBAC implementation
    /// All requests must include [Authorize(Roles = "...")] with these exact values
    /// </summary>
    public static class UserRoles
    {
        /// <summary>Admin - Full system access including all CRUD operations</summary>
        public const string Admin = "Admin";

        /// <summary>Doctor - View assigned patients, manage prescriptions, create appointments</summary>
        public const string Doctor = "Doctor";

        /// <summary>Receptionist - Patient CRUD, appointment management, emergency intake</summary>
        public const string Receptionist = "Receptionist";

        /// <summary>Billing Staff - Payment processing, invoice generation, insurance claims</summary>
        public const string BillingStaff = "BillingStaff";

        /// <summary>Pharmacy Staff - Medicine inventory, stock management, supplier orders</summary>
        public const string PharmacyStaff = "PharmacyStaff";

        /// <summary>Employee/General Staff - Limited view-only operations</summary>
        public const string Employee = "Employee";

        /// <summary>Lab Technician - Lab test ordering and result entry</summary>
        public const string LabTechnician = "LabTechnician";

        /// <summary>Radiology Technician - Radiology orders and image management</summary>
        public const string RadiologyTechnician = "RadiologyTechnician";

        /// <summary>Get all roles as array</summary>
        public static readonly string[] AllRoles = new[]
        {
            Admin,
            Doctor,
            Receptionist,
            BillingStaff,
            PharmacyStaff,
            Employee,
            LabTechnician,
            RadiologyTechnician
        };

        /// <summary>Medical staff roles (Doctor, LabTechnician, RadiologyTechnician)</summary>
        public static readonly string[] MedicalStaff = new[]
        {
            Doctor,
            LabTechnician,
            RadiologyTechnician
        };

        /// <summary>Administrative staff roles (Admin, Receptionist, BillingStaff, PharmacyStaff)</summary>
        public static readonly string[] AdministrativeStaff = new[]
        {
            Admin,
            Receptionist,
            BillingStaff,
            PharmacyStaff
        };
    }

    /// <summary>
    /// Permission constants for granular access control
    /// Maps to specific operations like Create, Read, Update, Delete
    /// </summary>
    public static class UserPermissions
    {
        // Patient Permissions
        public const string PatientCreate = "patient.create";
        public const string PatientRead = "patient.read";
        public const string PatientUpdate = "patient.update";
        public const string PatientDelete = "patient.delete";

        // Doctor Permissions
        public const string DoctorCreate = "doctor.create";
        public const string DoctorRead = "doctor.read";
        public const string DoctorUpdate = "doctor.update";
        public const string DoctorDelete = "doctor.delete";

        // Appointment Permissions
        public const string AppointmentCreate = "appointment.create";
        public const string AppointmentRead = "appointment.read";
        public const string AppointmentUpdate = "appointment.update";
        public const string AppointmentDelete = "appointment.delete";

        // Prescription Permissions
        public const string PrescriptionCreate = "prescription.create";
        public const string PrescriptionRead = "prescription.read";
        public const string PrescriptionUpdate = "prescription.update";
        public const string PrescriptionDelete = "prescription.delete";

        // Billing Permissions
        public const string BillingCreate = "billing.create";
        public const string BillingRead = "billing.read";
        public const string BillingUpdate = "billing.update";
        public const string BillingDelete = "billing.delete";

        // Medicine Permissions
        public const string MedicineCreate = "medicine.create";
        public const string MedicineRead = "medicine.read";
        public const string MedicineUpdate = "medicine.update";
        public const string MedicineDelete = "medicine.delete";

        // Report Permissions
        public const string ReportView = "report.view";
        public const string ReportGenerate = "report.generate";
        public const string ReportExport = "report.export";

        // Admin Permissions
        public const string AdminFullAccess = "admin.full";
        public const string UserManage = "user.manage";
        public const string RoleManage = "role.manage";
    }
}
