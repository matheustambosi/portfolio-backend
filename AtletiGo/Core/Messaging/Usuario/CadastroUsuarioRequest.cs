using AtletiGo.Core.Utils.Enums;

namespace AtletiGo.Core.Messaging.Usuario
{
    public class CadastroUsuarioRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        public void Validar()
        {

        }
    }
}
