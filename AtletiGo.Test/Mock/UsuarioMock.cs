using AtletiGo.Core.Entities;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using AtletiGo.Test.Mock.Base;
using Moq;
using System;
using System.Collections.Generic;

namespace AtletiGo.Test.Mock
{
    public class UsuarioMock : IMockBase<IUsuarioRepository>
    {
        private readonly Mock<IUsuarioRepository> _mock;

        public UsuarioMock() => _mock = new Mock<IUsuarioRepository>();

        public Mock<IUsuarioRepository> SetupQueries()
        {
            //Mock - Administrador
            _mock.Setup(repo => repo.GetById<Usuario>(new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305")))
                .Returns(new Usuario
                {
                    Codigo = new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305"),
                    CodigoAtletica = null,
                    Nome = "Administrador",
                    Email = "Administrador",
                    HashSenha = "Administrador",
                    TipoUsuario = TipoUsuario.Administrador,
                    DtCriacao = DateTime.Now,
                    DtAlteracao = DateTime.Now
                });

            _mock.Setup(repo => repo.GetAll<Usuario>())
                .Returns(new List<Usuario>
                {
                    new Usuario
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Nome = "Teste",
                        Email = "Teste",
                        HashSenha = "Teste",
                        TipoUsuario = TipoUsuario.Universitario,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                    new Usuario
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Nome = "Teste",
                        Email = "Teste",
                        HashSenha = "Teste",
                        TipoUsuario = TipoUsuario.Representante,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    }
                });

            //Mock - Representante
            _mock.Setup(repo => repo.GetById<Usuario>(new Guid("E4548A8B-C881-4DEF-9A49-2A0778B477BF")))
                .Returns(new Usuario
                {
                    Codigo = new Guid("E4548A8B-C881-4DEF-9A49-2A0778B477BF"),
                    CodigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505"),
                    Nome = "Representante",
                    Email = "Representante",
                    HashSenha = "Representante",
                    TipoUsuario = TipoUsuario.Representante,
                    DtCriacao = DateTime.Now,
                    DtAlteracao = DateTime.Now
                });

            _mock.Setup(repo => repo.GetAll<Usuario>(It.IsAny<object>())).Returns(new List<Usuario>
                {
                    new Usuario
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505"),
                        Nome = "Teste",
                        Email = "Teste",
                        HashSenha = "Teste",
                        TipoUsuario = TipoUsuario.Universitario,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                    new Usuario
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505"),
                        Nome = "Teste",
                        Email = "Teste",
                        HashSenha = "Teste",
                        TipoUsuario = TipoUsuario.Representante,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    }
                });

            //Mock - Universitário
            _mock.Setup(repo => repo.GetById<Usuario>(new Guid("ADF7E04C-531C-499B-A236-A05ECF57CF72")))
                .Returns(new Usuario
                {
                    Codigo = new Guid("ADF7E04C-531C-499B-A236-A05ECF57CF72"),
                    CodigoAtletica = Guid.NewGuid(),
                    Nome = "Teste",
                    Email = "Teste",
                    TipoUsuario = TipoUsuario.Universitario,
                    HashSenha = "1234",
                    DtCriacao = DateTime.Now,
                    DtAlteracao = DateTime.Now
                });

            return _mock;
        }
    }
}
