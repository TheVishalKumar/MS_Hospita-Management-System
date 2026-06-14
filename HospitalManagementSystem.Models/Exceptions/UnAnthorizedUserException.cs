using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Exceptions
{
    public class UnAnthorizedUserException : Exception
    {
        public UnAnthorizedUserException(string message):base(message)
        {
                
        }
    }
}
