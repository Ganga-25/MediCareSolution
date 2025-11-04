using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTechnicianDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class LabTechnicianCredentialService : ILabTechnicianCredentialService
    {
        private readonly IGenericRepository<LabTechnicians> _labtechRepo;
        private readonly IGenericRepository<LabTechnicicanCredential> _labtechcredRepo;

        public LabTechnicianCredentialService(IGenericRepository<LabTechnicians> labtechRepo, IGenericRepository<LabTechnicicanCredential> labtechcredRepo)
        {
            _labtechRepo = labtechRepo;
            _labtechcredRepo = labtechcredRepo;

        }
        public async Task<ApiResponse<string>> Addasync(AddLabtechCredentialDTO dto, int userId)
        {
            try
            {
                var labtechnicians = await _labtechRepo.GetAllAsync("SP_LABTECH");
                var labtechnician = labtechnicians.SingleOrDefault(x => x.UserId == userId);
                if (labtechnician == null) return new ApiResponse<string>(404, "Labtechnician not found.");

                var newlabtechniciancredential = new LabTechnicicanCredential
                {
                    LabTechnicianId = labtechnician.LabTechnicianId,
                    CredentialType = dto.CredentialType,
                    DegreeType = dto.DegreeType,
                    DegreeName = dto.DegreeName,
                    InstitutionName = dto.InstitutionName,
                    LabName = dto.LabName,
                    Designation = dto.Designation,
                    DocumentType = dto.DocumentType,
                    UploadDocument = dto.UploadDocument,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                await _labtechcredRepo.AddAsync("SP_LABTECH_CREDENTIALS", newlabtechniciancredential);
                return new ApiResponse<string>(200, "Labtechnicians credential Added successfully.");


            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>> GetLabtechnicianCredentialsAsync(int userId)
        {
            try
            {
                var labtechnicians = await _labtechRepo.GetAllAsync("SP_LABTECH");
                var labtechnician = labtechnicians.SingleOrDefault(x => x.UserId == userId);
                if (labtechnician == null) return new ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>(404, "Labtechnician not found.");

                var Labtechcredentials = await _labtechcredRepo.GetAllAsync("SP_LABTECH_CREDENTIALS");
                var labtechniciancredential = Labtechcredentials.Where(x => x.LabTechnicianId == labtechnician.LabTechnicianId).ToList();

                if (!labtechniciancredential.Any())
                    return new ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>(404, "No credentials found for this lab technician.");

                var Credentials = labtechniciancredential.Select(x => new LabTechnicianCredentialDTO
                {
                    Id = x.Id,
                    LabTechnicianId = x.LabTechnicianId,
                    CredentialType=x.CredentialType,
                    DegreeType=x.DegreeType,
                    DegreeName=x.DegreeName,
                    InstitutionName=x.InstitutionName,                  
                    LabName = x.LabName,
                    Designation=x.Designation,
                    DocumentType=x.DocumentType,
                    UploadDocument=x.UploadDocument,
                    StartDate=x.StartDate,
                    EndDate=x.EndDate

                });
                return new ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>(200, "Labtechnician Credential Fetched Successfully", Credentials);

            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<LabTechnicianCredentialDTO>>(500, ex.Message);
            }

        }
    }
}