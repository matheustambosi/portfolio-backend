using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.Atletica
{
    public class AtleticaRepository : RepositoryBase, IAtleticaRepository
    {
        public AtleticaRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
