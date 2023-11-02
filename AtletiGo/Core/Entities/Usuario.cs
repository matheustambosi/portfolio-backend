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

        [Column("tipousuario")]
        public TipoUsuario TipoUsuario { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }
    }
}
