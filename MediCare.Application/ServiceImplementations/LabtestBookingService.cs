using MediCare.Application.Common;
using MediCare.Application.Contracts.Repository;
using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestBooking;

using MediCare.Domain.Entities;
using MediCare.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediCare.Application.ServiceImplementations
{
    public class LabTestBookingService : ILabtestBookingService
    {
        private readonly IGenericRepository<PatientLabTest> _patientLabTestRepo;
        private readonly IGenericRepository<StaffAvailability> _staffAvailabilityRepo;
        private readonly IGenericRepository<LabTestBooking> _labTestBookingRepo;
        private readonly IGenericRepository<LabTests> _labtestRepo;
        public LabTestBookingService(
            IGenericRepository<PatientLabTest> patientLabTestRepo,
            IGenericRepository<StaffAvailability> staffAvailabilityRepo,
            IGenericRepository<LabTestBooking> labTestBookingRepo,
            IGenericRepository<LabTests> labtestRepo)
        {
            _patientLabTestRepo = patientLabTestRepo;
            _staffAvailabilityRepo = staffAvailabilityRepo;
            _labTestBookingRepo = labTestBookingRepo;
            _labtestRepo = labtestRepo;
        }

        public async Task<ApiResponse<string>> BookLabTestAsync(LabTestBookingDTO dto, int userId)
        {
            try
            {
                // 1️⃣ Check if lab test exists
                var labtest = await _patientLabTestRepo.GetByIdAsync("SP_PATIENTLABTEST", "ID", dto.LabTestId);
                if (labtest == null)
                    return new ApiResponse<string>(404, "Lab test not found");
                var test = await _labtestRepo.GetByIdAsync("SP_LABTEST", "LABTESTID", dto.LabTestId);
                if (test == null) return new ApiResponse<string>(404, "Labtest not found.");

                // 2️⃣ Ensure test is available in-house
                if (!labtest.IsInHouse)
                    return new ApiResponse<string>(400, "Selected test is not available in the lab");

                // 3️⃣ Check for available slots
                var staffAvailabilities = await _staffAvailabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                var availableSlot = staffAvailabilities.FirstOrDefault(a =>
                    a.Date.Date == dto.LabTestDate.Date &&
                    a.Session == dto.LabTestTime.ToString() &&
                    a.AvailableSlots > 0 &&
                    a.StaffType ==StaffType.LabTechnician);

                if (availableSlot == null)
                    return new ApiResponse<string>(400, "No available slots for selected date or time");

                // 4️⃣ Check if the same user already booked same test and not completed
                var allBookings = await _labTestBookingRepo.GetAllAsync("SP_LABTESTBOOKING");
                var duplicate = allBookings.FirstOrDefault(b =>
                    b.PatientId == labtest.PatientId &&
                    b.LabTestId == labtest.Id &&
                    (b.Status == LabTestStatus.Scheduled));

                if (duplicate != null)
                    return new ApiResponse<string>(400, "You already have a booking for this test that is not yet completed");

                // 5️⃣ Create new booking record
                var newBooking = new LabTestBooking
                {
                    PatientId = labtest.PatientId,
                    DoctorId = labtest.DoctorId,
                    DepartmentId = labtest.DepartmentId,
                    LabTestId = labtest.Id,
                    LabTechnicianId = availableSlot.StaffId,
                    Amount=test.Price,
                    LabTestDate = dto.LabTestDate,
                    LabTestTime = dto.LabTestTime,
                    Status = LabTestStatus.Scheduled,
                    PaymentStatus = PaymentStatus.Pending,
                    CreatedBy = userId.ToString(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };

                var inserted = await _labTestBookingRepo.AddAsync("SP_LABTESTBOOKING", newBooking);
                if (inserted <= 0)
                    return new ApiResponse<string>(400, "Failed to book lab test");

                // 6️⃣ Reduce available slots
                availableSlot.AvailableSlots -= 1;
                await _staffAvailabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", availableSlot);

                return new ApiResponse<string>(200, "Lab test booked successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }

        public async Task<ApiResponse<string>> CancelBookingAsync(int bookingId, int userId)
        {
            try
            {
                var booking = await _labTestBookingRepo.GetByIdAsync("SP_LABTESTBOOKING", "ID", bookingId);
                if (booking == null)
                    return new ApiResponse<string>(404, "Booking not found");

                if (booking.Status == LabTestStatus.Cancelled)
                    return new ApiResponse<string>(400, "Cannot cancel a completed booking");

                booking.Status = LabTestStatus.Cancelled;
                booking.PaymentStatus = PaymentStatus.Failed;
                booking.ModifiedBy = userId;
               

                await _labTestBookingRepo.UpdateAsync("SP_LABTESTBOOKING", booking);

                //  Reopen slot
                var staffAvailabilityList = await _staffAvailabilityRepo.GetAllAsync("SP_STAFF_AVAILABILITY");
                var slot = staffAvailabilityList.FirstOrDefault(a =>
                    a.StaffId == booking.LabTechnicianId &&
                    a.Date == booking.LabTestDate &&
                    a.Session == booking.LabTestTime.ToString());

                if (slot != null)
                {
                    slot.AvailableSlots += 1;
                    await _staffAvailabilityRepo.UpdateAsync("SP_STAFF_AVAILABILITY", slot);
                }

                return new ApiResponse<string>(200, "Booking cancelled successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, ex.Message);
            }
        }
    }
}
