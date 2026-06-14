using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.FileModels
{
    public class UploadFile
    {
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public IFormFile? file { get; set; }
    }
}
