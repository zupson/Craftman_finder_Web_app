using BL.Constants;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly JobApplicationService _jobApplicationService;

        public JobApplicationController(JobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseJobApplicationDto>> CreateJobApplication(CreateJobApplicationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdLocation = await _jobApplicationService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetJobApplicationById), new { createdLocation.Id }, createdLocation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteJobApplication(int id)
        {
            try
            {
                bool location = await _jobApplicationService.DeleteAsync(id);
                if (!location)
                    return NotFound();

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ResponseJobApplicationDto>>> GetAllJobApplications()
        {
            try
            {
                var locations = await _jobApplicationService.GetAllAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseJobApplicationDto>> GetJobApplicationById(int id)
        {
            try
            {
                var location = await _jobApplicationService.GetByIdAsync(id);
                if (location == null)
                    return NotFound();

                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateJobApplication(int id, EditJobApplicationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool location = await _jobApplicationService.EditAsync(id, dto);
                if (!location)
                    return NotFound();

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
    }
}
