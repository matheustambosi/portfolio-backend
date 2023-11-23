using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
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
    [Authorize]
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
        public ActionResult<List<GetAllQrCodeResponse>> ListarQRCodes()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                var response = _qrCodeService.GetAll(codigoUsuario, codigoAtletica);

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
        public IActionResult CriarQRCode(CriarQRCodeRequest request)
        {
            try
            {
                var codigoAtletica = GetCodigoAtletica();

                if (codigoAtletica == null)
                    throw new AtletiGoException("Atlética inválida.");

                var codigoQRCode = _qrCodeService.CriarQRCode(request, codigoAtletica.Value);

                return Ok(codigoQRCode);
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
        public IActionResult InativarQRCode(Guid codigo)
        {
            try
            {
                _qrCodeService.InativarQRCode(codigo);

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
