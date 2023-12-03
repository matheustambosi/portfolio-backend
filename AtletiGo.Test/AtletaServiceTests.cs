using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Messaging.RawQueryResult;
using AtletiGo.Core.Services.Atleta;
using AtletiGo.Test.Mock;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtletiGo.Test
{
    public class AtletaServiceTests
    {
        private readonly AtletaMock _atletaMock;
        private readonly UsuarioMock _usuarioMock;
        private readonly AtletaService _atletaService;

        public AtletaServiceTests()
        {
            _atletaMock = new AtletaMock();
            _usuarioMock = new UsuarioMock();
            _atletaService = new AtletaService(_atletaMock.SetupQueries().Object, _usuarioMock.SetupQueries().Object);
        }

        [Fact]
        public void GetAllAtletas_PerfilAdministrador_DeveRetornarAtletas()
        {
            var codigoUsuario = new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305");

            // Act
            var result = _atletaService.GetAllAtletas(codigoUsuario, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllAtletasRawQueryResult>>(result);
        }

        [Fact]
        public void DesvincularAtletaAtletica_UsuarioNaoAdmin_EstourarException()
        {
            var codigoAtleta = new Guid("DFC258A0-371D-41D6-BB1F-202C434168AA");
            var codigoUsuario = new Guid("ADF7E04C-531C-499B-A236-A05ECF57CF72");

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _atletaService.DesvincularAtletaAtletica(codigoAtleta, codigoUsuario, null));
            Assert.Equal("Este atleta não pertence a sua atlética", ex.Message);
        }

        [Fact]
        public void SalvarModalidadeAtleta_SalvarModalidade_DeveInserirModalidade()
        {
            var codigoUsuario = new Guid("ADF7E04C-531C-499B-A236-A05ECF57CF72");

            // Arrange
            var request = new SalvarModalidadeAtletaRequest
            {
                ModalidadesAtivas = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
            };

            // Act
            _atletaService.SalvarModalidadeAtleta(codigoUsuario, request);

            // Assert
            _atletaMock.SetupQueries().Verify(repo => repo.Insert(It.IsAny<Atleta>()), Times.AtLeastOnce);
        }
    }
}
