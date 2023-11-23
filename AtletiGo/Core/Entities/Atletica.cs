using AtletiGo.Core.Messaging.Atletica;
using AtletiGo.Core.Utils.Enums;
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

        [Column("universidade")]
        public string Universidade { get; set; }

        [Column("cidade")]
        public string Cidade { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("situacao")]
        public Situacao Situacao { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }

        public void AlterarAtletica(CriarAtleticaRequest request)
        {
            Nome = request.NomeAtletica;
            Universidade = request.Universidade;
            Cidade = request.Cidade;
            Estado = request.Estado;
            Situacao= request.Situacao;
            DtAlteracao = DateTime.Now;
        }
    }
}
