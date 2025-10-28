using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.AppointmentDTO;
using MediCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<StaffAvailability> _availabilityRepo;
        private readonly IGenericRepository<Appointment> _appointmentRepo;
        private readonly IGenericRepository<Doctors> _doctorRepo;


        public AppointmentService(IGenericRepository<StaffAvailability> availabilityRepo, IGenericRepository<Appointment> appointmentRepo, IGenericRepository<Doctors> doctorRepo)
        {
            _availabilityRepo = availabilityRepo;
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
        }
        public async Task<ApiResponse<string>> BookAppointmentAsync(AppointmentDTO dto)
        {
            var doctor = await _doctorRepo.GetByIdAsync("SP_DOCTORS", "@DOCTORID", dto.DoctorId);
            if (doctor == null)
                return new ApiResponse<string>(404, "Doctor not found.");

            // 1️⃣ Get doctor availability (StaffId == Doctor.UserId)
            var allAvailabilities = await _availabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
            var doctorAvailability = allAvailabilities
                .FirstOrDefault(a => a.StaffId == doctor.UserId && a.Date.Date == dto.AppointmentDate.Date);

            if (doctorAvailability == null)
                return new ApiResponse<string>(400, "No availability found for the selected doctor/date.");

            // 2️⃣ Parse slot list
            var slotList = doctorAvailability.SlotList?.Split(',')
                            .Select(s => s.Trim())
                            .ToList() ?? new List<string>();

            // 3️⃣ Check if the selected slot exists
            // Ensure the string (like "06:00 PM") is parsed correctly
            var selectedSlot = DateTime.ParseExact(dto.AppointmentTime.Trim(), "hh:mm tt", null)
                                        .ToString("hh:mm tt");
            if (!slotList.Contains(selectedSlot))
                return new ApiResponse<string>(400, "Invalid slot. Doctor not available at that time.");

            // 4️⃣ Check if already booked
            var allAppointments = await _appointmentRepo.GetAllAsync("SP_APPOINTMENTS");
            bool isAlreadyBooked = allAppointments.Any(a =>
                a.DoctorId == doctor.UserId &&
                a.AppointmentDate.Date == dto.AppointmentDate.Date &&
                a.AppointmentTime == dto.AppointmentTime);

            if (isAlreadyBooked)
                return new ApiResponse<string>(400, "Slot already booked by another patient.");

            // 5️⃣ Save the new appointment
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                UHID = dto.UHID,
                DoctorId = doctor.UserId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = dto.AppointmentTime,
                Mode = dto.Mode,
                Notes = dto.Notes
            };

            await _appointmentRepo.AddAsync("SP_APPOINTMENTS", appointment);

            // 6️⃣ Decrease available slot count (don’t remove the slot string)
            if (doctorAvailability.AvailableSlots > 0)
                doctorAvailability.AvailableSlots--;

            await _availabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", doctorAvailability);

            return new ApiResponse<string>(200, "Appointment booked successfully.");
        }


    } }



