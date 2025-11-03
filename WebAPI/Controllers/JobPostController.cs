using BL.Constants;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostController : ControllerBase
    {
        private readonly JobPostService _jobPostService;
        public JobPostController(JobPostService jobPostService)
        {
            _jobPostService = jobPostService;
        }
        
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseJobPostDto>> CreateJobPost(CreateJobPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdLocation = await _jobPostService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetJobPostById), new { createdLocation.Id }, createdLocation);
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
        public async Task<IActionResult> DeleteJobPost(int id)
        {
            try
            {
                bool location = await _jobPostService.DeleteAsync(id);
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
        public async Task<ActionResult<IEnumerable<ResponseJobPostDto>>> GetAllJobPosts()
        {
            try
            {
                var locations = await _jobPostService.GetAllAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseJobPostDto>> GetJobPostById(int id)
        {
            try
            {
                var location = await _jobPostService.GetByIdAsync(id);
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
        public async Task<IActionResult> UpdateJobPost(int id, EditJobPostDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool location = await _jobPostService.EditAsync(id, dto);
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
