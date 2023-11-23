using AtletiGo.Core.Utils.Enums;
using Dapper;
using System;

namespace AtletiGo.Core.Entities
{
    [Table("qrcode")]
    public class QRCode
    {
        [Key]
        [Column("codigo")]
        public Guid Codigo { get; set; }

        [Column("codigoatletica")]
        public Guid CodigoAtletica { get; set; }

        [Column("situacao")]
        public Situacao Situacao { get; set; }

        [Column("descricao")]
        public string Descricao { get; set; }

        [Column("duracaodias")]
        public int DuracaoDias { get; set; }

        [Column("dtcriacao")]
        public DateTime DtCriacao { get; set; }
    }
}
