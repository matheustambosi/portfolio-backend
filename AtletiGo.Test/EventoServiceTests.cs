using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Evento;
using AtletiGo.Core.Services.Evento;
using AtletiGo.Test.Mock;
using Moq;
using System;
using Xunit;

namespace AtletiGo.Test
{
    public class EventoServiceTests
    {
        private readonly EventoMock _eventoMock;
        private readonly UsuarioMock _usuarioMock;
        private readonly AtletaMock _atletaMock;
        private readonly EventoService _eventoService;

        public EventoServiceTests()
        {
            _eventoMock = new EventoMock();
            _usuarioMock = new UsuarioMock();
            _atletaMock = new AtletaMock();
            _eventoService = new EventoService(_eventoMock.SetupQueries().Object, _usuarioMock.SetupQueries().Object, _atletaMock.SetupQueries().Object);
        }

        [Fact]
        public void CriarEvento_RequestValida_DeveCriarEvento()
        {
            // Arrange
            var request = new CriarEventoRequest
            {
                NomeEvento = "Teste",
                EnderecoEvento = "Teste"
            };

            // Act
            _eventoService.CriarEvento(request, Guid.NewGuid());

            // Assert
            _eventoMock.SetupQueries().Verify(repo => repo.Insert(It.IsAny<Evento>()), Times.Once);
        }

        [Fact]
        public void CriarEvento_NomeInvalido_DeveEstourarException()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CriarEventoRequest
            {
                NomeEvento = ""
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _eventoService.CriarEvento(request, codigoAtletica));
            Assert.Equal("Nome do evento é obrigatório.", ex.Message);
        }

        [Fact]
        public void CriarEvento_EnderecoInvalido_DeveEstourarException()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CriarEventoRequest
            {
                NomeEvento = "Teste",
                EnderecoEvento = ""
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _eventoService.CriarEvento(request, codigoAtletica));
            Assert.Equal("Endereço do evento é obrigatório.", ex.Message);
        }

        [Fact]
        public void EditarEvento_EventoInexistente_DeveEstourarException()
        {
            var codigoEvento = Guid.Empty;

            // Arrange
            var request = new CriarEventoRequest();

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _eventoService.EditarEvento(codigoEvento, request));
            Assert.Equal("Evento inválido.", ex.Message);
        }

        [Fact]
        public void DesativarEvento_EventoInexistente_DeveEstourarException()
        {
            // Arrange
            var codigoEvento = Guid.Empty;

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _eventoService.DesativarEvento(codigoEvento, Guid.Empty));
            Assert.Equal("Evento inválido.", ex.Message);
        }
    }
}
