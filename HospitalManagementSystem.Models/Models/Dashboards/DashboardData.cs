using HospitalManagementSystem.Models.Models.Appointments;
using HospitalManagementSystem.Models.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Dashboards
{
    public class DashboardData
    {
        public int PatientsTotalCounts { get; set; }
        public int PatientsTodayCounts { get; set; }
        public int AppointmentTotalCounts { get; set; }
        public int AppointmentTodayCounts { get; set; }
        public int MedicineTodayCounts { get; set; }
        public int MedicineTotalCounts { get; set; }
        public ICollection<PatientDetails>? TodayPatientDetails { get; set; }
        public ICollection<Appointment>? TodayAppointmentsDetails { get; set; }
        public AmountDetails? AmountDetails { get; set; }
        public double DueAmount { get; set; }
    }

    public class AmountDetails
    { 
        public double Cash { get; set; }
        public double UPI { get; set; }
        public double Card { get; set; }
        public double Other { get; set; }
    }
}
