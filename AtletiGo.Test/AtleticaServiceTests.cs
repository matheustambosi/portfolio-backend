using AtletiGo.Core.Entities;
using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Utils.Enums;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtletiGo.Test
{
    public class AtleticaServiceTests
    {
        private readonly Mock<IAtleticaRepository> _mockRepository;
        private readonly AtleticaService _atleticaService;

        public AtleticaServiceTests()
        {
            _mockRepository = new Mock<IAtleticaRepository>();
            _atleticaService = new AtleticaService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_Atleticas_ReturnsListOfAtleticas()
        {
            // Arrange
            var atleticasMock = new List<Atletica>
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
            };
            _mockRepository.Setup(repo => repo.GetAll<Atletica>()).Returns(atleticasMock);

            // Act
            var result = _atleticaService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Atletica>>(result);
            Assert.Equal(atleticasMock.Count, result.Count);
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
            _mockRepository.Verify(repo => repo.Insert(It.IsAny<Atletica>()), Times.Once);
        }

        [Fact]
        public void EditarAtletica_Exists_UpdateCalled()
        {
            // Arrange
            var codigo = Guid.NewGuid();
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
                Codigo = codigo,
                Nome = "Teste 1",
                Cidade = "Teste 1",
                Estado = "Teste 1",
                Universidade = "Teste 1",
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                Situacao = Situacao.Ativo
            };

            _mockRepository.Setup(repo => repo.GetById<Atletica>(codigo)).Returns(atletica);

            // Act
            _atleticaService.EditarAtletica(codigo, request);

            // Assert
            _mockRepository.Verify(repo => repo.Update(atletica), Times.Once);
        }

        [Fact]
        public void DesativarAtletica_ExistsAndNotInactive_SetsInactiveAndUpdateCalled()
        {
            // Arrange
            var codigo = Guid.NewGuid();
            var atletica = new Atletica
            {
                Codigo = codigo,
                Nome = "Teste 1",
                Cidade = "Teste 1",
                Estado = "Teste 1",
                Universidade = "Teste 1",
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                Situacao = Situacao.Ativo
            };

            _mockRepository.Setup(repo => repo.GetById<Atletica>(codigo)).Returns(atletica);

            // Act
            _atleticaService.DesativarAtletica(codigo);

            // Assert
            Assert.Equal(Situacao.Inativo, atletica.Situacao);
            _mockRepository.Verify(repo => repo.Update(atletica), Times.Once);
        }
    }
}
