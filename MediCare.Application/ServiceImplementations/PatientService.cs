using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class PatientService : IPatientService
    {


        private readonly IGenericRepository<Users> _userRepo;
        private readonly IGenericRepository<Patients> _patientRepo;
        public PatientService(IGenericRepository<Users> userRepo, IGenericRepository<Patients> patientRepo)
        {
            _userRepo = userRepo;
            _patientRepo = patientRepo;
        }
        public async Task<ApiResponse<bool>> RegisterPatientAsync(PatientUpdateDTO Patdto, int userId)
        {
            // 1️⃣ Check user exists
            var users = await _userRepo.GetAllAsync("Users");
            if (!users.Any(u => u.UserId == userId))
                return new ApiResponse<bool>(404, "User not found", false);


            string genderString = Patdto.Gender switch
            {
                Gender.Male => "Male",
                Gender.Female => "Female",
                Gender.Transgender => "Transgender",
                Gender.Prefernottosay => "Prefer not to say",
                _ => throw new ArgumentException("Invalid gender value")
            };

            // 2️⃣ Prepare new lab technician
            var newDoctor = new Patients
            {

                UserId = userId,
                UHID=GenerateUHID(),
                ContactNumber = Patdto.ContactNumber,
                Gender= genderString,
                Age= Patdto.Age

                
                


            };

            // 3️⃣ Insert
            int rowsAffected = await _patientRepo.AddAsync("SP_Patients", newDoctor);

            return new ApiResponse<bool>(
                rowsAffected > 0 ? 201 : 400,
                rowsAffected > 0 ? "Patient registered successfully" : "Insert failed",
                rowsAffected > 0
            );
        }

        public static string GenerateUHID()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }

    }


