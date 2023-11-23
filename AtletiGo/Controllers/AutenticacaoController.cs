using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Services.Autenticacao;
using AtletiGo.Core.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace AtletiGo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly UsuarioService _usuarioService;

        public AutenticacaoController(ILogger<AutenticacaoController> logger, UsuarioService usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var usuario = _usuarioService.Autenticar(request.Email, request.Senha);

                if (usuario is null)
                    throw new AtletiGoException("Usuário ou senha inválidos");

                return Ok(usuario);
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

        [HttpPost("Cadastro")]
        [AllowAnonymous]
        public IActionResult Cadastro(CadastroRequest request)
        {
            try
            {
                _usuarioService.CadastrarUsuario(request);

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
