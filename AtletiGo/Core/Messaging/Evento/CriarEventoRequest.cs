using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Utils.Enums;
using System;

namespace AtletiGo.Core.Messaging.Evento
{
    public class CriarEventoRequest
    {
        public string NomeEvento { get; set; }
        public string EnderecoEvento { get; set; }
        public DateTime DtEvento { get; set; }
        public bool VisivelSemAtletica { get; set; }
        public bool VisivelComAtletica { get; set; }
        public bool VisivelAtleta { get; set; }
        public Guid? CodigoModalidade { get; set; }
        public Situacao Situacao { get; set; }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(NomeEvento))
                throw new AtletiGoException("Nome do evento é obrigatório");
        }
    }
}
