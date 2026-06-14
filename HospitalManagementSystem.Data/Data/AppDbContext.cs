using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Billings;
using HospitalManagementSystem.Models.Models.Categories;
using HospitalManagementSystem.Models.Models.Diseases;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Models.Models.HospitalBranches;
using HospitalManagementSystem.Models.Models.Hospitals;
using HospitalManagementSystem.Models.Models.Medicines;
using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Models.Models.Rooms;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Models.Models.Wards;
using HospitalManagementSystem.Models.Models.Roles;
using HospitalManagementSystem.Models.StoreProcedures.Appointments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Data.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<UserDetails>? UserMaster { get; set; }
        public DbSet<MedicineMaster>? MedicineMaster { get; set; }
        public DbSet<PatientDetails>? PatientMaster { get; set; }
        public DbSet<DiseaseMaster>? DiseaseMaster { get; set; }
        public DbSet<WardMaster>? WardMaster { get; set; }
        public DbSet<RoomMaster>? RoomMaster { get; set; }
        public DbSet<HospitalMaster>? HospitalMaster { get; set; }
        public DbSet<BranchMaster>? BranchMaster { get; set; }
        public DbSet<DoctorMaster>? DoctorMaster { get; set; }
        public DbSet<Appointment>? AppointmentMaster { get; set; }
        public DbSet<PatientPrescription>? PatientPrescriptionMaster { get; set; }
        public DbSet<PrescriptionBilling>? PrescriptionBillings { get; set; }
        public DbSet<GetTodayAppointmentList>? SP_GetTodayAppointmentList { get; set; }
        
        /// <summary>
        /// Role and permission management tables
        /// </summary>
        public DbSet<RolePermission>? RolePermissions { get; set; }
        public DbSet<UserRoleAssignment>? UserRoleAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GetTodayAppointmentList>().HasNoKey();

            //modelBuilder.Entity<PatientPrescription>()
            //    .HasOne(x => x.PrescriptionBilling)
            //    .WithMany(x => x.PatientPrescriptions)
            //    .HasForeignKey(x => x.AppointmentId).IsRequired();

            //modelBuilder.Entity<PrescriptionBilling>()
            //    .HasMany(x => x.PatientPrescriptions)
            //    .WithOne(x => x.PrescriptionBilling)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
