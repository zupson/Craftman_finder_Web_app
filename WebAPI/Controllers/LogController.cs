using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI_ForRWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;
        public LogController(LogService logService)
        {
            _logService = logService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogsAsync()
        {
            try
            {
                var logs =  await _logService.GetAllLogs();
                return Ok(logs);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
