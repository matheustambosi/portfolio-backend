using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Services.Modalidade;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Test.Mock;
using System.Collections.Generic;
using System;
using Xunit;
using AtletiGo.Core.Messaging.Modalidade;

namespace AtletiGo.Test
{
    public class ModalidadeServiceTests
    {
        private readonly ModalidadeMock _modalidadeMock;
        private readonly UsuarioMock _usuarioMock;
        private readonly AtletaMock _atletaMock;
        private readonly ModalidadeService _modalidadeService;

        public ModalidadeServiceTests()
        {
            _modalidadeMock = new ModalidadeMock();
            _usuarioMock = new UsuarioMock();
            _atletaMock = new AtletaMock();
            _modalidadeService = new ModalidadeService(_modalidadeMock.SetupQueries().Object, _usuarioMock.SetupQueries().Object, _atletaMock.SetupQueries().Object);
        }

        [Fact]
        public void GetAll_PerfilAdministrador_DeveRetornarModalidades()
        {
            var codigoUsuario = new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305");

            // Act
            var result = _modalidadeService.GetAll(codigoUsuario, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllModalidadeResponse>>(result);
        }

        [Fact]
        public void GetAll_PerfilRepresentante_DeveRetornarModalidades()
        {
            var codigoUsuario = new Guid("E4548A8B-C881-4DEF-9A49-2A0778B477BF");
            var codigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505");

            // Act
            var result = _modalidadeService.GetAll(codigoUsuario, codigoAtletica);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllModalidadeResponse>>(result);
        }

        [Fact]
        public void GetAllModalidadesBuscandoAtletas_PerfilAdministrador_DeveRetornarModalidades()
        {
            var codigoUsuario = new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305");

            // Act
            var result = _modalidadeService.GetAllModalidadesBuscandoAtletas(codigoUsuario, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllModalidadesBuscandoAtletasResponse>>(result);
        }

        [Fact]
        public void GetAllModalidadesBuscandoAtletas_PerfilRepresentante_DeveRetornarModalidades()
        {
            var codigoUsuario = new Guid("E4548A8B-C881-4DEF-9A49-2A0778B477BF");
            var codigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505");

            // Act
            var result = _modalidadeService.GetAllModalidadesBuscandoAtletas(codigoUsuario, codigoAtletica);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllModalidadesBuscandoAtletasResponse>>(result);
        }
    }
}
