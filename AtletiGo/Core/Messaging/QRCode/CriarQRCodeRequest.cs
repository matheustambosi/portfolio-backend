using AtletiGo.Core.Exceptions;
using System;

namespace AtletiGo.Core.Messaging.QRCode
{
    public class CriarQRCodeRequest
    {
        public string Descricao { get; set; }
        public int DuracaoDias { get; set; }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Descricao))
                throw new AtletiGoException("Descricao é obrigatório");

            if (DuracaoDias < 1)
                throw new AtletiGoException("A duração do qrcode precisa ser maior que 0.");
        }
    }
}
