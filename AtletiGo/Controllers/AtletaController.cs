using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Services.Atleta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AtletiGo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AtletaController : ControllerBase
    {
        private readonly ILogger<AtletaController> _logger;

        private readonly AtletaService _atletaService;

        public AtletaController(ILogger<AtletaController> logger, AtletaService atletaService)
        {
            _logger = logger;
            _atletaService = atletaService;
        }

        [HttpPost("SalvarAtleta")]
        public IActionResult Criar([FromBody] SalvarModalidadeAtletaRequest request)
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();

                _atletaService.SalvarModalidadeAtleta(codigoUsuario, request);

                return Ok();
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
