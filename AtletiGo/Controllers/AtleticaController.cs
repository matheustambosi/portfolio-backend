using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Services.Atletica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
            try
            {
                _atleticaService.Insert(atletica);

                return Ok();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(atEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Erro desconhecido");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Atletica>> GetAll()
        {
            try
            {
                var response = _atleticaService.GetAll();

                return Ok(response);
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(atEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Erro desconhecido");
            }
        }
    }
}
