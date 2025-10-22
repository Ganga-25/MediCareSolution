using MediCare.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;






    namespace MediCare.Application.DTOs.ProfileUpdateDTO
    {
        public class PatientUpdateDTO
        {
             //public string UHID { get; set; }
             public int? Age { get; set; }
            public string? ContactNumber { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Transgender|Prefer not to say)$",
             ErrorMessage = "Gender must be Male, Female, Transgender, or Prefer not to say")]

        public Gender? Gender { get; set; }
        }
    }
