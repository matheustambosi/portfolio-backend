using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Services.QRCode;
using AtletiGo.Core.Services.Usuario;
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
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        private readonly UsuarioService _usuarioService;
        private readonly QRCodeService _qrCodeService;

        public UsuarioController(ILogger<UsuarioController> logger, UsuarioService usuarioService, QRCodeService qrCodeService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _qrCodeService = qrCodeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetAll()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                var response = _usuarioService.GetAll(codigoUsuario, codigoAtletica);

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
        public IActionResult CriarUsuario(CadastroUsuarioRequest request)
        {
            try
            {
                var codigoAtletica = GetCodigoAtletica();

                _usuarioService.AtleticaCadastrarUsuario(request, codigoAtletica);

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
        public IActionResult Editar([FromBody] CadastroUsuarioRequest request, Guid codigo)
        {
            try
            {
                _usuarioService.EditarUsuario(codigo, request);

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
                _usuarioService.DesvincularUsuarioAtletica(codigo);

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

        [HttpPost("AssociarAtletica/{codigoQrCode}")]
        public IActionResult AssociarUsuarioAtletica(Guid codigoQrCode)
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();

                _qrCodeService.AssociarQRCode(codigoUsuario, codigoQrCode);

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
