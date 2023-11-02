using AtletiGo.Core.Entities;
using AtletiGo.Core.Services.Atletica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AtletiGo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AtleticaController : ControllerBase
    {
        private readonly ILogger<AtleticaController> _logger;
        private readonly AtleticaService _atleticaService;

        public AtleticaController(ILogger<AtleticaController> logger, AtleticaService atleticaService)
        {
            _logger = logger;
            _atleticaService = atleticaService;
        }

        [HttpPost]
        public IActionResult CriarAtletica(Atletica atletica)
        {
            _atleticaService.Insert(atletica);

            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Atletica>> GetAll()
        {
            var response = _atleticaService.GetAll();

            return Ok(response);
        }
    }
}
