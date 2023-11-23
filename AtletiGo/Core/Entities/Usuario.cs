using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Messaging.Usuario;
using AtletiGo.Core.Utils.Enums;
using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("codigoatletica")]
        public Guid? CodigoAtletica { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("hashsenha")]
        public string HashSenha { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("tipousuario")]
        public TipoUsuario TipoUsuario { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }

        public void AlterarUsuario(CadastroUsuarioRequest request)
        {
            Nome = request.Nome;
            Email = request.Email;
            TipoUsuario = request.TipoUsuario;
            DtAlteracao = DateTime.Now;
        }
    }
}
