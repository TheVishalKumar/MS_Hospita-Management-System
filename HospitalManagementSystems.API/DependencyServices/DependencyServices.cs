using HospitalManagementSystem.Services.Categories;
using HospitalManagementSystem.Services.Users;
using HospitalManagementSystem.Services.Medicines;
using HospitalManagementSystem.Services.Patients;
using HospitalManagementSystem.Services.Diseases;
using HospitalManagementSystem.Services.Wards;
using HospitalManagementSystem.Services;
using HospitalManagementSystem.Services.Rooms;
using HospitalManagementSystem.Services.Hospitals;
using HospitalManagementSystem.Services.HospitalBranches;
using HospitalManagementSystem.Services.Doctors;
using HospitalManagementSystem.Services.Appointments;
using HospitalManagementSystem.Services.Accounts;
using HospitalManagementSystem.Services.Prescriptions;
using HospitalManagementSystem.Services.Billings;
using HospitalManagementSystem.Services.Dashboards;
using HospitalManagementSystem.Services.Security;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystems.API
{
    public static class DependencyServices
    {
        public static IServiceCollection AddHospitalManagementServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Domain Services
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
            services.AddTransient<IDoctorLoginRepository, DoctorLoginService>();
            services.AddTransient<IAppointmentRepository, AppointmentService>();
            services.AddTransient<ILoginRepository, LoginService>();
            services.AddTransient<IPatientPrescription, PatientPrescriptionService>();
            services.AddTransient<IPrescriptionBillingRepository, PrescriptionBillingService>();
            services.AddTransient<IDashboardData, DashboardService>();

            // Security & Authentication Services
            services.AddSingleton<IPasswordHashingService, PasswordHashingService>();
            services.AddSingleton<IEncryptionService>(provider => 
            {
                var encryptionKey = configuration["Encryption:Key"] ?? "DefaultEncryptionKey";
                return new EncryptionService(encryptionKey);
            });
            services.AddScoped<IMfaService, MfaService>();
            services.AddScoped<IRoleAssignmentService, RoleAssignmentService>();

            return services;
        }
    }
}