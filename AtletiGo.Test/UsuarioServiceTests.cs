using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Core.Utils.Enums;
using AtletiGo.Test.Mock;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AtletiGo.Test
{
    public class UsuarioServiceTests
    {
        private readonly UsuarioMock _usuarioMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioMock = new UsuarioMock();
            _usuarioService = new UsuarioService(_usuarioMock.SetupQueries().Object);
        }

        [Fact]
        public void GetAllUsuarios_PerfilAdministrador_DeveRetornarUsuarios()
        {
            var codigoUsuario = new Guid("1193EF05-AFFB-4C5C-AE55-629D65708305");

            // Act
            var result = _usuarioService.GetAll(codigoUsuario, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllUsuarioResponse>>(result);
        }

        [Fact]
        public void GetAllUsuarios_PerfilRepresentante_DeveRetornarUsuarios()
        {
            var codigoUsuario = new Guid("E4548A8B-C881-4DEF-9A49-2A0778B477BF");
            var codigoAtletica = new Guid("726B36F1-03F9-47BD-ACAA-DDECF0307505");

            // Act
            var result = _usuarioService.GetAll(codigoUsuario, codigoAtletica);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllUsuarioResponse>>(result);
        }

        [Fact]
        public void CadastrarUsuario_RequestValida_DeveInserirUsuario()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "Teste@gmail.com",
                Senha = "1234",
                RepeticaoSenha = "1234"
            };

            // Act
            _usuarioService.CadastrarUsuario(request);

            // Assert
            _usuarioMock.SetupQueries().Verify(repo => repo.Insert(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void CadastrarUsuario_UsuarioJaCadastrado_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "Teste",
                Senha = "1234",
                RepeticaoSenha = "1234"
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("Já existe um usuário cadastrado com o email informado.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuario_UsuarioNomeInvalido_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "",
                Email = "Teste",
                Senha = "1234",
                RepeticaoSenha = "1234"
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("Nome é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuario_EmailInvalido_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "",
                Senha = "1234",
                RepeticaoSenha = "1234"
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("Email é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuario_SenhaInvalida_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "Teste",
                Senha = "",
                RepeticaoSenha = "1234"
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("Senha é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuario_RepeticaoSenhaInvalida_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "Teste",
                Senha = "1234",
                RepeticaoSenha = ""
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("Repetição senha é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuario_SenhasDiferentes_DeveEstourarException()
        {
            // Arrange
            var request = new CadastroRequest
            {
                Nome = "Teste",
                Email = "Teste",
                Senha = "1234",
                RepeticaoSenha = "5678"
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.CadastrarUsuario(request));
            Assert.Equal("As senhas não são iguais.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuarioPorAtletica_RequestValida_DeveInserirUsuario()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CadastroUsuarioRequest
            {
                Nome = "Teste",
                Email = "Universitario@gmail.com",
                TipoUsuario = TipoUsuario.Universitario
            };

            // Act
            _usuarioService.AtleticaCadastrarUsuario(request, codigoAtletica);

            // Assert
            _usuarioMock.SetupQueries().Verify(repo => repo.Insert(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void CadastrarUsuarioPorAtletica_NomeInvalido_DeveEstourarException()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CadastroUsuarioRequest
            {
                Nome = "",
                Email = "Universitario@gmail.com",
                TipoUsuario = TipoUsuario.Universitario
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.AtleticaCadastrarUsuario(request, codigoAtletica));
            Assert.Equal("Nome é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuarioPorAtletica_EmailInvalido_DeveEstourarException()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CadastroUsuarioRequest
            {
                Nome = "Teste",
                Email = "",
                TipoUsuario = TipoUsuario.Universitario
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.AtleticaCadastrarUsuario(request, codigoAtletica));
            Assert.Equal("Email é obrigatório.", ex.Message);
        }

        [Fact]
        public void CadastrarUsuarioPorAtletica_UsuarioJaCadastrado_DeveEstourarException()
        {
            var codigoAtletica = Guid.NewGuid();

            // Arrange
            var request = new CadastroUsuarioRequest
            {
                Nome = "Teste",
                Email = "Teste",
                TipoUsuario = TipoUsuario.Universitario
            };

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.AtleticaCadastrarUsuario(request, codigoAtletica));
            Assert.Equal("Já existe um usuário cadastrado com o email informado.", ex.Message);
        }

        [Fact]
        public void EditarUsuario_UsuarioExistente_DeveAtualizarUsuario()
        {
            var codigoUsuario = new Guid("ADF7E04C-531C-499B-A236-A05ECF57CF72");

            // Arrange
            var request = new CadastroUsuarioRequest
            {
                Nome = "Teste",
                Email = "Teste",
                TipoUsuario = TipoUsuario.Representante
            };

            // Act
            _usuarioService.EditarUsuario(codigoUsuario, request);

            // Assert
            _usuarioMock.SetupQueries().Verify(repo => repo.Update(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void EditarUsuario_UsuarioInexistente_DeveEstourarException()
        {
            var codigoUsuario = Guid.Empty;

            // Arrange
            var request = new CadastroUsuarioRequest();

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.EditarUsuario(codigoUsuario, request));
            Assert.Equal("Usuário inválido.", ex.Message);
        }

        [Fact]
        public void DesativarUsuarioAtletica_UsuarioInexistente_DeveEstourarException()
        {
            // Arrange
            var codigoUsuario = Guid.Empty;

            // Act & Assert
            var ex = Assert.Throws<AtletiGoException>(() => _usuarioService.DesvincularUsuarioAtletica(codigoUsuario));
            Assert.Equal("Usuário inválido.", ex.Message);
        }
    }
}
