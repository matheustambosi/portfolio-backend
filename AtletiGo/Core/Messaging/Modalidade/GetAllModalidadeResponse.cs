using AtletiGo.Core.Utils.Enums;
using System;

namespace AtletiGo.Core.Messaging.Modalidade
{
    public class GetAllModalidadeResponse
    {
        public Guid Codigo { get; set; }
        public string Descricao { get; set; }
        public bool BuscandoAtletas { get; set; }

        public GetAllModalidadeResponse(Entities.Modalidade entity)
        {
            Codigo = entity.Codigo;
            Descricao = entity.Descricao;
            BuscandoAtletas = entity.BuscandoAtletas;
        }
    }
}
