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
        private readonly IGenericRepository<Doctors> _doctorRepo;
        public PrescriptionService(IGenericRepository<Appointment> appointmentRepo, IGenericRepository<Prescription> prescriptionRepo,IGenericRepository<Doctors> doctorRepo)
        {
            _appointmentRepo = appointmentRepo;
            _prescriptionRepo = prescriptionRepo;
            _doctorRepo = doctorRepo;
        }
        public async Task<ApiResponse<string>> AddPrescription(AddPresciptionDTO addPresciptionDTO, int userId,string UserRole)
        {
            try
            {
                var appointment = await _appointmentRepo.GetByIdAsync("SP_APPOINTMENT", "APPOINTMENTID", addPresciptionDTO.AppointmentId);
                if (appointment == null) return new ApiResponse<string>(404, "Appointmentnot found");

                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                var doctor= doctors.SingleOrDefault(x=>x.UserId == userId);
                if (doctor == null) return new ApiResponse<string>(404, "Doctor not found");

                if (appointment.DoctorId != doctor.DoctorId) return new ApiResponse<string>(403, "Unauthorized access — doctor mismatch");

                var prescription = new Prescription
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    DoctorId =appointment.DoctorId,
                    PrescriptionDate = addPresciptionDTO.PrescriptionDate,
                    Summary = addPresciptionDTO.Summary,
                    Medicine = addPresciptionDTO.Medicine,
                    Dosage = addPresciptionDTO.Dosage,
                    NoOfDays = addPresciptionDTO.NoOfDays,
                    Instructions = addPresciptionDTO.Instructions,
                    CreatedBy=UserRole
                };
                await _prescriptionRepo.AddAsync("SP_PRESCRIPTION",prescription);
                return new ApiResponse<string>(201, "Prescription Added Successfully");

            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
    }
}

