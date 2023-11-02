using Microsoft.Extensions.Configuration;

namespace AtletiGo.Core.Repositories.Evento
{
    public class EventoRepository : RepositoryBase, IEventoRepository
    {
        public EventoRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
