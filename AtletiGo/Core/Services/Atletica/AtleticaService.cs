using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Utils.Enums;
using System;
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

        public void CadastrarAtletica(CriarAtleticaRequest request)
        {
            request.Validar();

            var entity = new Entities.Atletica
            {
                Codigo = Guid.NewGuid(),
                Nome = request.NomeAtletica,
                Universidade = request.Universidade,
                Cidade = request.Cidade,
                Estado = request.Estado,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now,
                Situacao = Situacao.Ativo
            };

            _atleticaRepository.Insert(entity);
        }

        public void EditarAtletica(Guid codigo, CriarAtleticaRequest request)
        {
            var atletica = _atleticaRepository.GetById<Entities.Atletica>(codigo);

            if (atletica == null)
                throw new AtletiGoException("Atletica inválida.");

            atletica.AlterarAtletica(request);

            _atleticaRepository.Update(atletica);
        }

        public void DesativarAtletica(Guid codigo)
        {
            var atletica = _atleticaRepository.GetById<Entities.Atletica>(codigo);

            if (atletica == null)
                throw new AtletiGoException("Atletica inválida.");

            if (atletica.Situacao == Situacao.Inativo)
                throw new AtletiGoException("A atletica ja está desativada.");

            atletica.Situacao = Situacao.Inativo;

            _atleticaRepository.Update(atletica);
        }
    }
}
