using System;
using System.Collections.Generic;

namespace AtletiGo.Core.Messaging.Evento
{
    public class GetAllEventoResponse
    {
        public DateTime DtEvento { get; set; }
        public List<GetAllEventoResponseData> Eventos { get; set; }
    }

    public class GetAllEventoResponseData
    {
        public Guid Codigo { get; set; }
        public DateTime DtEvento { get; set; }
        public string NomeEvento { get; set; }
        public string EnderecoEvento { get; set; }

        public GetAllEventoResponseData(Entities.Evento entity)
        {
            Codigo = entity.Codigo;
            DtEvento = entity.DtEvento;
            NomeEvento = entity.NomeEvento;
            EnderecoEvento = entity.EnderecoEvento;
        }
    }
}
