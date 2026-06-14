using HospitalManagementSystem.Models.DTO.Dashboards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Services.Dashboards
{
    public interface IDashboardData
    {
        Task<object> GetDashboardData();


        //Task<int> Patients { get; set; }
        //Task<int> Appointments { get; set; }
        //Task<int> Medicines { get; set; }
        //Task<object> Amounts { get; set; }
        //Task<object> TodaysAppointments { get; set; }
        //Task<object> TodaysPatients { get; set; }
    }
}
