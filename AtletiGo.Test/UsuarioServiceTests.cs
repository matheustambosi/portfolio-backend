using AtletiGo.Core.Entities;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Core.Utils.Enums;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AtletiGo.Test
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_Usuarios_ReturnsListOfUsuarios_Administrador()
        {
            var codigoUsuario = Guid.NewGuid();
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var usuariosMock = new List<Usuario>
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
            };

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

            _mockRepository.Setup(repo => repo.GetAll<Usuario>()).Returns(usuariosMock);

            _mockRepository.Setup(repo => repo.GetById<Usuario>(codigoUsuario)).Returns(usuarioMock);

            // Act
            var result = _usuarioService.GetAll(codigoUsuario, codigoAtletica);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllUsuarioResponse>>(result);
            Assert.Equal(usuariosMock.Count, result.Count);
        }
    }
}
