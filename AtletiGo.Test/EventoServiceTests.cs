using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Evento;
using Moq;

namespace AtletiGo.Test
{
    public class EventoServiceTests
    {
        private readonly Mock<IEventoRepository> _mockRepository;
        private readonly Mock<IAtletaRepository> _mockAtletaRepository;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly EventoService _eventoService;

        public EventoServiceTests()
        {
            _mockRepository = new Mock<IEventoRepository>();
            _mockAtletaRepository = new Mock<IAtletaRepository>();
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _eventoService = new EventoService(_mockRepository.Object, _mockUsuarioRepository.Object, _mockAtletaRepository.Object);
        }
    }
}
