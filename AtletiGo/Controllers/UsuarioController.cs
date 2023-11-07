using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
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

        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario)
        {
            try
            {
                _usuarioService.Insert(usuario);

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
        public ActionResult<IEnumerable<Usuario>> GetAll()
        {
            try
            {
                var response = _usuarioService.GetAll();

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
