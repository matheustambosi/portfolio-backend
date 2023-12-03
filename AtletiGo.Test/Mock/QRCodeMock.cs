using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Test.Mock.Base;
using Moq;

namespace AtletiGo.Test.Mock
{
    public class QRCodeMock : IMockBase<IQRCodeRepository>
    {
        private readonly Mock<IQRCodeRepository> _mock;

        public QRCodeMock() => _mock = new Mock<IQRCodeRepository>();

        public Mock<IQRCodeRepository> SetupQueries()
        {
            return _mock;
        }
    }
}
