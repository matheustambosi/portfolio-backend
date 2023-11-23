using AtletiGo.Core.Utils.Enums;
using System;

namespace AtletiGo.Core.Messaging.Usuario
{
    public class GetAllUsuarioResponse
    {
        public Guid Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        public GetAllUsuarioResponse(Entities.Usuario usuario)
        {
            Codigo = usuario.Codigo;
            Nome= usuario.Nome;
            Email = usuario.Email;
            TipoUsuario = usuario.TipoUsuario;
            DtCriacao= usuario.DtCriacao;
            DtAlteracao = usuario.DtAlteracao;
        }
    }
}
