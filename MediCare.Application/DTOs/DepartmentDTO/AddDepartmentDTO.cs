using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.DepartmentDTO
{
    public class AddDepartmentDTO
    {
       
        public string DepartmentName { get; set; } 
        public string Descriptions { get; set; } 
        public IFormFile? DepartmentImageUrl { get; set; } 
    }
}
