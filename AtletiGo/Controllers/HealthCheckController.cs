using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Evento;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Services.Evento;
using AtletiGo.Core.Services.Usuario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;

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
