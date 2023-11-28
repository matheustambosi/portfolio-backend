using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Atleta;
using Moq;

namespace AtletiGo.Test
{
    public class AtletaServiceTests
    {
        private readonly Mock<IAtletaRepository> _mockRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly AtletaService _atletaService;

        public AtletaServiceTests()
        {
            _mockRepository = new Mock<IAtletaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _atletaService = new AtletaService(_mockRepository.Object, _mockUsuarioRepository.Object);
        }
    }
}
