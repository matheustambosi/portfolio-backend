using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.QRCode;
using AtletiGo.Core.Services.QRCode;
using AtletiGo.Core.Utils.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AtletiGo.Controllers
{
    [Authorize(Roles = nameof(TipoUsuario.Representante))]
    [ApiController]
    [Route("[controller]")]
    public class QRCodeController : ControllerBase
    {
        private readonly ILogger<QRCodeController> _logger;

        private readonly QRCodeService _qrCodeService;

        public QRCodeController(ILogger<QRCodeController> logger, QRCodeService qrCodeService)
        {
            _logger = logger;
            _qrCodeService = qrCodeService;
        }

        [HttpGet]
        public ActionResult<List<QRCode>> ListarQRCodes()
        {
            try
            {
                return _qrCodeService.Listar();
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

        [HttpPost]
        public IActionResult CriarQRCode(CriarQRCodeRequest request)
        {
            try
            {
                var codigoAtletica = GetCodigoAtletica();

                var codigoQRCode = _qrCodeService.CriarQRCode(request, codigoAtletica);

                return Ok(codigoQRCode);
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

        [HttpDelete("{codigo}")]
        public IActionResult InativarQRCode(Guid codigo)
        {
            try
            {
                _qrCodeService.InativarQRCode(codigo);

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
