using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("evento")]
    public class Evento
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("codigoatletica")]
        public Guid CodigoAtletica { get; set; }

        [Column("dtevento")]
        public DateTime DtEvento { get; set; }

        [Column("nomeevento")]
        public string NomeEvento { get; set; }

        [Column("enderecoevento")]
        public string EnderecoEvento { get; set; }
        public bool MostrarTodos { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }
    }
}
