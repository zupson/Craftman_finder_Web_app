using BL.Constants;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        // POST api/<CountryController>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseCountryDto>> CreateCountry(CreateCountryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdCountry = await _countryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetCountryById), new { id = createdCountry.Id }, createdCountry);
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

        // DELETE api/<CountryController>/5
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            try
            {
                var deletedCountry = await _countryService.DeleteAsync(id);
                if (!deletedCountry)
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

        // GET: api/<CountryController>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ResponseCountryDto>>> GetAllCountries()
        {
            try
            {
                var countries = await _countryService.GetAllAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<CountryController>/5
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseCountryDto>> GetCountryById(int id)
        {
            try
            {
                var country = await _countryService.GetByIdAsync(id);
                if (country == null)
                    return NotFound();
                
                return Ok(country);
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

        // PUT api/<CountryController>/5
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateCountry(int id, EditCountryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var updatedCountry = await _countryService.EditAsync(id, dto);
                if (!updatedCountry)
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
