using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Exceptions
{
    public class DuplicationRecordException: Exception
    {
        public DuplicationRecordException(string message):base(message)
        {
                
        }
    }
}
