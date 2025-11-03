using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.DTOs.CredentialDTO
{
    public class DoctorCredentialDTO
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public CredentialType CredentialType { get; set; }
        public string DegreeType { get; set; } 
        public string DegreeName { get; set; } 
        public string InstitutionName { get; set; } 
        public string HospitalName { get; set; } 
        public string Designation { get; set; } 
        public string DocumentType { get; set; } 

        public string UploadDocument { get; set; } 

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class AddDoctorCredentialDTO
    {
       
       
        public CredentialType CredentialType { get; set; }
        public string DegreeType { get; set; }
        public string DegreeName { get; set; }
        public string InstitutionName { get; set; }
        public string HospitalName { get; set; }
        public string Designation { get; set; }
        public string DocumentType { get; set; }

        public string UploadDocument { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class UpdateDoctorCredentialDTO
    {
        public int Id { get; set; }
        public CredentialType CredentialType { get; set; }
        public string DegreeType { get; set; }
        public string DegreeName { get; set; }
        public string InstitutionName { get; set; }
        public string HospitalName { get; set; }
        public string Designation { get; set; }
        public string DocumentType { get; set; }

        public string UploadDocument { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
