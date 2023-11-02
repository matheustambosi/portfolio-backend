using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.QRCode;
using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Services.QRCode
{
    public class QRCodeService
    {
        public readonly IQRCodeRepository _qrCodeRepository;
        public readonly IUsuarioRepository _usuarioRepository;

        public QRCodeService(IQRCodeRepository qrCodeRepository, IUsuarioRepository usuarioRepository)
        {
            _qrCodeRepository = qrCodeRepository;
            _usuarioRepository = usuarioRepository;
        }

        public List<Entities.QRCode> Listar()
        {
            return _qrCodeRepository.GetAll<Entities.QRCode>()?.ToList();
        }

        public Guid CriarQRCode(CriarQRCodeRequest request, Guid codigoAtletica)
        {
            var qrCode = new Entities.QRCode
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = codigoAtletica,
                DtCriacao = DateTime.Now,
                DtExpiracao = request.DtExpiracao
            };

            _qrCodeRepository.Insert(qrCode);

            return qrCode.Codigo;
        }

        public void AssociarQRCode(Guid codigoUsuario, Guid codigoQrCode)
        {
            var qrcode = _qrCodeRepository.GetById<Entities.QRCode>(codigoQrCode);

            if (qrcode.DtExpiracao < DateTime.Now)
                throw new AtletiGoException("O QRCode escaneado já expirou.");

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            usuario.CodigoAtletica = qrcode.CodigoAtletica;

            _usuarioRepository.Update(usuario);
        }

        public void InativarQRCode(Guid codigoQRCode)
        {
            var qrCode = _qrCodeRepository.GetById<Entities.QRCode>(codigoQRCode);

            if (qrCode is null)
                throw new AtletiGoException("QRCode inválido.");

            qrCode.Situacao = Situacao.Inativo;

            _qrCodeRepository.Update(qrCode);
        }
    }
}
