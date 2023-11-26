using System;
using System.Collections.Generic;

namespace AtletiGo.Core.Messaging.Modalidade
{
    public class GetAllModalidadesBuscandoAtletasResponse
    {
        public Guid CodigoModalidade { get; set; }
        public string Descricao { get; set; }
        public bool Inscrito { get; set; }

        public GetAllModalidadesBuscandoAtletasResponse(Entities.Modalidade entity, List<Entities.Atleta> modalidadesInscritas)
        {
            var jaInscrito = modalidadesInscritas.Exists(atletaModalidade => atletaModalidade.CodigoModalidade == entity.Codigo);

            CodigoModalidade = entity.Codigo;
            Descricao = entity.Descricao;
            Inscrito = jaInscrito;
        }
    }
}
