using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("atletica")]
    public class Atletica
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }
    }
}
