using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.QRCode;
using Moq;

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
    }
}
