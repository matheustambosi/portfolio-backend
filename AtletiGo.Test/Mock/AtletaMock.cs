using AtletiGo.Core.Entities;
using AtletiGo.Core.Messaging.RawQueryResult;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Test.Mock.Base;
using Moq;
using System;
using System.Collections.Generic;

namespace AtletiGo.Test.Mock
{
    public class AtletaMock : IMockBase<IAtletaRepository>
    {
        private readonly Mock<IAtletaRepository> _mock;

        public AtletaMock() => _mock = new Mock<IAtletaRepository>();

        public Mock<IAtletaRepository> SetupQueries()
        {
            _mock.Setup(repo => repo.GetById<Atleta>(It.IsAny<Guid>()))
                .Returns(new Atleta
                {
                    Codigo = new Guid("DFC258A0-371D-41D6-BB1F-202C434168AA")
                });

            _mock.Setup(repo => repo.GetAll<Atleta>())
                .Returns(new List<Atleta>
                {
                    new Atleta
                    {
                        Codigo = Guid.NewGuid()
                    },
                    new Atleta
                    {
                        Codigo = Guid.NewGuid()
                    }
                });

            _mock.Setup(repo => repo.Query<GetAllAtletasRawQueryResult>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(new List<GetAllAtletasRawQueryResult>
                        {
                            new GetAllAtletasRawQueryResult
                            {
                            },
                            new GetAllAtletasRawQueryResult
                            {
                            }
                        });

            return _mock;
        }
    }
}
