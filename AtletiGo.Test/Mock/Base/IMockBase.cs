using Moq;

namespace AtletiGo.Test.Mock.Base
{
    public interface IMockBase<T> where T : class
    {
        public Mock<T> SetupQueries();
    }
}
