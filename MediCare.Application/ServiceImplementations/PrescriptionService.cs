using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.PrescriptionDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<Prescription> _prescriptionRepo;
        public PrescriptionService(IGenericRepository<Appointment> appointmentRepo, IGenericRepository<Prescription> prescriptionRepo)
        {
            _appointmentRepo = appointmentRepo;
            _prescriptionRepo = prescriptionRepo;
        }
        public async Task<ApiResponse<string>> AddPrescription(AddPresciptionDTO addPresciptionDTO, int doctorId,string UserRole)
        {
            try
            {
                var appointment = await _appointmentRepo.GetByIdAsync("SP_APPOINTMENT", "APPOINTMENTID", addPresciptionDTO.AppointmentId);
                if (appointment == null) return new ApiResponse<string>(404, "Appointmentnot found");

                if (appointment.DoctorId != doctorId) return new ApiResponse<string>(403, "Unauthorized access — doctor mismatch");

                var prescription = new Prescription
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId = doctorId,
                    PrescriptionDate = addPresciptionDTO.PrescriptionDate,
                    Summary = addPresciptionDTO.Summary,
                    Medicine = addPresciptionDTO.Medicine,
                    Dosage = addPresciptionDTO.Dosage,
                    NoOfDays = addPresciptionDTO.NoOfDays,
                    Instructions = addPresciptionDTO.Instructions,
                    CreatedBy=UserRole
                };
                return new ApiResponse<string>(201, "Prescription Added Successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
    }
}

