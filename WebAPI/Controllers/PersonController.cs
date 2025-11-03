using BL.Constants;
using BL.Dtos;
using BL.Services.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IAuthentication _personService;
        public PersonController(IAuthentication personService)
        {
            _personService = personService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ResponsePersonDto>>> GetAllPerson()
        {
            try
            {
                var people = await _personService.GetAllAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponsePersonDto>> GetPersonById(int id)
        {
            try
            {
                var person = await _personService.GetByIdAsync(id);
                if (person == null)                
                    return NotFound();
                
                return Ok(person);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponsePersonDto>> RegisterPerson(RegisterPersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (person, token) = await _personService.RegisterAsync(dto);
                var response = new
                {
                    Person = person,
                    Token = token
                };

                return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, response);
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

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponsePersonDto>> LoginPerson(LoginPersonDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                (var person, var token) = await _personService.LoginAsync(dto);
                var response = new
                {
                    Person = person,
                    Token = token
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [Authorize(Roles = Roles.User)]
        [HttpPut("[action]")]
        public async Task<ActionResult<ChangePasswordDto>> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                var passwordChanged = await _personService.PasswordChangeAsync(dto);
                return Ok(passwordChanged);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPut("[action]")]

        public async Task<ActionResult> EditPerson(int id, EditPersonDto dto)
        {
            try
            {
                var passwordChanged = await _personService.EditAsync(id, dto);
                return Ok(passwordChanged);
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
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                bool success = await _personService.DeleteAsync(id);
                if (!success)
                    return NotFound();
                
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
