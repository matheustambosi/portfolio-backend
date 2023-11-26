using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.Modalidade
{
    public class ModalidadeRepository : RepositoryBase, IModalidadeRepository
    {
        public ModalidadeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
