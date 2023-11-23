using AtletiGo.Core.Utils.Enums;
using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("atleta")]
    public class Atleta
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("codigomodalidade")]
        public Guid CodigoModalidade { get; set; }

        [Column("codigousuario")]
        public Guid CodigoUsuario { get; set; }

        [Column("situacaoatleta")]
        public SituacaoAtleta SituacaoAtleta { get; set; }
    }
}
