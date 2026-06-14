using HospitalManagementSystem.Services.Categories;
using HospitalManagementSystem.Services.Users;
using HospitalManagementSystem.Services.Medicines;
using HospitalManagementSystem.Services.Patients;
using HospitalManagementSystem.Services.Diseases;
using HospitalManagementSystem.Services.Wards;
using HospitalManagementSystem.Services.Rooms;
using HospitalManagementSystem.Services.Hospitals;
using HospitalManagementSystem.Services.HospitalBranches;
using HospitalManagementSystem.Services.Doctors;
using HospitalManagementSystem.Services.Appointments;
using HospitalManagementSystem.Services.Accounts;
using HospitalManagementSystem.Services.Prescriptions;
using HospitalManagementSystem.Services.Billings;
using HospitalManagementSystem.Services.Dashboards;
using HospitalManagementSystem.Services.Roles;
using HospitalManagementSystem.Services;

namespace HospitalManagementSystems.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHospitalManagementDependencies(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryService>();
            services.AddTransient<IUserRepository, UserDetailService>();
            services.AddTransient<IMedicineRepository, MedicineService>();
            services.AddTransient<IPatientRepository, PatientService>();
            services.AddTransient<IDiseaseRepository, DiseaseService>();
            services.AddTransient<IWardRepository, WardService>();
            services.AddTransient<IRoomRepository, RoomService>();
            services.AddTransient<IHospitalRepository, HospitalService>();
            services.AddTransient<IHospitalBranchRepository, HospitalBranchService>();
            services.AddTransient<IDoctorRepository, DoctorService>();
            services.AddTransient<IAppointmentRepository, AppointmentService>();
            services.AddTransient<ILoginRepository, LoginService>();
            services.AddTransient<IPatientPrescription, PatientPrescriptionService>();
            services.AddTransient<IPrescriptionBillingRepository, PrescriptionBillingService>();
            services.AddTransient<IDashboardData, DashboardService>();
            services.AddTransient<IRolePermissionService, RolePermissionService>();
            return services;
        }
    }
}
using HospitalManagementSystems.API; // Add this at the top

// ... other code ...

// Replace all individual AddTransient registrations with the following line:
builder.Services.AddHospitalManagementDependencies();

// ... other code ...
