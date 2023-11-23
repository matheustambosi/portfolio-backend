using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Atletica;
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
        public IActionResult CriarAtletica(CriarAtleticaRequest request)
        {
            try
            {
                _atleticaService.CadastrarAtletica(request);

                return NoContent();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
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
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }

        [HttpPut("{codigo}")]
        public IActionResult Editar([FromBody] CriarAtleticaRequest request, Guid codigo)
        {
            try
            {
                _atleticaService.EditarAtletica(codigo, request);

                return NoContent();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }

        [HttpDelete("{codigo}")]
        public IActionResult Delete(Guid codigo)
        {
            try
            {
                _atleticaService.DesativarAtletica(codigo);

                return NoContent();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }
    }
}
