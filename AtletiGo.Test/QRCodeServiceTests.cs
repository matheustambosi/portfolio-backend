using AtletiGo.Core.Messaging.QRCode;
using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.QRCode;
using AtletiGo.Core.Utils.Enums;
using Moq;
using System.Collections.Generic;
using System;
using Xunit;
using AtletiGo.Core.Entities;

namespace AtletiGo.Test
{
    public class QRCodeServiceTests
    {
        private readonly Mock<IQRCodeRepository> _mockRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;

        private readonly QRCodeService _qrCodeService;

        public QRCodeServiceTests()
        {
            _mockRepository = new Mock<IQRCodeRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _qrCodeService = new QRCodeService(_mockRepository.Object, _mockUsuarioRepository.Object);
        }

        [Fact]
        public void Listar_ReturnsListOfQRCode()
        {
            // Arrange
            var qrCodesMock = new List<QRCode>
            {
                new QRCode
                {
                    Codigo = Guid.NewGuid(),
                    CodigoAtletica= Guid.NewGuid(),
                    Descricao = "Teste",
                    DtCriacao= DateTime.Now,
                    DuracaoDias = 5,
                    Situacao = Situacao.Ativo
                },
                new QRCode
                {
                    Codigo = Guid.NewGuid(),
                    CodigoAtletica= Guid.NewGuid(),
                    Descricao = "Teste",
                    DtCriacao= DateTime.Now,
                    DuracaoDias = 5,
                    Situacao = Situacao.Ativo
                }
            };

            var usuarioMock = new Usuario
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = null,
                Nome = "Teste",
                Email = "Teste",
                HashSenha = "Teste",
                TipoUsuario = TipoUsuario.Administrador,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.GetAll<QRCode>()).Returns(qrCodesMock);

            _mockUsuarioRepository.Setup(repo => repo.GetById<Usuario>(usuarioMock.Codigo)).Returns(usuarioMock);

            // Act
            var result = _qrCodeService.GetAll(usuarioMock.Codigo, usuarioMock.CodigoAtletica);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<GetAllQrCodeResponse>>(result);
            Assert.Equal(qrCodesMock.Count, result.Count);
        }

        [Fact]
        public void CriarQRCode_ValidRequest_CreatesQRCode()
        {
            // Arrange
            var request = new CriarQRCodeRequest
            {
                Descricao = "Test QR Code",
                DuracaoDias = 5
            };

            // Act
            var result = _qrCodeService.CriarQRCode(request, Guid.NewGuid());

            // Assert
            _mockRepository.Verify(repo => repo.Insert(It.IsAny<QRCode>()), Times.Once);
        }

        [Fact]
        public void AssociarQRCode_ValidQRCode_AssociatesQRCodeToUser()
        {
            // Arrange
            var qrCode = new QRCode
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = Guid.NewGuid(),
                DtCriacao = DateTime.Now,
                Descricao = "Test QR Code",
                DuracaoDias = 5,
                Situacao = Situacao.Ativo
            };

            var usuario = new Usuario
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = null,
                TipoUsuario = TipoUsuario.Nenhum
            };

            _mockRepository.Setup(repo => repo.GetById<QRCode>(qrCode.Codigo)).Returns(qrCode);
            _mockUsuarioRepository.Setup(repo => repo.GetById<Usuario>(usuario.Codigo)).Returns(usuario);

            // Act
            _qrCodeService.AssociarQRCode(usuario.Codigo, qrCode.Codigo);

            // Assert
            Assert.Equal(qrCode.CodigoAtletica, usuario.CodigoAtletica);
            Assert.Equal(TipoUsuario.Universitario, usuario.TipoUsuario);
        }

        [Fact]
        public void InativarQRCode_ValidQRCode_InactivatesQRCode()
        {
            // Arrange
            var qrCode = new QRCode
            {
                Codigo = Guid.NewGuid(),
                Descricao = "Test QR Code",
                DuracaoDias = 5,
                Situacao = Situacao.Ativo
            };

            _mockRepository.Setup(repo => repo.GetById<QRCode>(qrCode.Codigo)).Returns(qrCode);

            // Act
            _qrCodeService.InativarQRCode(qrCode.Codigo);

            // Assert
            Assert.Equal(Situacao.Inativo, qrCode.Situacao);
        }
    }
}
