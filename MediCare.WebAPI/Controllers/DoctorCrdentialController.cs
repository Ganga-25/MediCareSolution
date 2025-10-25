using MediCare.Application.Contracts.Service;
using MediCare.Application.DTOs.CredentialDTO;
using MediCare.Application.ServiceImplementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediCare.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorCrdentialController : ControllerBase
    {
        private readonly IDoctorCredentialService _DocCredService;

        public DoctorCrdentialController (IDoctorCredentialService docCredService)
        {
            _DocCredService = docCredService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _DocCredService.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/DoctorCredential/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _DocCredService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/DoctorCredential
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddDoctorCredentialDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _DocCredService.AddAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/DoctorCredential/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorCredentialDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest(new { Message = "Id in URL and payload do not match" });

            var response = await _DocCredService.UpdateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{doctorId}/credentials")]
        public async Task<IActionResult> GetDoctorCredentials(int doctorId)
        {
            var response = await _DocCredService.GetDoctorCredentialsAsync(doctorId);
            return StatusCode(response.StatusCode, response);
        }


    }
}

