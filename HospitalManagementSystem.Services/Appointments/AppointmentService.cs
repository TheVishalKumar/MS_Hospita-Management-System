#nullable disable
using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Users;
using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Users;
using HospitalManagementSystem.Models.Models.Doctors;
using HospitalManagementSystem.Models.Models.Wards;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Appointments
{
    public class AppointmentService : IAppointmentRepository
    {
       
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public AppointmentService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext!;
            _mapper = mapper!;
        }
        public async Task<Response> CreateAsync(CreateAppointmentDto createAppointmentDto)
        {
            var appointmentDetails = _mapper.Map<Appointment>(createAppointmentDto);
            appointmentDetails.Id = Guid.NewGuid();
            appointmentDetails.CreatedDate = DateTime.Now;
            appointmentDetails.AppointmentDate=createAppointmentDto.AppointmentDate.AddDays(1);
            _dbContext.AppointmentMaster.Add(appointmentDetails);
            _dbContext.SaveChanges();

            return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, CommonMessage.SuccessMessage);
        }

        public async Task<GetAppointmentDto> Get(Guid id)
        {
            var appointment = await _dbContext.AppointmentMaster.FindAsync(id);
            
            if (appointment == null)
                return null;

            // Fetch patient details
            var patient = await _dbContext.PatientMaster.FindAsync(appointment.PatientId);
            
            // Fetch doctor details
            var doctor = appointment.DoctorAssigned != Guid.Empty 
                ? await _dbContext.DoctorMaster.FindAsync(appointment.DoctorAssigned)
                : null;

            // Fetch ward details
            var ward = appointment.WardId != Guid.Empty
                ? await _dbContext.WardMaster.FindAsync(appointment.WardId)
                : null;

            return new GetAppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientFirstName = patient?.FirstName,
                PatientLastName = patient?.LastName,
                PatientMiddleName = patient?.MiddleName,
                PatientEmail = patient?.Email,
                PatientMobileNo = patient?.MobileNo,
                PatientAge = patient?.Age,
                PatientGender = patient?.Gender,
                PatientAddress = patient?.Address,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentTime = appointment.AppointmentTime,
                DoctorAssigned = appointment.DoctorAssigned,
                DoctorFirstName = doctor?.FirstName,
                DoctorLastName = doctor?.LastName,
                DoctorMiddleName = doctor?.MiddleName,
                DoctorEmail = doctor?.EmailId,
                DoctorMobileNo = doctor?.MobileNo,
                Height = appointment.Height,
                Weight = appointment.Weight,
                WardId = appointment.WardId,
                WardName = ward?.WardName,
                WardDescription = ward?.WardDescription,
                Disease = appointment.Disease,
                RoomNo = appointment.RoomNo,
                Prescription = appointment.Prescription,
                PrescriptionId = appointment.PrescriptionId,
                Status = appointment.Status,
                HospitalId = appointment.HospitalId,
                BranchId = appointment.BranchId,
                CreatedDate = appointment.CreatedDate,
                CreatedBy = appointment.CreatedBy,
                UpdateDate = appointment.UpdateDate,
                UpdateBy = appointment.UpdateBy
            };
        }

        public async Task<List<GetAppointmentDto>> GetList()
        {
            var data = await _dbContext.AppointmentMaster
                .AsNoTracking()
                .Join(_dbContext.PatientMaster, 
                    appointment => appointment.PatientId, 
                    patient => patient.Id, 
                    (appointment, patient) => new { appointment, patient })
                .GroupJoin(_dbContext.WardMaster,
                    joined => joined.appointment.WardId,
                    ward => ward.Id,
                    (joined, ward) => new { joined.appointment, joined.patient, ward })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = data
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

            return data.Select(x => 
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetAppointmentDto
                {
                    Id = x.appointment.Id,
                    PatientId = x.appointment.PatientId,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientMiddleName = x.patient.MiddleName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    PatientAddress = x.patient.Address,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    DoctorAssigned = x.appointment.DoctorAssigned,
                    DoctorFirstName = doctor?.FirstName,
                    DoctorLastName = doctor?.LastName,
                    DoctorMiddleName = doctor?.MiddleName,
                    DoctorEmail = doctor?.EmailId,
                    DoctorMobileNo = doctor?.MobileNo,
                    Height = x.appointment.Height,
                    Weight = x.appointment.Weight,
                    WardId = x.appointment.WardId,
                    WardName = x.ward.FirstOrDefault()?.WardName,
                    WardDescription = x.ward.FirstOrDefault()?.WardDescription,
                    Disease = x.appointment.Disease,
                    RoomNo = x.appointment.RoomNo,
                    Prescription = x.appointment.Prescription,
                    PrescriptionId = x.appointment.PrescriptionId,
                    Status = x.appointment.Status,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    CreatedDate = x.appointment.CreatedDate,
                    CreatedBy = x.appointment.CreatedBy,
                    UpdateDate = x.appointment.UpdateDate,
                    UpdateBy = x.appointment.UpdateBy
                };
            }).ToList();
        }


        public async Task<List<GetAppointmentDto>> GetTodayAppointmentListByPatient(Guid patientId, Guid HospitalId, Guid BranchId)
        {
            // Fetch appointments with patient details
            var appointmentsWithPatients = await _dbContext.AppointmentMaster
                .AsNoTracking()
                .Where(a => a.PatientId == patientId 
                    && a.HospitalId == HospitalId 
                    && a.BranchId == BranchId)
                .Join(_dbContext.PatientMaster,
                    appointment => appointment.PatientId,
                    patient => patient.Id,
                    (appointment, patient) => new { appointment, patient })
                .ToListAsync();

            // Get distinct ward IDs and doctor IDs
            var wardIds = appointmentsWithPatients
                .Select(x => x.appointment.WardId)
                .Where(w => w != null && w != Guid.Empty)
                .Distinct()
                .ToList();

            var doctorIds = appointmentsWithPatients
                .Select(x => x.appointment.DoctorAssigned)
                .Where(d => d != null && d != Guid.Empty)
                .Distinct()
                .ToList();

            // Fetch wards
            var wards = wardIds.Count > 0
                ? await _dbContext.WardMaster
                    .AsNoTracking()
                    .Where(w => wardIds.Contains(w.Id))
                    .ToListAsync()
                : new List<WardMaster>();

            // Fetch doctors
            var doctors = doctorIds.Count > 0
                ? await _dbContext.DoctorMaster
                    .AsNoTracking()
                    .Where(d => doctorIds.Contains(d.Id))
                    .ToListAsync()
                : new List<DoctorMaster>();

            // Map to DTO with ward and doctor information
            return appointmentsWithPatients.Select(x => 
            {
                var ward = wards.FirstOrDefault(w => w.Id == x.appointment.WardId);
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetAppointmentDto
                {
                    Id = x.appointment.Id,
                    PatientId = x.appointment.PatientId,
                    DoctorAssigned = x.appointment.DoctorAssigned,
                    DoctorFirstName = doctor?.FirstName,
                    DoctorLastName = doctor?.LastName,
                    DoctorMiddleName = doctor?.MiddleName,
                    DoctorEmail = doctor?.EmailId,
                    DoctorMobileNo = doctor?.MobileNo,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    Status = x.appointment.Status,
                    Disease = x.appointment.Disease,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    PatientAddress = x.patient.Address,
                    CreatedDate = x.appointment.CreatedDate,
                    CreatedBy = x.appointment.CreatedBy,
                    UpdateDate = x.appointment.UpdateDate,
                    UpdateBy = x.appointment.UpdateBy,
                    Height = x.appointment.Height,
                    Weight = x.appointment.Weight,
                    RoomNo = x.appointment.RoomNo,
                    WardId = x.appointment.WardId,
                    WardName = ward?.WardName,
                    WardDescription = ward?.WardDescription,
                    Prescription = x.appointment.Prescription,
                    PrescriptionId = x.appointment.PrescriptionId
                };
            }).ToList();
        }

        public async Task<object> GetTodayAppointmentList(Guid HospitalId, Guid BranchId)
        {
            var param = new List<SqlParameter>();
            //var param = new SqlParameter("@Action", "TodayForAll");
            param.Add(new SqlParameter("@Action", "TodayForAll"));
            param.Add(new SqlParameter("@PatientId", Guid.Parse("00000000-0000-0000-0000-000000000000")));
            param.Add(new SqlParameter("@FromDate", DBNull.Value));
            param.Add(new SqlParameter("@ToDate", DBNull.Value));
            param.Add(new SqlParameter("@HospitalId", HospitalId));
            param.Add(new SqlParameter("@BranchId", BranchId));

            var data = await _dbContext.SP_GetTodayAppointmentList.FromSqlRaw("GetTodayAppointmentList @Action, @PatientId, @FromDate, @ToDate, @HospitalId, @BranchId", param.ToArray()).ToListAsync();
            //var appointmentDetails = _mapper.Map<List<GetAppointmentDto>>(data);
            return data;
        }

        public async Task<object> GetAppointmentListByPatientAndDate(Guid patientId, DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId)
         {
            string newfDate = fromDate.ToString();
            string newtDate = toDate.ToString();
            DateTime fdate = DateTime.ParseExact(newfDate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            var tdate = DateTime.ParseExact(newtDate, "dd-MM-yyyy hh:mm:ss",
                                   CultureInfo.InvariantCulture);

            string newFromDate = fdate.AddDays(-1).ToString("yyyy-MM-dd");
            string newToDate = tdate.AddDays(1).ToString("yyyy-MM-dd");

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Action", "GetByPatientAndFromDateToDate"));
            param.Add(new SqlParameter("@PatientId", patientId));
            param.Add(new SqlParameter("@FromDate", newFromDate.ToString()));
            param.Add(new SqlParameter("@ToDate", newToDate.ToString()));
            param.Add(new SqlParameter("@HospitalId", HospitalId));
            param.Add(new SqlParameter("@BranchId", BranchId));


            var data = await _dbContext.SP_GetTodayAppointmentList.FromSqlRaw("GetTodayAppointmentList @Action,@PatientId, @FromDate, @ToDate, @HospitalId,@BranchId ", param.ToArray()).ToListAsync();
            //var appointmentDetails = _mapper.Map<List<GetAppointmentDto>>(data);
            return data;
        }

        public async Task<object> GetAppointmentListByDates(DateTime fromDate, DateTime toDate, Guid HospitalId, Guid BranchId)
        {
            string newfDate = fromDate.ToString();
            string newtDate = toDate.ToString();
            DateTime fdate = DateTime.ParseExact(newfDate, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            var tdate = DateTime.ParseExact(newtDate, "dd-MM-yyyy hh:mm:ss",
                                   CultureInfo.InvariantCulture);

            string newFromDate = fdate.AddDays(-1).ToString("yyyy-MM-dd");
            string newToDate = tdate.AddDays(1).ToString("yyyy-MM-dd");

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Action", "GetByFromDateToDate"));
            param.Add(new SqlParameter("@FromDate", newFromDate.ToString()));
            param.Add(new SqlParameter("@ToDate", newToDate.ToString()));
            param.Add(new SqlParameter("@HospitalId", HospitalId));
            param.Add(new SqlParameter("@BranchId", BranchId));
            param.Add(new SqlParameter("@PatientId", Guid.Parse("00000000-0000-0000-0000-000000000000")));

            var data = await _dbContext.SP_GetTodayAppointmentList.FromSqlRaw("GetTodayAppointmentList @Action,@PatientId, @FromDate, @ToDate, @HospitalId,@BranchId ", param.ToArray()).ToListAsync();
            //var appointmentDetails = _mapper.Map<List<GetAppointmentDto>>(data);
            return data;
        }

        public async Task<bool> UpdateStatus(Guid id, string status, Guid updateBy)
        {
            var data = _dbContext.AppointmentMaster.Find(id);

            data.Status  = status;
            data.UpdateBy = updateBy;
            data.UpdateDate = DateTime.Now;
            _dbContext.AppointmentMaster.Update(data);

            return true;
        }

        public async Task<List<GetAppointmentDto>> GetAppointmentListByHospitalIdBranchId(Guid hospitalId, Guid branchId)
        {
            var data = await _dbContext.AppointmentMaster
                .AsNoTracking()
                .Where(a => a.HospitalId == hospitalId && a.BranchId == branchId)
                .Join(_dbContext.PatientMaster,
                    appointment => appointment.PatientId,
                    patient => patient.Id,
                    (appointment, patient) => new { appointment, patient })
                .GroupJoin(_dbContext.WardMaster,
                    joined => joined.appointment.WardId,
                    ward => ward.Id,
                    (joined, ward) => new { joined.appointment, joined.patient, ward })
                .ToListAsync();

            // Get distinct doctor IDs
            var doctorIds = data
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

            return data.Select(x => 
            {
                var doctor = doctors.FirstOrDefault(d => d.Id == x.appointment.DoctorAssigned);
                return new GetAppointmentDto
                {
                    Id = x.appointment.Id,
                    PatientId = x.appointment.PatientId,
                    PatientFirstName = x.patient.FirstName,
                    PatientLastName = x.patient.LastName,
                    PatientMiddleName = x.patient.MiddleName,
                    PatientEmail = x.patient.Email,
                    PatientMobileNo = x.patient.MobileNo,
                    PatientAge = x.patient.Age,
                    PatientGender = x.patient.Gender,
                    PatientAddress = x.patient.Address,
                    AppointmentDate = x.appointment.AppointmentDate,
                    AppointmentTime = x.appointment.AppointmentTime,
                    DoctorAssigned = x.appointment.DoctorAssigned,
                    DoctorFirstName = doctor?.FirstName,
                    DoctorLastName = doctor?.LastName,
                    DoctorMiddleName = doctor?.MiddleName,
                    DoctorEmail = doctor?.EmailId,
                    DoctorMobileNo = doctor?.MobileNo,
                    Height = x.appointment.Height,
                    Weight = x.appointment.Weight,
                    WardId = x.appointment.WardId,
                    WardName = x.ward.FirstOrDefault()?.WardName,
                    WardDescription = x.ward.FirstOrDefault()?.WardDescription,
                    Disease = x.appointment.Disease,
                    RoomNo = x.appointment.RoomNo,
                    Prescription = x.appointment.Prescription,
                    PrescriptionId = x.appointment.PrescriptionId,
                    Status = x.appointment.Status,
                    HospitalId = x.appointment.HospitalId,
                    BranchId = x.appointment.BranchId,
                    CreatedDate = x.appointment.CreatedDate,
                    CreatedBy = x.appointment.CreatedBy,
                    UpdateDate = x.appointment.UpdateDate,
                    UpdateBy = x.appointment.UpdateBy
                };
            }).ToList();
        }
    }
}
