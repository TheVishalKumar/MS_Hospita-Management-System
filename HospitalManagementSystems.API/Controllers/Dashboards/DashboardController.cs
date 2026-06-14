using HospitalManagementSystem.Models.DTO.Billings;
using HospitalManagementSystem.Models.Models.Billings;
using HospitalManagementSystem.Services.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystems.API.Controllers.Dashboards
{
   // [Authorize]
    using HospitalManagementSystems.API.Attributes;

    [Route("api/[controller]")]
    [ApiController]
    
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardData _dashboardData;
        public DashboardController(IDashboardData dashboardData)
        {
            _dashboardData = dashboardData;
        }

        /// <summary>
        /// Get complete dashboard data with all statistics, counts, and today's details
        /// </summary>
        /// <remarks>
        /// Returns aggregated dashboard information including:
        /// - Total and today's appointment counts
        /// - Total and today's medicine counts
        /// - Total and today's patient counts
        /// - Today's appointment details with patient information
        /// - Today's patient details
        /// - Total due amounts
        /// - Payment breakdown by payment type (Cash, Card, UPI, Other)
        /// </remarks>
        /// <returns>DashboardDataDto containing all dashboard metrics</returns>
        [HttpGet]
        [Route("GetDashboard")]
        public async Task<object> GetDashboard()
        {
            return await _dashboardData.GetDashboardData();
        }

        /// <summary>
        /// Get appointment statistics (total and today's count)
        /// </summary>
        /// <remarks>
        /// Returns appointment metrics:
        /// - AppointmentTotalCounts: Total appointments in database
        /// - AppointmentTodayCounts: Appointments scheduled for today
        /// </remarks>
        /// <returns>Object with appointment counts</returns>
        [HttpGet]
        [Route("GetAppointmentStats")]
        public async Task<object> GetAppointmentStats()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = new
                {
                    AppointmentTotalCounts = dashboardDto?.AppointmentTotalCounts ?? 0,
                    AppointmentTodayCounts = dashboardDto?.AppointmentTodayCounts ?? 0
                },
                error = (object)null
            };
        }

        /// <summary>
        /// Get patient statistics (total and today's count)
        /// </summary>
        /// <remarks>
        /// Returns patient metrics:
        /// - PatientsTotalCounts: Total registered patients
        /// - PatientsTodayCounts: Patients added today
        /// </remarks>
        /// <returns>Object with patient counts</returns>
        [HttpGet]
        [Route("GetPatientStats")]
        public async Task<object> GetPatientStats()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = new
                {
                    PatientsTotalCounts = dashboardDto?.PatientsTotalCounts ?? 0,
                    PatientsTodayCounts = dashboardDto?.PatientsTodayCounts ?? 0
                },
                error = (object)null
            };
        }

        /// <summary>
        /// Get medicine statistics (total and today's count)
        /// </summary>
        /// <remarks>
        /// Returns medicine metrics:
        /// - MedicineTotalCounts: Total medicines in inventory
        /// - MedicineTodayCounts: Medicines added today
        /// </remarks>
        /// <returns>Object with medicine counts</returns>
        [HttpGet]
        [Route("GetMedicineStats")]
        public async Task<object> GetMedicineStats()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = new
                {
                    MedicineTotalCounts = dashboardDto?.MedicineTotalCounts ?? 0,
                    MedicineTodayCounts = dashboardDto?.MedicineTodayCounts ?? 0
                },
                error = (object)null
            };
        }

        /// <summary>
        /// Get today's appointment details with patient information
        /// </summary>
        /// <remarks>
        /// Returns list of appointments scheduled for today including:
        /// - AppointmentDate
        /// - AppointmentTime
        /// - PatientName (First + Middle + Last name)
        /// - MobileNo (Patient contact)
        /// - RoomNo
        /// - CreatedDate
        /// </remarks>
        /// <returns>List of today's appointment details</returns>
        [HttpGet]
        [Route("GetTodayAppointments")]
        public async Task<object> GetTodayAppointments()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = dashboardDto?.TodayAppointmentsDetails ?? new List<object>(),
                error = (object)null
            };
        }

        /// <summary>
        /// Get today's patient details
        /// </summary>
        /// <remarks>
        /// Returns list of patients created/registered today including:
        /// - Id
        /// - FirstName, MiddleName, LastName
        /// - Age, Gender
        /// - MobileNo, EmailId
        /// - Address
        /// - BloodGroup
        /// - With all audit trail information
        /// </remarks>
        /// <returns>List of today's patient details</returns>
        [HttpGet]
        [Route("GetTodayPatients")]
        public async Task<object> GetTodayPatients()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = dashboardDto?.TodayPatientDetails ?? new List<object>(),
                error = (object)null
            };
        }

        /// <summary>
        /// Get financial overview (due amounts and payment breakdown)
        /// </summary>
        /// <remarks>
        /// Returns financial metrics:
        /// - DueAmount: Total outstanding amount across all prescriptions
        /// - AmountDetails breakdown:
        ///   - Cash: Amount paid via cash today
        ///   - Card: Amount paid via card today
        ///   - UPI: Amount paid via UPI today
        ///   - Other: Amount paid via other methods today
        /// </remarks>
        /// <returns>Object with financial information</returns>
        [HttpGet]
        [Route("GetFinancialOverview")]
        public async Task<object> GetFinancialOverview()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            var cash = dashboardDto?.AmountDetails?.Cash ?? 0.0;
            var card = dashboardDto?.AmountDetails?.Card ?? 0.0;
            var upi = dashboardDto?.AmountDetails?.UPI ?? 0.0;
            var other = dashboardDto?.AmountDetails?.Other ?? 0.0;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = new
                {
                    DueAmount = dashboardDto?.DueAmount ?? 0.0,
                    AmountDetails = new
                    {
                        Cash = cash,
                        Card = card,
                        UPI = upi,
                        Other = other,
                        TotalCollected = cash + card + upi + other
                    }
                },
                error = (object)null
            };
        }

        /// <summary>
        /// Get all counts (appointments, patients, medicines)
        /// </summary>
        /// <remarks>
        /// Returns all count metrics in one call:
        /// - AppointmentTotalCounts
        /// - AppointmentTodayCounts
        /// - PatientsTotalCounts
        /// - PatientsTodayCounts
        /// - MedicineTotalCounts
        /// - MedicineTodayCounts
        /// </remarks>
        /// <returns>Object with all count statistics</returns>
        [HttpGet]
        [Route("GetAllCounts")]
        public async Task<object> GetAllCounts()
        {
            var dashboardData = await _dashboardData.GetDashboardData();
            var response = dashboardData as dynamic;
            var dashboardDto = response.Data as dynamic;
            
            return new
            {
                statusCode = 200,
                statusMessage = "Success",
                data = new
                {
                    AppointmentTotalCounts = dashboardDto?.AppointmentTotalCounts ?? 0,
                    AppointmentTodayCounts = dashboardDto?.AppointmentTodayCounts ?? 0,
                    PatientsTotalCounts = dashboardDto?.PatientsTotalCounts ?? 0,
                    PatientsTodayCounts = dashboardDto?.PatientsTodayCounts ?? 0,
                    MedicineTotalCounts = dashboardDto?.MedicineTotalCounts ?? 0,
                    MedicineTodayCounts = dashboardDto?.MedicineTodayCounts ?? 0
                },
                error = (object)null
            };
        }
    }
}
