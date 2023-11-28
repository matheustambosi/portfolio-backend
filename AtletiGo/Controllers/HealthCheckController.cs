using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AtletiGo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly ILogger<HealthCheckController> _logger;
        private readonly string _connString;

        public HealthCheckController(ILogger<HealthCheckController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_connString);
        }
    }
}
