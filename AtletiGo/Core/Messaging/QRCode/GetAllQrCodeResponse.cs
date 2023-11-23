using System;

namespace AtletiGo.Core.Messaging.QRCode
{
    public class GetAllQrCodeResponse
    {
        public Guid Codigo { get; set; }
        public string Descricao { get; set; }
        public int DuracaoDias { get; set; }

        public GetAllQrCodeResponse(Entities.QRCode entity)
        {
            Codigo = entity.Codigo;
            Descricao = entity.Descricao;
            DuracaoDias = entity.DuracaoDias;
        }
    }
}
