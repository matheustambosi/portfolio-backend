using AtletiGo.Core.Utils.Enums;

namespace AtletiGo.Core.Messaging.Autenticacao
{
    public class LoginResponse
    {
        public string Nome { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public string Token { get; set; }

        public LoginResponse(Entities.Usuario usuario, string token)
        {
            Nome = usuario.Nome;
            TipoUsuario = usuario.TipoUsuario;
            Token = token;
        }
    }
}
