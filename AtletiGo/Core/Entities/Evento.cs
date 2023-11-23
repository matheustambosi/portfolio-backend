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

        [Column("visivelsematletica")]
        public bool VisivelSemAtletica { get; set; }

        [Column("visivelcomatletica")]
        public bool VisivelComAtletica { get; set; }

        [Column("visivelatleta")]
        public bool VisivelAtleta { get; set; }

        [Column("codigomodalidade")]
        public Guid? CodigoModalidade { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }
    }
}
