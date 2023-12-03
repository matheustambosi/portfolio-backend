using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Test.Mock.Base;
using Moq;

namespace AtletiGo.Test.Mock
{
    public class EventoMock : IMockBase<IEventoRepository>
    {
        private readonly Mock<IEventoRepository> _mock;

        public EventoMock() => _mock = new Mock<IEventoRepository>();

        public Mock<IEventoRepository> SetupQueries()
        {
            return _mock;
        }
    }
}
