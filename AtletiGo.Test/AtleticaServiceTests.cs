using AtletiGo.Core.Entities;
using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Utils.Enums;
using AtletiGo.Test.Mock;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtletiGo.Test
{
    public class AtleticaServiceTests
    {
        private readonly AtleticaMock _atleticaMock;
        private readonly AtleticaService _atleticaService;

        public AtleticaServiceTests()
        {
            _atleticaMock = new AtleticaMock();
            _atleticaService = new AtleticaService(_atleticaMock.SetupQueries().Object);
        }

        [Fact]
        public void GetAll_Atleticas_ReturnsListOfAtleticas()
        {
            // Act
            var result = _atleticaService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Atletica>>(result);
        }

        [Fact]
        public void CadastrarAtletica_ValidRequest_CallsRepositoryInsert()
        {
            // Arrange
            var request = new CriarAtleticaRequest
            {
                NomeAtletica = "Teste 1",
                Cidade = "Teste 1",
                Estado = "Teste 1",
                Universidade = "Teste 1",
                Situacao = Situacao.Ativo
            };

            // Act
            _atleticaService.CadastrarAtletica(request);

            // Assert
            _atleticaMock.SetupQueries().Verify(repo => repo.Insert(It.IsAny<Atletica>()), Times.Once);
        }

        [Fact]
        public void EditarAtletica_Exists_UpdateCalled()
        {
            // Arrange
            var codigoAtletica = new Guid("550167FA-0817-4296-8E3C-DCD71549107E");

            var request = new CriarAtleticaRequest
            {
                NomeAtletica = "Novo Nome",
                Cidade = "Nova Cidade",
                Estado = "Novo Estado",
                Universidade = "Nova Universidade",
                Situacao = Situacao.Ativo
            };
            var atletica = new Atletica
            {
                Codigo = codigoAtletica,
                Nome = "Teste 1",
                Cidade = "Teste 1",
                Estado = "Teste 1",
                Universidade = "Teste 1",
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                Situacao = Situacao.Ativo
            };

            // Act
            _atleticaService.EditarAtletica(codigoAtletica, request);

            // Assert
            _atleticaMock.SetupQueries().Verify(repo => repo.Update(It.IsAny<Atletica>()), Times.Once);
        }

        [Fact]
        public void DesativarAtletica_ExistsAndNotInactive_SetsInactiveAndUpdateCalled()
        {
            // Arrange
            var codigoAtletica = new Guid("550167FA-0817-4296-8E3C-DCD71549107E");

            // Act
            _atleticaService.DesativarAtletica(codigoAtletica);

            // Assert
            _atleticaMock.SetupQueries().Verify(repo => repo.Update(It.IsAny<Atletica>()), Times.Once);
        }
    }
}
