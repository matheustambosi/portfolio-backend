using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Autenticacao;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Services.Usuario
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<Entities.Usuario> GetAll()
        {
            return _usuarioRepository.GetAll<Entities.Usuario>()?.ToList();
        }

        public void Insert(Entities.Usuario entity)
        {
            _usuarioRepository.Insert(entity);
        }

        public string Autenticar(string usuario, string senha)
        {
            // Validar usuario e senha

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
