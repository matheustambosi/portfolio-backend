using AtletiGo.Core.Repositories.Atletica;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Services.Atletica
{
    public class AtleticaService
    {
        public readonly IAtleticaRepository _atleticaRepository;

        public AtleticaService(IAtleticaRepository atleticaRepository)
        {
            _atleticaRepository = atleticaRepository;
        }

        public List<Entities.Atletica> GetAll()
        {
            return _atleticaRepository.GetAll<Entities.Atletica>()?.ToList();
        }

        public void Insert(Entities.Atletica entity)
        {
            _atleticaRepository.Insert(entity);
        }
    }
}
