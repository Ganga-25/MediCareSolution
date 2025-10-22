using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class DoctorService:IDoctorService
    {
        private readonly IGenericRepository<Users> _userRepo;
        private readonly IGenericRepository<Doctors> _doctorRepo;
         public DoctorService(IGenericRepository<Users> userRepo, IGenericRepository<Doctors> doctorRepo)
        {
            _userRepo = userRepo;
            _doctorRepo = doctorRepo;
        }

        async Task<ApiResponse<bool>> RegisterDoctorAsync(DoctorUpdateDTO Docdto, int userId)
        {
            // 1️⃣ Check user exists
            var users = await _doctorRepo.GetAllAsync("Users");
            if (!users.Any(u => u.UserId == userId))
                return new ApiResponse<bool>(404, "User not found", false);

            // 2️⃣ Prepare new lab technician
            var newDoctor = new Doctors
            {

                UserId = userId,
                DepartmentId = Docdto.DepartmentId,
                ContactNumber=Docdto.ContactNumber,
                MedicalRegistrationNumber=Docdto.MedicalRegistrationNumber,
                Fees=Docdto.Fees,
                Experience=Docdto.Experience,
                ProfilePhoto=Docdto.ProfilePhoto
                

            };

            // 3️⃣ Insert
            int rowsAffected = await _doctorRepo.AddAsync("SP_DOCTORS", newDoctor);

            return new ApiResponse<bool>(
                rowsAffected > 0 ? 201 : 400,
                rowsAffected > 0 ? "Lab technician registered successfully" : "Insert failed",
                rowsAffected > 0
            );
        }

    }
}
