using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Utils.Enums;

namespace AtletiGo.Core.Messaging.Usuario
{
    public class CadastroUsuarioRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        public void Validar()
        {
            if (!System.Enum.IsDefined(typeof(TipoUsuario), TipoUsuario))
                throw new AtletiGoException("Tipo de usuario invalido.");
            if (string.IsNullOrWhiteSpace(Nome))
                throw new AtletiGoException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new AtletiGoException("Email é obrigatório.");
        }
    }
}
