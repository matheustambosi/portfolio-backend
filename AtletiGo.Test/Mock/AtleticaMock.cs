using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Test.Mock.Base;
using Moq;

namespace AtletiGo.Test.Mock
{
    public class AtleticaMock : IMockBase<IAtleticaRepository>
    {
        private readonly Mock<IAtleticaRepository> _mock;

        public AtleticaMock() => _mock = new Mock<IAtleticaRepository>();

        public Mock<IAtleticaRepository> SetupQueries()
        {
            return _mock;
        }
    }
}
