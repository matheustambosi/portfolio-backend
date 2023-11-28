using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Modalidade;
using Moq;

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
    }
}
