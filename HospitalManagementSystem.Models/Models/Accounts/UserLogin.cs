using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Accounts
{
    [NotMapped]
    public class UserLogin
    {
        public string? UserId { get; set; }
        public string? Password { get; set; }
    }
}
