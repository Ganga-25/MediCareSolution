using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Entities
{
    public class LabTechnicicanCredential:BaseEntity
    {
        public int Id { get; set; }
        public int LabTechnicianId { get; set; }

        public CredentialType CredentialType { get; set; }

        public string DegreeType { get; set; }=string.Empty;
        public string DegreeName { get; set; } = string.Empty;
        public string InstitutionName { get; set; }= string.Empty;
        public string LabName { get; set; }=string.Empty ;
        public string Designation { get; set; }=string.Empty;
        public string DocumentType { get; set; } = string.Empty;

        public string UploadDocument { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }  
        public DateOnly EndDate { get; set; }     
       
    }
}
