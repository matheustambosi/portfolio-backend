using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using AtletiGo.Core.Services.Modalidade;
using AtletiGo.Core.Messaging.Modalidade;

namespace AtletiGo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ModalidadeController : ControllerBase
    {
        private readonly ILogger<ModalidadeController> _logger;

        private readonly ModalidadeService _modalidadeService;

        public ModalidadeController(ILogger<ModalidadeController> logger, ModalidadeService modalidadeService)
        {
            _logger = logger;
            _modalidadeService = modalidadeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetAllModalidadeResponse>> GetAll()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                var response = _modalidadeService.GetAll(codigoUsuario, codigoAtletica);

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
        public IActionResult Criar(CriarModalidadeRequest request)
        {
            try
            {
                var codigoAtletica = GetCodigoAtletica();

                if (codigoAtletica == null)
                    throw new AtletiGoException("Atlética inválida.");

                _modalidadeService.CadastrarModalidade(request, codigoAtletica.Value);

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

        [HttpPut("{codigo}")]
        public IActionResult Editar([FromBody] CriarModalidadeRequest request, Guid codigo)
        {
            try
            {
                _modalidadeService.EditarModalidade(codigo, request);

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
                _modalidadeService.DeletarModalidadeAtletica(codigo);

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

        [HttpGet("BuscandoAtletas")]
        public ActionResult<IEnumerable<GetAllModalidadesBuscandoAtletasResponse>> GetAllModalidadesBuscandoAtleta()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                if (codigoAtletica == null)
                    throw new AtletiGoException("Entre em uma atlética para continuar.");

                var response = _modalidadeService.GetAllModalidadesBuscandoAtletas(codigoUsuario, codigoAtletica.GetValueOrDefault());

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
    }
}
