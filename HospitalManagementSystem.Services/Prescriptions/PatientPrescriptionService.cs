#nullable disable
using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Patients;
using HospitalManagementSystem.Models.DTO.Prescriptions;
using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Models.Models.Patients;
using HospitalManagementSystem.Models.Models.Prescriptions;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Prescriptions
{
    public class PatientPrescriptionService : IPatientPrescription
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public PatientPrescriptionService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext!;
            _mapper = mapper!;
        }

        public async Task<Response> CreateAsync(CreatePatientPrescriptionDto createPatientPrescriptionDto)
        {
            var userDetails = _mapper.Map<PatientPrescription>(createPatientPrescriptionDto);
            userDetails.Id = Guid.NewGuid();
            userDetails.CreatedDate = DateTime.Now;
            await _dbContext.PatientPrescriptionMaster.AddAsync(userDetails);
            await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetPatientPrescriptionDto> Get(Guid id)
        {
            // Fetch prescription with patient and appointment details
            var prescriptionData = await _dbContext.PatientPrescriptionMaster
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Join(_dbContext.PatientMaster,
                    prescription => prescription.PatientId,
                    patient => patient.Id,
                    (prescription, patient) => new { prescription, patient })
                .Join(_dbContext.AppointmentMaster,
                    joined => joined.prescription.AppointmentId,
                    appointment => appointment.Id,
                    (joined, appointment) => new { joined.prescription, joined.patient, appointment })
                .FirstOrDefaultAsync();

            if (prescriptionData == null)
                return null;

            // Fetch doctor separately
            var doctor = prescriptionData.appointment.DoctorAssigned != Guid.Empty && prescriptionData.appointment.DoctorAssigned != null
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .FirstOrDefaultAsync(d => d.Id == prescriptionData.appointment.DoctorAssigned)
                : null;

            return new GetPatientPrescriptionDto
            {
                Id = prescriptionData.prescription.Id,
                BillingId = prescriptionData.prescription.BillingId,
                AppointmentId = prescriptionData.prescription.AppointmentId,
                PatientId = prescriptionData.prescription.PatientId,
                MedicineId = prescriptionData.prescription.MedicineId,
                Quantity = prescriptionData.prescription.Quantity.ToString(),
                Root = prescriptionData.prescription.Root,
                Times = prescriptionData.prescription.Times,
                PrescriptionEnd = prescriptionData.prescription.PrescriptionEnd,
                PrescriptionEndAs = prescriptionData.prescription.PrescriptionEndAs,
                DoctorSignature = prescriptionData.prescription.DoctorSignature,
                HospitalId = prescriptionData.appointment.HospitalId,
                BranchId = prescriptionData.appointment.BranchId,
                Price = prescriptionData.prescription.Price,
                PatientFirstName = prescriptionData.patient.FirstName,
                PatientLastName = prescriptionData.patient.LastName,
                PatientEmail = prescriptionData.patient.Email,
                PatientMobileNo = prescriptionData.patient.MobileNo,
                PatientAge = prescriptionData.patient.Age,
                PatientGender = prescriptionData.patient.Gender,
                DoctorId = doctor != null ? doctor.Id : null,
                DoctorFirstName = doctor != null ? doctor.FirstName : null,
                DoctorLastName = doctor != null ? doctor.LastName : null,
                CreatedDate = prescriptionData.prescription.CreatedDate,
                CreatedBy = prescriptionData.prescription.CreatedBy,
                UpdateDate = prescriptionData.prescription.UpdateDate,
                UpdateBy = prescriptionData.prescription.UpdateBy
            };
        }

        public async Task<List<GetPatientPrescriptionDto>> GetByAppointmentId(Guid appointmentId)
        {
            // Fetch prescriptions with patient and appointment details
            var prescriptionData = await _dbContext.PatientPrescriptionMaster
                .AsNoTracking()
                .Where(p => p.AppointmentId == appointmentId)
                .Join(_dbContext.PatientMaster,
                    prescription => prescription.PatientId,
                    patient => patient.Id,
                    (prescription, patient) => new { prescription, patient })
                .Join(_dbContext.AppointmentMaster,
                    joined => joined.prescription.AppointmentId,
                    appointment => appointment.Id,
                    (joined, appointment) => new { joined.prescription, joined.patient, appointment })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = prescriptionData
                .Select(x => x.appointment.DoctorAssigned)
                .Where(d => d != null && d != Guid.Empty)
                .Distinct()
                .ToList();

            // Fetch doctors
            var doctors = doctorIds.Count > 0
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .Where(d => doctorIds.Contains(d.Id))
                    .ToListAsync()
                : new List<DoctorMaster>();

            // Map to DTO with doctor information
            return prescriptionData.Select(x =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetPatientPrescriptionDto
                {
                    Id = x.prescription.Id,
                    BillingId = x.prescription.BillingId,
                    AppointmentId = x.prescription.AppointmentId,
                    PatientId = x.prescription.PatientId,
                    MedicineId = x.prescription.MedicineId,
                    Quantity = x.prescription.Quantity.ToString(),
                    Root = x.prescription.Root,
                    Times = x.prescription.Times,
                    PrescriptionEnd = x.prescription.PrescriptionEnd,
                    PrescriptionEndAs = x.prescription.PrescriptionEndAs,
                    DoctorSignature = x.prescription.DoctorSignature,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    Price = x.prescription.Price,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    DoctorId = doctor != null ? doctor.Id : null,
                    DoctorFirstName = doctor != null ? doctor.FirstName : null,
                    DoctorLastName = doctor != null ? doctor.LastName : null,
                    CreatedDate = x.prescription.CreatedDate,
                    CreatedBy = x.prescription.CreatedBy,
                    UpdateDate = x.prescription.UpdateDate,
                    UpdateBy = x.prescription.UpdateBy
                };
            }).ToList();
        }

        public async Task<List<GetPatientPrescriptionDto>> GetByPatientId(Guid patientId)
        {
            // Fetch prescriptions with patient and appointment details
            var prescriptionData = await _dbContext.PatientPrescriptionMaster
                .AsNoTracking()
                .Where(p => p.PatientId == patientId)
                .Join(_dbContext.PatientMaster,
                    prescription => prescription.PatientId,
                    patient => patient.Id,
                    (prescription, patient) => new { prescription, patient })
                .Join(_dbContext.AppointmentMaster,
                    joined => joined.prescription.AppointmentId,
                    appointment => appointment.Id,
                    (joined, appointment) => new { joined.prescription, joined.patient, appointment })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = prescriptionData
                .Select(x => x.appointment.DoctorAssigned)
                .Where(d => d != null && d != Guid.Empty)
                .Distinct()
                .ToList();

            // Fetch doctors
            var doctors = doctorIds.Count > 0
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .Where(d => doctorIds.Contains(d.Id))
                    .ToListAsync()
                : new List<DoctorMaster>();

            // Map to DTO with doctor information
            return prescriptionData.Select(x =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetPatientPrescriptionDto
                {
                    Id = x.prescription.Id,
                    BillingId = x.prescription.BillingId,
                    AppointmentId = x.prescription.AppointmentId,
                    PatientId = x.prescription.PatientId,
                    MedicineId = x.prescription.MedicineId,
                    Quantity = x.prescription.Quantity.ToString(),
                    Root = x.prescription.Root,
                    Times = x.prescription.Times,
                    PrescriptionEnd = x.prescription.PrescriptionEnd,
                    PrescriptionEndAs = x.prescription.PrescriptionEndAs,
                    DoctorSignature = x.prescription.DoctorSignature,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    Price = x.prescription.Price,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    DoctorId = doctor != null ? doctor.Id : null,
                    DoctorFirstName = doctor != null ? doctor.FirstName : null,
                    DoctorLastName = doctor != null ? doctor.LastName : null,
                    CreatedDate = x.prescription.CreatedDate,
                    CreatedBy = x.prescription.CreatedBy,
                    UpdateDate = x.prescription.UpdateDate,
                    UpdateBy = x.prescription.UpdateBy
                };
            }).ToList();
        }

        public async Task<List<GetPatientPrescriptionDto>> GetList()
        {
            // Fetch prescriptions with patient and appointment details
            var prescriptionData = await _dbContext.PatientPrescriptionMaster
                .AsNoTracking()
                .Join(_dbContext.PatientMaster,
                    prescription => prescription.PatientId,
                    patient => patient.Id,
                    (prescription, patient) => new { prescription, patient })
                .Join(_dbContext.AppointmentMaster,
                    joined => joined.prescription.AppointmentId,
                    appointment => appointment.Id,
                    (joined, appointment) => new { joined.prescription, joined.patient, appointment })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = prescriptionData
                .Select(x => x.appointment.DoctorAssigned)
                .Where(d => d != null && d != Guid.Empty)
                .Distinct()
                .ToList();

            // Fetch doctors
            var doctors = doctorIds.Count > 0
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .Where(d => doctorIds.Contains(d.Id))
                    .ToListAsync()
                : new List<DoctorMaster>();

            // Map to DTO with doctor information
            return prescriptionData.Select(x =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetPatientPrescriptionDto
                {
                    Id = x.prescription.Id,
                    BillingId = x.prescription.BillingId,
                    AppointmentId = x.prescription.AppointmentId,
                    PatientId = x.prescription.PatientId,
                    MedicineId = x.prescription.MedicineId,
                    Quantity = x.prescription.Quantity.ToString(),
                    Root = x.prescription.Root,
                    Times = x.prescription.Times,
                    PrescriptionEnd = x.prescription.PrescriptionEnd,
                    PrescriptionEndAs = x.prescription.PrescriptionEndAs,
                    DoctorSignature = x.prescription.DoctorSignature,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    Price = x.prescription.Price,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    DoctorId = doctor != null ? doctor.Id : null,
                    DoctorFirstName = doctor != null ? doctor.FirstName : null,
                    DoctorLastName = doctor != null ? doctor.LastName : null,
                    CreatedDate = x.prescription.CreatedDate,
                    CreatedBy = x.prescription.CreatedBy,
                    UpdateDate = x.prescription.UpdateDate,
                    UpdateBy = x.prescription.UpdateBy
                };
            }).ToList();
        }

        public async Task<List<GetPatientPrescriptionDto>> GetPrescriptionsByHospitalAndBranch(Guid hospitalId, Guid branchId)
        {
            // Fetch prescriptions with patient and appointment details
            var prescriptionData = await _dbContext.PatientPrescriptionMaster
                .AsNoTracking()
                .Join(_dbContext.AppointmentMaster,
                    prescription => prescription.AppointmentId,
                    appointment => appointment.Id,
                    (prescription, appointment) => new { prescription, appointment })
                .Where(x => x.appointment.HospitalId == hospitalId && x.appointment.BranchId == branchId)
                .Join(_dbContext.PatientMaster,
                    joined => joined.prescription.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.prescription, joined.appointment, patient })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = prescriptionData
                .Select(x => x.appointment.DoctorAssigned)
                .Where(d => d != null && d != Guid.Empty)
                .Distinct()
                .ToList();

            // Fetch doctors
            var doctors = doctorIds.Count > 0
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .Where(d => doctorIds.Contains(d.Id))
                    .ToListAsync()
                : new List<DoctorMaster>();

            // Map to DTO with doctor information
            return prescriptionData.Select(x =>
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetPatientPrescriptionDto
                {
                    Id = x.prescription.Id,
                    BillingId = x.prescription.BillingId,
                    AppointmentId = x.prescription.AppointmentId,
                    PatientId = x.prescription.PatientId,
                    MedicineId = x.prescription.MedicineId,
                    Quantity = x.prescription.Quantity.ToString(),
                    Root = x.prescription.Root,
                    Times = x.prescription.Times,
                    PrescriptionEnd = x.prescription.PrescriptionEnd,
                    PrescriptionEndAs = x.prescription.PrescriptionEndAs,
                    DoctorSignature = x.prescription.DoctorSignature,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    Price = x.prescription.Price,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    DoctorId = doctor != null ? doctor.Id : null,
                    DoctorFirstName = doctor != null ? doctor.FirstName : null,
                    DoctorLastName = doctor != null ? doctor.LastName : null,
                    CreatedDate = x.prescription.CreatedDate,
                    CreatedBy = x.prescription.CreatedBy,
                    UpdateDate = x.prescription.UpdateDate,
                    UpdateBy = x.prescription.UpdateBy
                };
            }).ToList();
        }
    }
}
