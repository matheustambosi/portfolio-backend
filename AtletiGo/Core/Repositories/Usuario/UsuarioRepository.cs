using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.Usuario
{
    public class UsuarioRepository : RepositoryBase, IUsuarioRepository
    {
        public UsuarioRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
