using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.LabTestBooking;
using MediCare.Infrastructure.Extentions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTestBookingController : ControllerBase
    {
        private readonly ILabtestBookingService _labtestbookingService;
        public LabTestBookingController(ILabtestBookingService labtestbookingService)
        {
            _labtestbookingService = labtestbookingService;
        }
        [HttpPost("Booking")]
        public async Task<IActionResult> Booking([FromBody]LabTestBookingDTO dTO)
        {
             int userId=User.GetUserId();   
            var result= await _labtestbookingService.BookLabTestAsync(dTO, userId);
            return StatusCode(result.StatusCode, result);

        }
        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            int userId = 1; // get from auth later
            var result = await _labtestbookingService.CancelBookingAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
