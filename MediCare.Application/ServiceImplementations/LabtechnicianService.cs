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
    public class LabtechnicianService : ILabtechnicianService
    {
        private readonly IGenericRepository<LabTechnicians> _LabTechnicianRepo;
        private readonly IGenericRepository<Users> _userRepo;

        public LabtechnicianService(IGenericRepository<LabTechnicians> labTechnicianRepo, IGenericRepository<Users> userRepo)
        {
            _LabTechnicianRepo = labTechnicianRepo;
            _userRepo = userRepo;

        }

        public async Task<ApiResponse<LabTechnicianDTO>> GetLabTechByUserIdAsync(int userId)
        {
            var LabTechs = (await _LabTechnicianRepo.GetAllAsync("Users"))
                                   .SingleOrDefault(x => x.UserId == userId);
            if (LabTechs == null)
                return new ApiResponse<LabTechnicianDTO>(404, $"LabTechnician with this id :{userId} not found. ");

            var dto = new LabTechnicianDTO
            {
                LabTechnicianId = LabTechs.LabTechnicianId,
                UserId = LabTechs.UserId,
                DepartmentId = LabTechs.DepartmentId,
                LicenceNumber = LabTechs.LicenceNumber,
                ContactNumber = LabTechs.ContactNumber,
                ProfilePhoto = LabTechs.ProfilePhoto,
                VerificationStatus = LabTechs.VerificationStatus.ToString(),
                IsActive = LabTechs.IsAvailable


            };
            return new ApiResponse<LabTechnicianDTO>(200, "LabTechnician Fetched Successfully.", dto);
        }

        public async Task<ApiResponse<bool>> RegisterLabTechnicianAsync(LabTechnicianUpdateDTO labTech, int userId)
        {
            // 1️⃣ Check user exists
            var users = await _userRepo.GetAllAsync("Users");
            if (!users.Any(u => u.UserId == userId))
                return new ApiResponse<bool>(404, "User not found", false);

            // 2️⃣ Prepare new lab technician
            var newLabTech = new LabTechnicians
            {

                UserId=userId,
                DepartmentId = labTech.DepartmentId,
                LicenceNumber = labTech.LicenceNumber,
                ContactNumber = labTech.ContactNumber,
                ProfilePhoto = labTech.ProfilePhoto,
                
            };

            // 3️⃣ Insert
            int rowsAffected = await _LabTechnicianRepo.AddAsync("SP_LABTECH", newLabTech);

            return new ApiResponse<bool>(
                rowsAffected > 0 ? 201 : 400,
                rowsAffected > 0 ? "Lab technician registered successfully" : "Insert failed",
                rowsAffected > 0
            );
        }

    }
}
