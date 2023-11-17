using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Autenticacao;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using BC = BCrypt.Net.BCrypt;

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

        public void CadastrarUsuario (CadastroRequest request, Guid? codigoAtletica = null)
        {
            request.Validar();

            var usuario = new Entities.Usuario
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = codigoAtletica ?? null,
                Nome = request.Nome,
                Email = request.Email,
                HashSenha = BC.HashPassword(request.Senha),
                TipoUsuario = TipoUsuario.Nenhum,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            _usuarioRepository.Insert(usuario);
        }

        public LoginResponse Autenticar(string nome, string senha)
        {
            var usuario = _usuarioRepository.GetAll<Entities.Usuario>(new { Nome = nome }).FirstOrDefault();

            if (usuario is null || !BC.Verify(senha, usuario.HashSenha))
            {
                var token = TokenService.GenerateToken(new Entities.Usuario
                {
                    Nome = usuario.Nome,
                    TipoUsuario = Utils.Enums.TipoUsuario.Universitario,
                    CodigoAtletica = null
                });

                return new LoginResponse(usuario, token);
            }

            return null;
        }
    }
}
