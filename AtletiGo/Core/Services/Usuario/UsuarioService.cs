using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Services.Usuario;

public class UsuarioService(IUsuarioRepository users)
{
    public List<GetAllUsuarioResponse> GetAll(Guid codigoUsuario, Guid? codigoAtletica)
    {
        var actor = users.GetById<Entities.Usuario>(codigoUsuario);
        if (actor.TipoUsuario is not (TipoUsuario.Administrador or TipoUsuario.Representante))
            throw new AtletiGoException("Sem permissao para listar usuarios.");
        var result = actor.TipoUsuario == TipoUsuario.Administrador ? users.GetAll<Entities.Usuario>()
            : users.GetAll<Entities.Usuario>(new { CodigoAtletica = actor.CodigoAtletica });
        return result.Select(u => new GetAllUsuarioResponse(u)).ToList();
    }
    public void CadastrarUsuario(CadastroRequest request)
    {
        request.Validar();
        users.Register(request.Nome, request.Email, request.Senha);
    }
    public void AtleticaCadastrarUsuario(CadastroUsuarioRequest request, Guid? codigoAtletica)
    {
        request.Validar();
        if (string.IsNullOrEmpty(request.Senha) || request.Senha.Length < 8)
            throw new AtletiGoException("Informe uma senha com pelo menos 8 caracteres.");
        var user = users.Register(request.Nome, request.Email, request.Senha);
        if (user.TipoUsuario != TipoUsuario.Nenhum || user.CodigoAtletica != null)
            throw new AtletiGoException("Usuario ja vinculado. Use a edicao autorizada da conta.");
        user.TipoUsuario = request.TipoUsuario;
        user.CodigoAtletica = codigoAtletica;
        users.Update(user);
    }
    public void ValidarUsuarioExiste(string email)
    {
        if (users.GetAll<Entities.Usuario>(new { Email = email }).Any())
            throw new AtletiGoException("Ja existe um usuario cadastrado com o email informado.");
    }
    public void EditarUsuario(Guid codigo, CadastroUsuarioRequest request)
    {
        var user = users.GetById<Entities.Usuario>(codigo) ?? throw new AtletiGoException("Usuário inválido.");
        request.Validar();
        user.AlterarUsuario(request);
        users.Update(user);
    }
    public void DesvincularUsuarioAtletica(Guid codigoUsuario)
    {
        var user = users.GetById<Entities.Usuario>(codigoUsuario) ?? throw new AtletiGoException("Usuário inválido.");
        user.CodigoAtletica = null;
        user.TipoUsuario = TipoUsuario.Nenhum;
        users.Update(user);
    }
    public LoginResponse Autenticar(string email, string senha) => users.Login(email, senha);
    public Entities.Usuario RequireManager(Guid actorId, Guid? targetId = null, TipoUsuario? requestedRole = null)
    {
        var actor = users.GetById<Entities.Usuario>(actorId);
        if (actor.TipoUsuario == TipoUsuario.Administrador) return actor;
        if (actor.TipoUsuario != TipoUsuario.Representante || actor.CodigoAtletica == null || requestedRole == TipoUsuario.Administrador)
            throw new AtletiGoException("Sem permissao para gerenciar usuarios.");
        if (targetId.HasValue)
        {
            var target = users.GetById<Entities.Usuario>(targetId.Value);
            if (target.CodigoAtletica != actor.CodigoAtletica || target.TipoUsuario == TipoUsuario.Administrador)
                throw new AtletiGoException("Usuario pertence a outra atletica.");
        }
        return actor;
    }
}
