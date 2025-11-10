using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.CredentialDTO;
using MediCare.Application.DTOs.ProfileUpdateDTO;
using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Users> _userRepo;
        private readonly IGenericRepository<Doctors> _doctorRepo;
        private readonly IGenericRepository<DoctorCredential> _doctorCredRepo;
        public DoctorService(IGenericRepository<Users> userRepo, IGenericRepository<Doctors> doctorRepo, IGenericRepository<DoctorCredential> doctorCredRepo)
        {
            _userRepo = userRepo;
            _doctorRepo = doctorRepo;
            _doctorCredRepo= doctorCredRepo;
        }


        public async Task<ApiResponse<IEnumerable<DoctorDTO>>> GetAllAsync()
        {
            var result = await _doctorRepo.GetAllAsync("SP_DOCTORS");
            var dtoList = result.Select(labTest => new DoctorDTO
            {
               DoctorId=labTest.DoctorId,
               UserId=labTest.UserId,
               DepartmentId=labTest.DepartmentId,
               ProfilePhoto=labTest.ProfilePhoto,
               ContactNumber=labTest.ContactNumber,
               MedicalRegistrationNumber=labTest.MedicalRegistrationNumber,
               Fees=labTest.Fees,
               Experience=labTest.Experience,
               VerificationStatus=labTest.VerificationStatus,
               IsAvailable=labTest.IsAvailable
            }).ToList();

            return new ApiResponse<IEnumerable<DoctorDTO>>(200, "Doctors fetched successfully", dtoList);

        }

        public async Task<ApiResponse<DoctorDTO>> GetByIdAsync(int id)
        {
            var result = (await _doctorRepo.GetAllAsync("SP_DOCTORS"))
                                 .FirstOrDefault(t => t.DoctorId == id);

            if (result == null)
                return new ApiResponse<DoctorDTO>(404, "Doctor  not found");
            var dto = new DoctorDTO
            {
                DoctorId=result.DoctorId,
                UserId=result.UserId,
                DepartmentId=result.DepartmentId,
                ProfilePhoto=result.ProfilePhoto,
                ContactNumber=result.ContactNumber,
                MedicalRegistrationNumber=result.MedicalRegistrationNumber,
                Fees=result.Fees,
                Experience=result.Experience,
                VerificationStatus=result.VerificationStatus,
                IsAvailable=result.IsAvailable


                
            };


            return new ApiResponse<DoctorDTO>(200, "Doctor  fetched successfully", dto);
        }


        public async Task<ApiResponse<bool>> RegisterDoctorAsync(DoctorUpdateDTO Docdto, int userId)
        {
            // 1️⃣ Check user exists
            var users = await _userRepo.GetAllAsync("Users");
            if (!users.Any(u => u.UserId == userId))
                return new ApiResponse<bool>(404, "User not found", false);

            // 2️⃣ Prepare new lab technician
            var newDoctor = new Doctors
            {

                UserId = userId,
                DepartmentId = Docdto.DepartmentId,
                ContactNumber = Docdto.ContactNumber,
                MedicalRegistrationNumber = Docdto.MedicalRegistrationNumber,
                Fees = Docdto.Fees,
                Experience = Docdto.Experience,
                ProfilePhoto = Docdto.ProfilePhoto


            };

            // 3️⃣ Insert
            int rowsAffected = await _doctorRepo.AddAsync("SP_DOCTORS", newDoctor);

            return new ApiResponse<bool>(
                rowsAffected > 0 ? 201 : 400,
                rowsAffected > 0 ? "Doctor registered successfully" : "Insert failed",
                rowsAffected > 0
            );
        }

        public async Task<ApiResponse<IEnumerable<DoctorDTO>>> GetDoctorsByDepartmentAsync(int departmentId)
        {
            // Step 1: Get all doctors from DB via existing SP
            var allDoctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");

            // Step 2: Filter by departmentId
            var filteredDoctors = allDoctors
                                    .Where(d => d.DepartmentId == departmentId)
                                    .ToList();

            // Step 3: Return response
            if (!filteredDoctors.Any())
                return new ApiResponse<IEnumerable<DoctorDTO>>(404, "No doctors found in this department");

            var doctorDTOs = filteredDoctors.Select(d => new DoctorDTO
            {
                DoctorId = d.DoctorId,
                UserId = d.UserId,
                DepartmentId = d.DepartmentId,
                ProfilePhoto = d.ProfilePhoto,
                ContactNumber = d.ContactNumber,
                MedicalRegistrationNumber = d.MedicalRegistrationNumber,
                Fees = d.Fees,
                Experience = d.Experience,
                VerificationStatus = d.VerificationStatus,
                IsAvailable = d.IsAvailable
            }).ToList();

            return new ApiResponse<IEnumerable<DoctorDTO>>(200, "Doctors fetched successfully", doctorDTOs);
        }

        public async Task<ApiResponse<IEnumerable<DoctorDTO>>> GetPendingDoctorsAsync()
        {
            try
            {
                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                if (doctors == null) throw new Exception("No doctors returned from repository");

                var allCredentials = await _doctorCredRepo.GetAllAsync("SP_DOC_CREDENTIALS");
                if (allCredentials == null) throw new Exception("No doctor credentials returned from repository");

                // Safe ToList conversions
                var doctorList = doctors.ToList();
                var credentialList = allCredentials.ToList();

                var filtered = doctorList
                    .Where(d => d != null
                             && d.VerificationStatus == Veri_Status.Pending
                             && credentialList.Any(c => c != null && c.DoctorId == d.DoctorId))
                    .ToList();

                if (!filtered.Any()) return new ApiResponse<IEnumerable<DoctorDTO>>(404, "No doctors found for pending verification");

                var dtoList = filtered.Select(d => new DoctorDTO
                {
                    DoctorId = d.DoctorId,
                    UserId = d.UserId,
                    DepartmentId = d.DepartmentId,
                    ProfilePhoto = d.ProfilePhoto,
                    ContactNumber = d.ContactNumber,
                    MedicalRegistrationNumber = d.MedicalRegistrationNumber,
                    Fees = d.Fees,
                    Experience = d.Experience,
                    VerificationStatus = d.VerificationStatus,
                    IsAvailable = d.IsAvailable,
                    Credentials = credentialList
                                .Where(c => c != null && c.DoctorId == d.DoctorId)
                                .Select(c => new DoctorCredentialDTO
                                {
                                    Id = c.Id,
                                    DoctorId = d.DoctorId,
                                    CredentialType = c.CredentialType,
                                    DegreeType = c.DegreeType,
                                    DegreeName = c.DegreeName,
                                    InstitutionName = c.InstitutionName,
                                    HospitalName = c.HospitalName,
                                    Designation = c.Designation,
                                    DocumentType = c.DocumentType,
                                    StartDate = c.StartDate,
                                    EndDate = c.EndDate,
                                    UploadDocument = c.UploadDocument
                                }).ToList()
                }).ToList();

                return new ApiResponse<IEnumerable<DoctorDTO>>(200, "Pending doctors with credentials fetched successfully", dtoList);


            }
            catch (Exception ex) 
            { 
                return new ApiResponse<IEnumerable<DoctorDTO>>(500, ex.Message);
            }

        }

        public async Task<ApiResponse<bool>> UpdateDoctorVerificationStatusAsync(DoctorVerificationUpdateDTO dto,int userId)
        {
            try
            {
                var doctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "DOCTORID", dto.DoctorId);
                if (doctor == null) return new ApiResponse<bool>(404, "Doctor id is not found.");
                var doctorCredential = (await _doctorCredRepo.GetAllAsync("SP_DOC_CREDENTIALS")).Where(d => d.DoctorId== dto.DoctorId);
                if (doctorCredential == null) return new ApiResponse<bool>(404, "Doctor doesnot have any credentials");
                var doctorEntity = new Doctors
                {
                    DoctorId = dto.DoctorId,
                    VerificationStatus = dto.VerificationStatus,
                    ModifiedBy = userId,

                };

                int rowsAffected = await _doctorRepo.UpdateAsync("SP_DOCTORS", doctorEntity);

                return new ApiResponse<bool>(
                     rowsAffected > 0 ? 201 : 400,
                     rowsAffected > 0 ? "Doctor status updated successfully" : "Failed to update",
                     rowsAffected > 0
                 );

            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>(500,ex.Message);
            }
            
        }

    }
}

