using Microsoft.AspNetCore.Mvc;

namespace PondLite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "PondLite API is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
}