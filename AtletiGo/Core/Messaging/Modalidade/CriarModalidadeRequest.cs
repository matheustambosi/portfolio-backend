using AtletiGo.Core.Exceptions;

namespace AtletiGo.Core.Messaging.Modalidade
{
    public class CriarModalidadeRequest
    {
        public string Descricao { get; set; }
        public bool BuscandoAtletas { get; set; }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new AtletiGoException("Descricao é obrigatório");
        }
    }
}
