using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.Atleta
{
    public class AtletaRepository : RepositoryBase, IAtletaRepository
    {
        public AtletaRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
