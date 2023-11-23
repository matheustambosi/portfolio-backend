using AtletiGo.Core.Utils.Enums;
using System;

namespace AtletiGo.Core.Messaging.Autenticacao
{
    public class LoginResponse
    {
        public Guid CodigoOperador { get; set; }
        public string Nome { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public string Token { get; set; }

        public LoginResponse(Entities.Usuario usuario, string token)
        {
            CodigoOperador = usuario.Codigo;
            Nome = usuario.Nome;
            TipoUsuario = usuario.TipoUsuario;
            Token = token;
        }
    }
}
