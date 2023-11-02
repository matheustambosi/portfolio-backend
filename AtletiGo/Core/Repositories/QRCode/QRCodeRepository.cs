using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.QRCode
{
    public class QRCodeRepository : RepositoryBase, IQRCodeRepository
    {
        public QRCodeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
