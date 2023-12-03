using AtletiGo.Core.Entities;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Utils.Enums;
using AtletiGo.Test.Mock.Base;
using Moq;
using System;
using System.Collections.Generic;

namespace AtletiGo.Test.Mock
{
    public class ModalidadeMock : IMockBase<IModalidadeRepository>
    {
        private readonly Mock<IModalidadeRepository> _mock;

        public ModalidadeMock() => _mock = new Mock<IModalidadeRepository>();

        public Mock<IModalidadeRepository> SetupQueries()
        {
            _mock.Setup(repo => repo.GetAll<Modalidade>())
                .Returns(new List<Modalidade>
                {
                    new Modalidade
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Descricao = "Teste",
                        BuscandoAtletas = true,
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                    new Modalidade
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Descricao = "Teste 2",
                        BuscandoAtletas = true,
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    }
                });

            _mock.Setup(repo => repo.GetAll<Modalidade>(It.IsAny<object>()))
                .Returns(new List<Modalidade>
                {
                    new Modalidade
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Descricao = "Teste",
                        BuscandoAtletas = true,
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    },
                    new Modalidade
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoAtletica = Guid.NewGuid(),
                        Descricao = "Teste 2",
                        BuscandoAtletas = true,
                        Situacao = Situacao.Ativo,
                        DtCriacao = DateTime.Now,
                        DtAlteracao = DateTime.Now
                    }
                });

            return _mock;
        }
    }
}
