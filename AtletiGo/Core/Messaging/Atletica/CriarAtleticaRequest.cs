using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Utils.Enums;

namespace AtletiGo.Core.Messaging.Atletica
{
    public class CriarAtleticaRequest
    {
        public string NomeAtletica { get; set; }
        public string Universidade { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public Situacao Situacao { get; set; }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(NomeAtletica))
                throw new AtletiGoException("Nome da atletica é obrigatório.");
        }
    }
}
