using BL.Constants;
using BL.Dtos;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly ContractorService _contractorService;
        private readonly LogService _logService;
        public ContractorController(ContractorService contractorService, LogService logService)
        {
            _contractorService = contractorService;
            _logService = logService;
        }


        [Authorize(Roles = Roles.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseContractorDto>> CreateContractor(CreateContractorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdContractor = await _contractorService.CreateAsync(dto);

                await _logService.CreateLog(2, $"Contractor successfully created");

                return CreatedAtAction(nameof(GetContractorById), new { createdContractor.Id }, createdContractor);
            }
            catch (KeyNotFoundException ex)
            {
                await _logService.CreateLog(4, $"Not found error while creating contractor");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _logService.CreateLog(4, $"Invalid operation error while creating contractor");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while creating contractor");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteContractor(int id)
        {
            try
            {
                bool contractor = await _contractorService.DeleteAsync(id);
                if (!contractor)
                    return NotFound();

                await _logService.CreateLog(3, $"Contractor successfully deleted");

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                await _logService.CreateLog(4, $"Not found error while deleting contractor");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while deleting contractor");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ResponseContractorDto>>> GetAllContractors()
        {
            try
            {
                var contractors = await _contractorService.GetAllAsync();
                await _logService.CreateLog(1, $"All contractors successfully retrieved");

                return Ok(contractors);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while retrieving all contractors");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<ResponseContractorDto>>> GetPagedContractors(int page, int size)
        {
            try
            {
                var pagedResult = await _contractorService.GetPagedAsync(page, size);
                await _logService.CreateLog(1, $"Contractors page {page} with size {size} successfully retrieved.");

                return Ok(pagedResult);

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while retrieving contractors page: {page} with size: {size}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<ResponseContractorDto>> GetContractorById(int id)
        {
            try
            {
                var contractor = await _contractorService.GetByIdAsync(id);
                if (contractor == null)
                    return NotFound();

                await _logService.CreateLog(1, "Contractor successfully retrieved");

                return Ok(contractor);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while retrieving contractor");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
               

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateContractor(int id, EditContractorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool contractor = await _contractorService.EditAsync(id, dto);
                if (!contractor)
                    return NotFound();

                await _logService.CreateLog(2, "Contractor successfully updated");

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                await _logService.CreateLog(4, $"Not found error while updating contractor");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                await _logService.CreateLog(4, $"Invalid operation error while updating contractor");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(5, $"Unexpected error while updating contractor");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
