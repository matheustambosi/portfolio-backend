using System;

namespace AtletiGo.Core.Messaging.Evento
{
    public class GetAllEventoResponse
    {
        public Guid Codigo { get; set; }
        public DateTime DtEvento { get; set; }
        public string NomeEvento { get; set; }
        public string EnderecoEvento { get; set; }

        public GetAllEventoResponse(Entities.Evento entity)
        {
            Codigo = entity.Codigo;
            DtEvento = entity.DtEvento;
            NomeEvento = entity.NomeEvento;
            EnderecoEvento = entity.EnderecoEvento;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            GetAllEventoResponse other = (GetAllEventoResponse)obj;
            return Codigo == other.Codigo;
        }

        public override int GetHashCode()
        {
            return Codigo.GetHashCode();
        }
    }
}
