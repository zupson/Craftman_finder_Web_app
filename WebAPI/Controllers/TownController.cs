using BL.Constants;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownController : ControllerBase
    {
        private readonly TownService _townService;
        public TownController(TownService townService)
        {
            _townService = townService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseTownDto>> CreateTown(CreateTownDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var createdTown = await _townService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetTownById), new { createdTown.Id }, createdTown);
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
        public async Task<IActionResult> DeleteTown(int id)
        {
            try
            {
                var town = await _townService.DeleteAsync(id);
                if (!town)

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
        public async Task<ActionResult<IEnumerable<ResponseTownDto>>> GetAllTowns()
        {
            try
            {
                var towns = await _townService.GetAllAsync();
                return Ok(towns);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseTownDto>> GetTownById(int id)
        {
            try
            {
                var town = await _townService.GetByIdAsync(id);
                if (town == null)
                    return NotFound();

                return Ok(town);
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
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateTown(int id, EditTownDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var town = await _townService.EditAsync(id, dto);
                if (!town)
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
