using AutoMapper.Configuration;
using HospitalManagementSystem.Models.Constants;
using HospitalManagementSystem.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Models.Models.Categories
{
    public class Category : CommonEntity
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int Version { get; set; } = 0;
    }
}
