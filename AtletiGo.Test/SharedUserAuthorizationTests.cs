using System;
using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Core.Utils.Enums;
using Moq;
using Xunit;

public class SharedUserAuthorizationTests
{
    [Fact]
    public void RepresentativeCannotPromoteAdministratorOrManageOtherAssociation()
    {
        var actor = new Usuario { Codigo = Guid.NewGuid(), CodigoAtletica = Guid.NewGuid(), TipoUsuario = TipoUsuario.Representante };
        var target = new Usuario { Codigo = Guid.NewGuid(), CodigoAtletica = Guid.NewGuid(), TipoUsuario = TipoUsuario.Universitario };
        var repository = new Mock<IUsuarioRepository>();
        repository.Setup(r => r.GetById<Usuario>(actor.Codigo)).Returns(actor);
        repository.Setup(r => r.GetById<Usuario>(target.Codigo)).Returns(target);
        var service = new UsuarioService(repository.Object);
        Assert.Throws<AtletiGoException>(() => service.RequireManager(actor.Codigo, requestedRole: TipoUsuario.Administrador));
        Assert.Throws<AtletiGoException>(() => service.RequireManager(actor.Codigo, target.Codigo));
        target.CodigoAtletica = actor.CodigoAtletica;
        Assert.Equal(actor, service.RequireManager(actor.Codigo, target.Codigo, TipoUsuario.Universitario));
    }

    [Fact]
    public void BasicUserCannotManageOrListAccounts()
    {
        var actor = new Usuario { Codigo = Guid.NewGuid(), TipoUsuario = TipoUsuario.Nenhum };
        var repository = new Mock<IUsuarioRepository>();
        repository.Setup(r => r.GetById<Usuario>(actor.Codigo)).Returns(actor);
        var service = new UsuarioService(repository.Object);
        Assert.Throws<AtletiGoException>(() => service.RequireManager(actor.Codigo));
        Assert.Throws<AtletiGoException>(() => service.GetAll(actor.Codigo, null));
    }
}
