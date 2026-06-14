using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Dashboards;
using HospitalManagementSystem.Models.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.DTO.Dashboards
{
    public class DashboardDataDto
    {
        public int PatientsTotalCounts { get; set; }
        public int PatientsTodayCounts { get; set; }
        public int AppointmentTotalCounts { get; set; }
        public int AppointmentTodayCounts { get; set; }
        public int MedicineTodayCounts { get; set; }
        public int MedicineTotalCounts { get; set; }
        public ICollection<PatientDetails>? TodayPatientDetails { get; set; }
        public object? TodayAppointmentsDetails { get; set; }
        public AmountDetails? AmountDetails { get; set; }
        public double DueAmount { get; set; }
    }

}
