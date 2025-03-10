using Microsoft.AspNetCore.Mvc;
using GestorUsuariosLog.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace GestorUsuariosLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogController : ControllerBase
    {
        // GET: api/log
        [HttpGet]
        public IActionResult GetLog()
        {
            try
            {
                // Lee el contenido del archivo de log
                var logContent = LogHelper.ReadLog();
                
                return Content(logContent, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al leer el log.", error = ex.Message });
            }
        }
    }
}
