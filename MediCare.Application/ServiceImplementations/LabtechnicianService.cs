using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTechnicianDTO;
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
        private readonly IGenericRepository<LabTechnicicanCredential> _labtechCredRepo;

        public LabtechnicianService(IGenericRepository<LabTechnicians> labTechnicianRepo, IGenericRepository<Users> userRepo, IGenericRepository<LabTechnicicanCredential> labtechCredRepo)
        {
            _LabTechnicianRepo = labTechnicianRepo;
            _userRepo = userRepo;
            _labtechCredRepo = labtechCredRepo;

        }

        public async Task<ApiResponse<LabTechnicianDTO>> GetLabTechByUserIdAsync(int userId)
        {
            var LabTechs = (await _LabTechnicianRepo.GetAllAsync("SP_LABTECH"))
                                   .SingleOrDefault(x => x.UserId == userId);
            if (LabTechs == null)
                return new ApiResponse<LabTechnicianDTO>(404, $"LabTechnician with this id :{userId} not found. ");

            var dto = new LabTechnicianDTO
            {
                LabTechnicianId = LabTechs.LabTechnicianId,
                UserId = userId,
                DepartmentId = LabTechs.DepartmentId,
                LicenceNumber = LabTechs.LicenceNumber,
                ContactNumber = LabTechs.ContactNumber,
                ProfilePhoto = LabTechs.ProfilePhoto,
                VerificationStatus = LabTechs.VerificationStatus.ToString(),
                IsActive = LabTechs.IsActive


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

                UserId = userId,
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
        public async Task<ApiResponse<IEnumerable<LabTechnicianDTO>>> GetPendingVerificationLabtech()
        {
            try
            {
                var labtechnicians = await _LabTechnicianRepo.GetAllAsync("SP_LABTECH");
                var Labtechcredentials = await _labtechCredRepo.GetAllAsync("SP_LABTECH_CREDENTIALS");
                var filter = labtechnicians
                           .Where(l => l != null
                                     && l.VerificationStatus == Veri_Status.Pending
                                     && Labtechcredentials.Any(c => c != null && c.LabTechnicianId == l.LabTechnicianId));
                if (!filter.Any()) return new ApiResponse<IEnumerable<LabTechnicianDTO>>(404, "No Labtechnician found for Verification.");

                var dtoList = filter.Select(i => new LabTechnicianDTO
                {
                    LabTechnicianId = i.LabTechnicianId,
                    UserId = i.UserId,
                    DepartmentId = i.DepartmentId,
                    LicenceNumber = i.LicenceNumber,
                    ProfilePhoto = i.ProfilePhoto,
                    VerificationStatus = i.VerificationStatus.ToString(),
                    IsActive = i.IsActive,
                    ContactNumber = i.ContactNumber,
                    Credentials = Labtechcredentials
                                     .Where(c => c != null && c.LabTechnicianId == i.LabTechnicianId)
                                     .Select(c => new LabTechnicianCredentialDTO
                                     {
                                         Id = c.Id,
                                         LabTechnicianId = c.LabTechnicianId,
                                         CredentialType = c.CredentialType,
                                         DegreeType = c.DegreeType,
                                         DegreeName = c.DegreeName,
                                         InstitutionName = c.InstitutionName,
                                         LabName = c.LabName,
                                         Designation = c.Designation,
                                         DocumentType = c.DocumentType,
                                         UploadDocument = c.UploadDocument,
                                         StartDate = c.StartDate,
                                         EndDate = c.EndDate
                                     }).ToList()

                }).ToList();

                return new ApiResponse<IEnumerable<LabTechnicianDTO>>(200, "Pending Labtechnician with credentials fetched successfully", dtoList);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<LabTechnicianDTO>>(500, ex.Message);
            }

        }
        public async Task<ApiResponse<string>> UpdateLabtechnicianVerficationStatus(UpdateverificationstatusofLabtechDTO dto, int userId)
        {
            try
            {
                var labtechnicians = await _LabTechnicianRepo.GetByIdAsync("SP_LABTECH", "LABTECHNICIANID", dto.LabTechnicianId);
                if (labtechnicians == null) return new ApiResponse<string>(404, "Invalid labtechnicianId");

                var labtechniciancredentials = (await _labtechCredRepo.GetAllAsync("SP_LABTECH_CREDENTIALS")).Where(l=>l.LabTechnicianId==dto.LabTechnicianId);
                if (labtechniciancredentials == null) return new ApiResponse<string>(404, "Labtechnician have no credentials.");

                    var Labtechentity = new LabTechnicians
                    {
                        LabTechnicianId = dto.LabTechnicianId,
                        VerificationStatus = dto.VerificationStatus,
                        ModifiedBy = userId
                    };

                await _LabTechnicianRepo.UpdateAsync("SP_LABTECH", Labtechentity);
                return new ApiResponse<string>(200, "Labtechnicians VerificationStatus Updated");


            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
    
    } 
}
