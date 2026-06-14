using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Billings;
using HospitalManagementSystem.Models.DTO.Prescriptions;
using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Billings;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Billings
{
    public class PrescriptionBillingService : IPrescriptionBillingRepository
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;
        public PrescriptionBillingService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<object> CreateBilling(CreatePrescriptionBillingDto prescriptionBillingDto)
        {
            var billing = _mapper.Map<PrescriptionBilling>(prescriptionBillingDto);
            billing.Id = Guid.NewGuid();
            billing.CreatedDate = DateTime.Now;
            
           await _dbContext.PrescriptionBillings.AddAsync(billing);
           await _dbContext.SaveChangesAsync();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, billing.Id);
        }

        public async Task<object> GetBillingByAppointmemtId(Guid appointmentId)
        {
            // Load from DB first
            var joinedData = await _dbContext.PrescriptionBillings
                .AsNoTracking()
                .Where(x => x.AppointmentId == appointmentId)
                .Join(_dbContext.AppointmentMaster,
                    billing => billing.AppointmentId,
                    appointment => appointment.Id,
                    (billing, appointment) => new { billing, appointment })
                .ToListAsync();

            // Load related patients and doctors
            var patients = await _dbContext.PatientMaster.AsNoTracking().ToListAsync();
            var doctors = await _dbContext.DoctorMaster.AsNoTracking().ToListAsync();

            // Client-side operations
            var data = joinedData
                .GroupJoin(patients,
                    joined => joined.appointment.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.billing, joined.appointment, patient = patient.FirstOrDefault() })
                .GroupJoin(doctors,
                    joined => joined.appointment.DoctorAssigned,
                    doctor => doctor.Id,
                    (joined, doctor) => new { joined.billing, joined.appointment, joined.patient, doctor = doctor.FirstOrDefault() })
                .Select(x => new GetPrescriptionBillingDto
                {
                    Id = x.billing.Id,
                    AppointmentId = x.billing.AppointmentId,
                    Amount = x.billing.Amount,
                    IsDiscount = x.billing.IsDiscount,
                    Discount = x.billing.Discount,
                    IsGST = x.billing.IsGST,
                    GstAmount = x.billing.GstAmount,
                    TotalAmount = x.billing.TotalAmount,
                    PaidAmount = x.billing.PaidAmount,
                    DueAmount = x.billing.DueAmount,
                    PaymentType = x.billing.PaymentType,
                    PatientId = x.patient != null ? x.patient.Id : null,
                    PatientFirstName = x.patient != null ? x.patient.FirstName : null,
                    PatientLastName = x.patient != null ? x.patient.LastName : null,
                    PatientEmail = x.patient != null ? x.patient.Email : null,
                    PatientMobileNo = x.patient != null ? x.patient.MobileNo : null,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Disease = x.appointment.Disease,
                    DoctorId = x.doctor != null ? x.doctor.Id : null,
                    DoctorFirstName = x.doctor != null ? x.doctor.FirstName : null,
                    DoctorLastName = x.doctor != null ? x.doctor.LastName : null,
                    BillingStatus = x.billing.DueAmount == 0 ? "Paid" : (x.billing.PaidAmount > 0 ? "Partial" : "Pending"),
                    CreatedDate = x.billing.CreatedDate,
                    CreatedBy = x.billing.CreatedBy,
                    UpdateDate = x.billing.UpdateDate,
                    UpdateBy = x.billing.UpdateBy
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            if (data != null && data.Count > 0) 
            {
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, data);
            }
            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, "No Data Found");
        }

        public async Task<object> GetPrescriptionBillingByAppointmemtId(Guid billingId, Guid appointmentId)
        {
            // Load from DB first
            var joinedData = await _dbContext.PrescriptionBillings
                .AsNoTracking()
                .Where(x => x.Id == billingId && x.AppointmentId == appointmentId)
                .Join(_dbContext.AppointmentMaster,
                    billing => billing.AppointmentId,
                    appointment => appointment.Id,
                    (billing, appointment) => new { billing, appointment })
                .ToListAsync();

            // Load related patients and doctors
            var patients = await _dbContext.PatientMaster.AsNoTracking().ToListAsync();
            var doctors = await _dbContext.DoctorMaster.AsNoTracking().ToListAsync();

            // Client-side operations
            var data = joinedData
                .GroupJoin(patients,
                    joined => joined.appointment.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.billing, joined.appointment, patient = patient.FirstOrDefault() })
                .GroupJoin(doctors,
                    joined => joined.appointment.DoctorAssigned,
                    doctor => doctor.Id,
                    (joined, doctor) => new { joined.billing, joined.appointment, joined.patient, doctor = doctor.FirstOrDefault() })
                .Select(x => new GetPrescriptionBillingDto
                {
                    Id = x.billing.Id,
                    AppointmentId = x.billing.AppointmentId,
                    Amount = x.billing.Amount,
                    IsDiscount = x.billing.IsDiscount,
                    Discount = x.billing.Discount,
                    IsGST = x.billing.IsGST,
                    GstAmount = x.billing.GstAmount,
                    TotalAmount = x.billing.TotalAmount,
                    PaidAmount = x.billing.PaidAmount,
                    DueAmount = x.billing.DueAmount,
                    PaymentType = x.billing.PaymentType,
                    PatientId = x.patient != null ? x.patient.Id : null,
                    PatientFirstName = x.patient != null ? x.patient.FirstName : null,
                    PatientLastName = x.patient != null ? x.patient.LastName : null,
                    PatientEmail = x.patient != null ? x.patient.Email : null,
                    PatientMobileNo = x.patient != null ? x.patient.MobileNo : null,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Disease = x.appointment.Disease,
                    DoctorId = x.doctor != null ? x.doctor.Id : null,
                    DoctorFirstName = x.doctor != null ? x.doctor.FirstName : null,
                    DoctorLastName = x.doctor != null ? x.doctor.LastName : null,
                    BillingStatus = x.billing.DueAmount == 0 ? "Paid" : (x.billing.PaidAmount > 0 ? "Partial" : "Pending"),
                    CreatedDate = x.billing.CreatedDate,
                    CreatedBy = x.billing.CreatedBy,
                    UpdateDate = x.billing.UpdateDate,
                    UpdateBy = x.billing.UpdateBy
                })
                .FirstOrDefault();

            if (data != null)
            {
                var prescriptionInfo = await _dbContext.PatientPrescriptionMaster
                    .Where(x => x.AppointmentId == appointmentId && x.BillingId == billingId)
                    .ToListAsync();

                if (prescriptionInfo.Count > 0)
                {
                    data.PatientPrescriptionDetails = prescriptionInfo;

                    if (data.PatientPrescriptionDetails.Count > 0)
                    {
                        foreach (var item in data.PatientPrescriptionDetails)
                        {
                            var medicine = await _dbContext.MedicineMaster
                                .Where(x => x.Id == item.MedicineId)
                                .ToListAsync();
                            item.MedicineDetails = medicine;
                        }
                    }
                }

                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, data);
            }

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, "No Data Found");
        }

        public async Task<List<GetPrescriptionBillingDto>> GetBillingsByPatientId(Guid patientId)
        {
            // Load from DB first
            var joinedData = await _dbContext.PrescriptionBillings
                .AsNoTracking()
                .Join(_dbContext.AppointmentMaster,
                    billing => billing.AppointmentId,
                    appointment => appointment.Id,
                    (billing, appointment) => new { billing, appointment })
                .Where(x => x.appointment.PatientId == patientId)
                .ToListAsync();

            // Load related patients and doctors
            var patients = await _dbContext.PatientMaster.AsNoTracking().ToListAsync();
            var doctors = await _dbContext.DoctorMaster.AsNoTracking().ToListAsync();

            // Client-side operations
            var data = joinedData
                .GroupJoin(patients,
                    joined => joined.appointment.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.billing, joined.appointment, patient = patient.FirstOrDefault() })
                .GroupJoin(doctors,
                    joined => joined.appointment.DoctorAssigned,
                    doctor => doctor.Id,
                    (joined, doctor) => new { joined.billing, joined.appointment, joined.patient, doctor = doctor.FirstOrDefault() })
                .Select(x => new GetPrescriptionBillingDto
                {
                    Id = x.billing.Id,
                    AppointmentId = x.billing.AppointmentId,
                    Amount = x.billing.Amount,
                    IsDiscount = x.billing.IsDiscount,
                    Discount = x.billing.Discount,
                    IsGST = x.billing.IsGST,
                    GstAmount = x.billing.GstAmount,
                    TotalAmount = x.billing.TotalAmount,
                    PaidAmount = x.billing.PaidAmount,
                    DueAmount = x.billing.DueAmount,
                    PaymentType = x.billing.PaymentType,
                    PatientId = x.patient != null ? x.patient.Id : null,
                    PatientFirstName = x.patient != null ? x.patient.FirstName : null,
                    PatientLastName = x.patient != null ? x.patient.LastName : null,
                    PatientEmail = x.patient != null ? x.patient.Email : null,
                    PatientMobileNo = x.patient != null ? x.patient.MobileNo : null,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Disease = x.appointment.Disease,
                    DoctorId = x.doctor != null ? x.doctor.Id : null,
                    DoctorFirstName = x.doctor != null ? x.doctor.FirstName : null,
                    DoctorLastName = x.doctor != null ? x.doctor.LastName : null,
                    BillingStatus = x.billing.DueAmount == 0 ? "Paid" : (x.billing.PaidAmount > 0 ? "Partial" : "Pending"),
                    CreatedDate = x.billing.CreatedDate,
                    CreatedBy = x.billing.CreatedBy,
                    UpdateDate = x.billing.UpdateDate,
                    UpdateBy = x.billing.UpdateBy
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return data;
        }

        public async Task<List<GetPrescriptionBillingDto>> GetBillingsByHospitalAndBranch(Guid hospitalId, Guid branchId)
        {
            // Load from DB first
            var joinedData = await _dbContext.PrescriptionBillings
                .AsNoTracking()
                .Join(_dbContext.AppointmentMaster,
                    billing => billing.AppointmentId,
                    appointment => appointment.Id,
                    (billing, appointment) => new { billing, appointment })
                .Where(x => x.appointment.HospitalId == hospitalId && x.appointment.BranchId == branchId)
                .ToListAsync();

            // Load related patients and doctors
            var patients = await _dbContext.PatientMaster.AsNoTracking().ToListAsync();
            var doctors = await _dbContext.DoctorMaster.AsNoTracking().ToListAsync();

            // Client-side operations
            var data = joinedData
                .GroupJoin(patients,
                    joined => joined.appointment.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.billing, joined.appointment, patient = patient.FirstOrDefault() })
                .GroupJoin(doctors,
                    joined => joined.appointment.DoctorAssigned,
                    doctor => doctor.Id,
                    (joined, doctor) => new { joined.billing, joined.appointment, joined.patient, doctor = doctor.FirstOrDefault() })
                .Select(x => new GetPrescriptionBillingDto
                {
                    Id = x.billing.Id,
                    AppointmentId = x.billing.AppointmentId,
                    Amount = x.billing.Amount,
                    IsDiscount = x.billing.IsDiscount,
                    Discount = x.billing.Discount,
                    IsGST = x.billing.IsGST,
                    GstAmount = x.billing.GstAmount,
                    TotalAmount = x.billing.TotalAmount,
                    PaidAmount = x.billing.PaidAmount,
                    DueAmount = x.billing.DueAmount,
                    PaymentType = x.billing.PaymentType,
                    HospitalId = hospitalId,
                    BranchId = branchId,
                    PatientId = x.patient != null ? x.patient.Id : null,
                    PatientFirstName = x.patient != null ? x.patient.FirstName : null,
                    PatientLastName = x.patient != null ? x.patient.LastName : null,
                    PatientEmail = x.patient != null ? x.patient.Email : null,
                    PatientMobileNo = x.patient != null ? x.patient.MobileNo : null,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Disease = x.appointment.Disease,
                    DoctorId = x.doctor != null ? x.doctor.Id : null,
                    DoctorFirstName = x.doctor != null ? x.doctor.FirstName : null,
                    DoctorLastName = x.doctor != null ? x.doctor.LastName : null,
                    BillingStatus = x.billing.DueAmount == 0 ? "Paid" : (x.billing.PaidAmount > 0 ? "Partial" : "Pending"),
                    CreatedDate = x.billing.CreatedDate,
                    CreatedBy = x.billing.CreatedBy,
                    UpdateDate = x.billing.UpdateDate,
                    UpdateBy = x.billing.UpdateBy
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return data;
        }

        public async Task<List<GetPrescriptionBillingDto>> GetPendingBillings(Guid hospitalId, Guid branchId)
        {
            // Load from DB first
            var joinedData = await _dbContext.PrescriptionBillings
                .AsNoTracking()
                .Join(_dbContext.AppointmentMaster,
                    billing => billing.AppointmentId,
                    appointment => appointment.Id,
                    (billing, appointment) => new { billing, appointment })
                .Where(x => x.appointment.HospitalId == hospitalId && x.appointment.BranchId == branchId && x.billing.DueAmount > 0)
                .ToListAsync();

            // Load related patients and doctors
            var patients = await _dbContext.PatientMaster.AsNoTracking().ToListAsync();
            var doctors = await _dbContext.DoctorMaster.AsNoTracking().ToListAsync();

            // Client-side operations
            var data = joinedData
                .GroupJoin(patients,
                    joined => joined.appointment.PatientId,
                    patient => patient.Id,
                    (joined, patient) => new { joined.billing, joined.appointment, patient = patient.FirstOrDefault() })
                .GroupJoin(doctors,
                    joined => joined.appointment.DoctorAssigned,
                    doctor => doctor.Id,
                    (joined, doctor) => new { joined.billing, joined.appointment, joined.patient, doctor = doctor.FirstOrDefault() })
                .Select(x => new GetPrescriptionBillingDto
                {
                    Id = x.billing.Id,
                    AppointmentId = x.billing.AppointmentId,
                    Amount = x.billing.Amount,
                    IsDiscount = x.billing.IsDiscount,
                    Discount = x.billing.Discount,
                    IsGST = x.billing.IsGST,
                    GstAmount = x.billing.GstAmount,
                    TotalAmount = x.billing.TotalAmount,
                    PaidAmount = x.billing.PaidAmount,
                    DueAmount = x.billing.DueAmount,
                    PaymentType = x.billing.PaymentType,
                    HospitalId = hospitalId,
                    BranchId = branchId,
                    PatientId = x.patient != null ? x.patient.Id : null,
                    PatientFirstName = x.patient != null ? x.patient.FirstName : null,
                    PatientLastName = x.patient != null ? x.patient.LastName : null,
                    PatientEmail = x.patient != null ? x.patient.Email : null,
                    PatientMobileNo = x.patient != null ? x.patient.MobileNo : null,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Disease = x.appointment.Disease,
                    DoctorId = x.doctor != null ? x.doctor.Id : null,
                    DoctorFirstName = x.doctor != null ? x.doctor.FirstName : null,
                    DoctorLastName = x.doctor != null ? x.doctor.LastName : null,
                    BillingStatus = x.billing.DueAmount == 0 ? "Paid" : (x.billing.PaidAmount > 0 ? "Partial" : "Pending"),
                    CreatedDate = x.billing.CreatedDate,
                    CreatedBy = x.billing.CreatedBy,
                    UpdateDate = x.billing.UpdateDate,
                    UpdateBy = x.billing.UpdateBy
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            return data;
        }
    }
}
