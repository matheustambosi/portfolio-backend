using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Utils.Enums;
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

        [Column("codigoatletica")]
        public Guid CodigoAtletica { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("buscandoatletas")]
        public bool BuscandoAtletas { get; set; }

        [Column("situacao")]
        public Situacao Situacao { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }

        [Column("dtalteracao")]
        public DateTime DtAlteracao { get; set; }

        public void AlterarModalidade(CriarModalidadeRequest request)
        {
            Descricao = request.Descricao;
            BuscandoAtletas = request.BuscandoAtletas;
            Situacao = Situacao.Ativo;
            DtAlteracao = DateTime.Now;
        }
    }
}
