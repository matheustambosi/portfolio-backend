using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Evento;
using AtletiGo.Core.Services.Evento;
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
    public class EventoController : ControllerBase
    {
        private readonly ILogger<EventoController> _logger;

        private readonly EventoService _eventoService;

        public EventoController(ILogger<EventoController> logger, EventoService eventoService)
        {
            _logger = logger;
            _eventoService = eventoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetAllEventoResponse>> GetAll()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                var response = _eventoService.GetAllEventos(codigoUsuario, codigoAtletica.GetValueOrDefault());

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

        [HttpPost]
        public IActionResult CriarAtletica(CriarEventoRequest request)
        {
            try
            {
                var codigoAtletica = GetCodigoAtletica();

                if (codigoAtletica == null)
                    throw new AtletiGoException("Atlética inválida.");

                _eventoService.CriarEvento(request, codigoAtletica.Value);

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

        [HttpPut("{codigo}")]
        public IActionResult Editar([FromBody] CriarEventoRequest request, Guid codigo)
        {
            try
            {
                _eventoService.EditarEvento(codigo, request);

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
                var codigoUsuario = GetCodigoUsuario();

                _eventoService.DesativarEvento(codigo, codigoUsuario);

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
