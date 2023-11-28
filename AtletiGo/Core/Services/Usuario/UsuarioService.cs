using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Messaging.Usuario;
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

        public List<GetAllUsuarioResponse> GetAll(Guid codigoUsuario, Guid? codigoAtletica)
        {
            var result = new List<Entities.Usuario>();

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            if (usuario.TipoUsuario == TipoUsuario.Administrador)
                result = _usuarioRepository.GetAll<Entities.Usuario>().ToList();
            else
                result = _usuarioRepository.GetAll<Entities.Usuario>(new { CodigoAtletica = codigoAtletica }).ToList();

            return result?.Select(usuario => new GetAllUsuarioResponse(usuario))?.ToList();
        }

        public void CadastrarUsuario(CadastroRequest request)
        {
            request.Validar();

            var usuario = new Entities.Usuario
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = null,
                Nome = request.Nome,
                Email = request.Email,
                HashSenha = BC.HashPassword(request.Senha),
                TipoUsuario = TipoUsuario.Nenhum,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            _usuarioRepository.Insert(usuario);
        }

        public void AtleticaCadastrarUsuario(CadastroUsuarioRequest request, Guid? codigoAtletica)
        {
            request.Validar();

            var usuario = new Entities.Usuario
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = codigoAtletica,
                Nome = request.Nome,
                Email = request.Email,
                HashSenha = BC.HashPassword("1234"),
                TipoUsuario = request.TipoUsuario,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            _usuarioRepository.Insert(usuario);
        }

        public void EditarUsuario(Guid codigo, CadastroUsuarioRequest request)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigo);

            if (usuario == null)
                throw new AtletiGoException("Usuário inválido.");

            usuario.AlterarUsuario(request);

            _usuarioRepository.Update(usuario);
        }

        public void DesvincularUsuarioAtletica(Guid codigoUsuario)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            if (usuario == null)
                throw new AtletiGoException("Usuário inválido.");

            usuario.CodigoAtletica = null;
            usuario.TipoUsuario = TipoUsuario.Nenhum;

            _usuarioRepository.Update(usuario);
        }

        public LoginResponse Autenticar(string email, string senha)
        {
            var usuario = _usuarioRepository.GetAll<Entities.Usuario>(new { Email = email }).FirstOrDefault();

            if (usuario != null)
            {
                var senhaValida = BC.Verify(senha, usuario?.HashSenha);

                if (senhaValida)
                {
                    var token = TokenService.GenerateToken(new Entities.Usuario
                    {
                        Nome = usuario.Nome,
                        Codigo = usuario.Codigo,
                        TipoUsuario = usuario.TipoUsuario,
                        CodigoAtletica = usuario.CodigoAtletica
                    });

                    return new LoginResponse(usuario, token);
                }
            }

            throw new AtletiGoException("Email ou senha inválido.");
        }
    }
}
