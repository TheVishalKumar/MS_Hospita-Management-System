using AutoMapper;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.DTO.Appointments;
using HospitalManagementSystem.Models.DTO.Dashboards;
using HospitalManagementSystem.Models.Models.Dashboards;
using HospitalManagementSystem.Shared.Common.CommonResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HospitalManagementSystem.Shared.Common.Constants;

namespace HospitalManagementSystem.Services.Dashboards
{
    public class DashboardService : IDashboardData
    {
        private readonly AppDbContext _dbContext;
        private IMapper _mapper;

        public DashboardService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<object> GetDashboardData()
        {

            //StringBuilder stringBuilder = new StringBuilder();
            try
            {
                // Use a date-range [today, tomorrow) to avoid translating Date property and to be consistent
                // with how CreatedDate/AppointmentDate are stored (they include time part).
                var today = DateTime.Today; // local date at 00:00:00
                var tomorrow = today.AddDays(1);

                var appointmentTotalCounts = await _dbContext.AppointmentMaster.CountAsync();
                var appointmentTodayCounts = await _dbContext.AppointmentMaster
                    .Where(x => x.AppointmentDate >= today && x.AppointmentDate < tomorrow)
                    .CountAsync();

                var medicineTodayCounts = await _dbContext.MedicineMaster
                    .Where(x => x.CreatedDate >= today && x.CreatedDate < tomorrow)
                    .CountAsync();

                var medicineTotalCounts = await _dbContext.MedicineMaster.CountAsync();

                var patientsTodayCounts = await _dbContext.PatientMaster
                    .Where(x => x.CreatedDate >= today && x.CreatedDate < tomorrow)
                    .CountAsync();

                var patientsTotalCounts = await _dbContext.PatientMaster.CountAsync();

                var todayAppointmentsDetails = await _dbContext.AppointmentMaster
                    .Join(_dbContext.PatientMaster,
                        AM => AM.PatientId,
                        PM => PM.Id,
                        (AM, PM) => new
                        {
                            AM.AppointmentDate,
                            AM.AppointmentTime,
                            PatientName = PM.FirstName + " " + PM.LastName + " " + PM.MiddleName,
                            PM.MobileNo,
                            AM.RoomNo,
                            AM.CreatedDate

                        })
                    .Where(x => x.AppointmentDate >= today && x.AppointmentDate < tomorrow)
                    .ToListAsync();

                var todayPatientDetails = await _dbContext.PatientMaster
                    .Where(x => x.CreatedDate >= today && x.CreatedDate < tomorrow)
                    .ToListAsync();

                var dueAmount = _dbContext.PrescriptionBillings.Sum(x => x.DueAmount);


                var cashAmount = _dbContext.PrescriptionBillings
                                .Where(x => x.PaymentType == PaymentConst.Cash &&
                                x.CreatedDate >= today && x.CreatedDate < tomorrow
                                ).Sum(x => x.PaidAmount);

                var cardAmount = _dbContext.PrescriptionBillings
                                .Where(x => x.PaymentType == PaymentConst.Card && x.CreatedDate >= today && x.CreatedDate < tomorrow)
                                .Sum(x => x.PaidAmount);

                var upiAmount = _dbContext.PrescriptionBillings
                                .Where(x => x.PaymentType == PaymentConst.UPI && x.CreatedDate >= today && x.CreatedDate < tomorrow)
                                .Sum(x => x.PaidAmount);

                var otherAmount = _dbContext.PrescriptionBillings
                                .Where(x => x.PaymentType == PaymentConst.Other && x.CreatedDate >= today && x.CreatedDate < tomorrow)
                                .Sum(x => x.PaidAmount);

                var data = new DashboardDataDto()
                {
                    AppointmentTodayCounts = appointmentTodayCounts,
                    AppointmentTotalCounts = appointmentTotalCounts,
                    MedicineTodayCounts = medicineTodayCounts,
                    MedicineTotalCounts = medicineTotalCounts,
                    PatientsTodayCounts = patientsTodayCounts,
                    PatientsTotalCounts = patientsTotalCounts,
                    TodayAppointmentsDetails = todayAppointmentsDetails,
                    TodayPatientDetails = todayPatientDetails,
                    DueAmount = dueAmount,
                    AmountDetails = new AmountDetails
                    {
                        Card = cardAmount,
                        UPI = upiAmount,
                        Cash = cashAmount,
                        Other = otherAmount
                    }
                };
                return new Response(Convert.ToInt32(ResponseCode.Success), CommonMessage.SuccessMessage, data);
            }
            catch (Exception ex)
            {

                return new Response(Convert.ToInt32(ResponseCode.Exception), CommonMessage.ExceptionMessage, ex.Message.Trim().ToString());
            }

        }
    }
}
