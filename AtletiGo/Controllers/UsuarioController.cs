using AtletiGo.Core.Services.QRCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AtletiGo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        private readonly QRCodeService _qrCodeService;

        public UsuarioController(ILogger<UsuarioController> logger, QRCodeService qrCodeService)
        {
            _logger = logger;
            _qrCodeService = qrCodeService;
        }

        [HttpPost("AssociarAtletica/{codigoQrCode}")]
        public IActionResult AssociarUsuarioAtletica(Guid codigoQrCode)
        {
            var codigoUsuario = GetCodigoUsuario();

            _qrCodeService.AssociarQRCode(codigoUsuario, codigoQrCode);

            return NoContent();
        }
    }
}
