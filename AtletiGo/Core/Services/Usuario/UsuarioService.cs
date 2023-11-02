using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Autenticacao;
using System;

namespace AtletiGo.Core.Services.Usuario
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public string Autenticar(string usuario, string senha)
        {
            var token = TokenService.GenerateToken(new Entities.Usuario
            {
                Nome = usuario,
                TipoUsuario = Utils.Enums.TipoUsuario.Universitario,
                CodigoAtletica = null
            });

            return token;
        }
    }
}
