using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Test.Mock.Base;
using Moq;

namespace AtletiGo.Test.Mock
{
    public class AtletaMock : IMockBase<IAtletaRepository>
    {
        private readonly Mock<IAtletaRepository> _mock;

        public AtletaMock() => _mock = new Mock<IAtletaRepository>();

        public Mock<IAtletaRepository> SetupQueries()
        {
            return _mock;
        }
    }
}
