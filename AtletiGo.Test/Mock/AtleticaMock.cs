using AtletiGo.Core.Entities;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Utils.Enums;
using AtletiGo.Test.Mock.Base;
using Moq;
using System.Collections.Generic;
using System;

namespace AtletiGo.Test.Mock
{
    public class AtleticaMock : IMockBase<IAtleticaRepository>
    {
        private readonly Mock<IAtleticaRepository> _mock;

        public AtleticaMock() => _mock = new Mock<IAtleticaRepository>();

        public Mock<IAtleticaRepository> SetupQueries()
        {
            _mock.Setup(repo => repo.GetAll<Atletica>())
                .Returns(new List<Atletica>
                {
                    new Atletica {
                        Codigo = Guid.NewGuid(),
                        Nome = "Teste",
                        Cidade = "Teste",
                        Estado = "Teste",
                        Universidade = "Teste",
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                    new Atletica {
                        Codigo = Guid.NewGuid(),
                        Nome = "Teste 2",
                        Cidade = "Teste 2",
                        Estado = "Teste 2",
                        Universidade = "Teste 2",
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                });

            _mock.Setup(repo => repo.GetById<Atletica>(new Guid("550167FA-0817-4296-8E3C-DCD71549107E")))
                .Returns(new Atletica
                {
                    Codigo = new Guid("550167FA-0817-4296-8E3C-DCD71549107E"),
                    Nome = "Teste 1",
                    Cidade = "Teste 1",
                    Estado = "Teste 1",
                    Universidade = "Teste 1",
                    DtCriacao = DateTime.Now,
                    DtAlteracao = DateTime.Now,
                    Situacao = Situacao.Ativo
                });

            return _mock;
        }
    }
}
