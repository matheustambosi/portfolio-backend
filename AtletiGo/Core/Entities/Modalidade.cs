using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("modalidade")]
    public class Modalidade
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("buscandoatletas")]
        public bool BuscandoAtletas { get; set; }
    }
}
