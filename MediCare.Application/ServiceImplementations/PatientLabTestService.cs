using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestDTO;
using MediCare.Application.DTOs.PrescriptionDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediCare.Application.ServiceImplementations
{
    public class PatientLabTestService : IPatientLabTestService
    {
        private readonly IGenericRepository<PatientLabTest> _patientLabTest;
        private readonly IGenericRepository<Doctors> _doctorRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<LabTests> _labtestRepo;
        private readonly IGenericRepository<Patients> _patientRepo;

        public PatientLabTestService(IGenericRepository<PatientLabTest> patientLabTest, IGenericRepository<Doctors> doctorRepo, IGenericRepository<Appointment> appointmentRepo, IGenericRepository<LabTests> labtesrRepo,IGenericRepository<Patients> patientRepo)
        {
            _patientLabTest = patientLabTest;
            _doctorRepo = doctorRepo;
            _appointmentRepo = appointmentRepo;
            _labtestRepo = labtesrRepo;
            _patientRepo = patientRepo;
        }

        public async Task<ApiResponse<string>> AddLabtestToPatient(AddPatientLabTestDTO labTestDTO, int userId, string userRole)
        {
            try
            {
                var appointment = await _appointmentRepo.GetByIdAsync("SP_APPOINTMENT", "APPOINTMENTID", labTestDTO.AppointmentId);
                if (appointment == null) return new ApiResponse<string>(404, "Appointmentnot found");

                var doctors = await _doctorRepo.GetAllAsync("SP_DOCTORS");
                var doctor = doctors.SingleOrDefault(x => x.UserId == userId);
                if (doctor == null) return new ApiResponse<string>(404, "Doctor not found");

                if (appointment.DoctorId != doctor.DoctorId) return new ApiResponse<string>(403, "Unauthorized access — doctor mismatch");
                var labtests = await _labtestRepo.GetAllAsync("SP_LABTEST");
                var existinglabtest = labtests.SingleOrDefault(x => x.TestName.Trim() == labTestDTO.TestName.Trim());
                bool labtest = existinglabtest != null;

                var labTest = new PatientLabTest
                {
                    AppointmentId = labTestDTO.AppointmentId,
                    TestName = labTestDTO.TestName,
                    DepartmentId = doctor.DepartmentId.Value,
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    IsInHouse = labtest,
                    CreatedBy = userRole

                };
                await _patientLabTest.AddAsync("SP_PATIENTLABTEST", labTest);


                return new ApiResponse<string>(201, "Patient lab test added successfully.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
        public async Task<ApiResponse<IEnumerable<LabtestViewDTO>>> GetLabTestforPatient(int userId)
        {
            try
            {
                var patients = await _patientRepo.GetAllAsync("SP_Patients");
                var patient = patients.SingleOrDefault(x => x.UserId == userId);
                if (patient == null) return new ApiResponse<IEnumerable<LabtestViewDTO>>(404, "Patient not found");

                var labtests = await _patientLabTest.GetAllAsync("SP_PATIENTLABTEST");
                var labtest = labtests.Where(x => x.PatientId == patient.PatientId).ToList();

                var tests = labtest.Select(x => new LabtestViewDTO
                {
                    Id = x.Id,
                    AppointmentId = x.AppointmentId,
                    PatientId = x.PatientId,
                    TestName = x.TestName,
                    DepartmentId = x.DepartmentId,
                    DoctorId = x.DoctorId,
                    IsInHouse = x.IsInHouse,
                    Status = x.Status


                }).ToList();

                return new ApiResponse<IEnumerable<LabtestViewDTO>>(200, "Patient LabTests.",tests);


            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<LabtestViewDTO>>(500,ex.Message);
            }
        }
    }
}
