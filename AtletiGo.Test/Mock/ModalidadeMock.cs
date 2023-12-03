using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Test.Mock.Base;
using Moq;

namespace AtletiGo.Test.Mock
{
    public class ModalidadeMock : IMockBase<IModalidadeRepository>
    {
        private readonly Mock<IModalidadeRepository> _mock;

        public ModalidadeMock() => _mock = new Mock<IModalidadeRepository>();

        public Mock<IModalidadeRepository> SetupQueries()
        {
            return _mock;
        }
    }
}
