using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Modalidade;
using AtletiGo.Core.Utils.Enums;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System;
using Xunit;
using AtletiGo.Core.Entities;
using AtletiGo.Core.Messaging.QRCode;

namespace AtletiGo.Test
{
    public class ModalidadeServiceTests
    {
        private readonly Mock<IModalidadeRepository> _mockRepository;
        private readonly Mock<IAtletaRepository> _mockAtletaRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly ModalidadeService _modalidadeService;

        public ModalidadeServiceTests()
        {
            _mockRepository = new Mock<IModalidadeRepository>();
            _mockAtletaRepository = new Mock<IAtletaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _modalidadeService = new ModalidadeService(_mockRepository.Object, _mockUsuarioRepository.Object, _mockAtletaRepository.Object);
        }

        [Fact]
        public void GetAllModalidadesBuscandoAtletas_ReturnsListOfModalidadesBuscandoAtletas()
        {
            // Arrange
            var codigoUsuario = Guid.NewGuid();
            var codigoAtletica = Guid.NewGuid();

            var usuarioMock = new Usuario
            {
                Codigo = codigoUsuario,
                CodigoAtletica = null,
                Nome = "Teste",
                Email = "Teste",
                HashSenha = "Teste",
                TipoUsuario = TipoUsuario.Administrador,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            var codigoModalidade = Guid.NewGuid();

            var modalidadesMock = new List<Modalidade>
            {
                new Modalidade
                {
                    Codigo = codigoModalidade,
                    CodigoAtletica = codigoAtletica,
                    BuscandoAtletas = true,
                    Descricao= "Teste",
                    Situacao = Situacao.Ativo,
                    DtCriacao= DateTime.Now,
                    DtAlteracao = DateTime.Now
                },
                new Modalidade
                {
                    Codigo = Guid.NewGuid(),
                    CodigoAtletica = codigoAtletica,
                    BuscandoAtletas = true,
                    Descricao= "Teste 2",
                    Situacao = Situacao.Ativo,
                    DtCriacao= DateTime.Now,
                    DtAlteracao = DateTime.Now
                }
            };

            var atletasMock = new List<Atleta>
            {
                new Atleta
                {
                    Codigo= Guid.NewGuid(),
                    CodigoModalidade = codigoModalidade,
                    CodigoUsuario = codigoUsuario
                }
            };

            _mockUsuarioRepository.Setup(repo => repo.GetById<Usuario>(usuarioMock.Codigo)).Returns(usuarioMock);

            _mockRepository.Setup(repo => repo.GetAll<Modalidade>()).Returns(modalidadesMock);

            _mockAtletaRepository.Setup(repo => repo.GetAll<Atleta>(It.IsAny<object>())).Returns(atletasMock);

            // Act
            var result = _modalidadeService.GetAllModalidadesBuscandoAtletas(usuarioMock.Codigo, usuarioMock.CodigoAtletica.GetValueOrDefault());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllModalidadesBuscandoAtletasResponse>>(result);
            Assert.Equal(modalidadesMock.Count, result.Count);
        }

        //[Fact]
        //public void CadastrarModalidade_NewModalidade_CreatesModalidade()
        //{
        //    // Arrange
        //    var codigoAtletica = Guid.NewGuid();
        //    var request = new CriarModalidadeRequest
        //    {

        //    };

        //    var modalidadesMock = new List<Modalidade>
        //    {
                
        //    };

        //    _mockRepository.Setup(repo => repo.GetAll<Modalidade>(It.IsAny<object>())).Returns(modalidadesMock);

        //    // Act
        //    _modalidadeService.CadastrarModalidade(request, codigoAtletica);

        //    // Assert
        //    _mockRepository.Verify(repo => repo.Insert(It.IsAny<Modalidade>()), Times.Once);
        //    // Add more assertions based on expected behavior
        //}

        //[Fact]
        //public void EditarModalidade_ValidId_EditsModalidade()
        //{
        //    // Arrange
        //    var codigo = Guid.NewGuid();
        //    var request = new CriarModalidadeRequest(); // Populate request object with necessary data for the test
        //    var mockModalidade = new Entities.Modalidade(); // Mock modalidade data

        //    _modalidadeRepositoryMock.Setup(repo => repo.GetById<Entities.Modalidade>(codigo))
        //                             .Returns(mockModalidade);

        //    _modalidadeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Entities.Modalidade>()));

        //    // Act
        //    _modalidadeService.EditarModalidade(codigo, request);

        //    // Assert
        //    _modalidadeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Entities.Modalidade>()), Times.Once);
        //    // Add more assertions based on expected behavior
        //}

        //[Fact]
        //public void DeletarModalidadeAtletica_ValidId_DeletesModalidadeAndRelatedAtletas()
        //{
        //    // Arrange
        //    var codigoModalidade = Guid.NewGuid();
        //    var mockModalidade = new Entities.Modalidade { Codigo = codigoModalidade }; // Mock modalidade data
        //    var mockAtletas = new List<Entities.Atleta>(); // Mock atletas data

        //    _modalidadeRepositoryMock.Setup(repo => repo.GetById<Entities.Modalidade>(codigoModalidade))
        //                             .Returns(mockModalidade);

        //    _atletaRepositoryMock.Setup(repo => repo.GetAll<Entities.Atleta>(It.IsAny<object>()))
        //                         .Returns(mockAtletas);

        //    _atletaRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Entities.Atleta>()));
        //    _modalidadeRepositoryMock.Setup(repo => repo.Update(It.IsAny<Entities.Modalidade>()));

        //    // Act
        //    _modalidadeService.DeletarModalidadeAtletica(codigoModalidade);

        //    // Assert
        //    _atletaRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Entities.Atleta>()), Times.Exactly(mockAtletas.Count));
        //    _modalidadeRepositoryMock.Verify(repo => repo.Update(It.IsAny<Entities.Modalidade>()), Times.Once);
        //    // Add more assertions based on expected behavior
        //}
    }
}
