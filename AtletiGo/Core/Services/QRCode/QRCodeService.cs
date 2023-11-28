using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.QRCode;
using AtletiGo.Core.Messaging.Usuario;
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

        public List<GetAllQrCodeResponse> GetAll(Guid codigoUsuario, Guid? codigoAtletica)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            var result = _qrCodeRepository.GetAll<Entities.QRCode>(
                usuario.TipoUsuario != TipoUsuario.Administrador
                    ? new { CodigoAtletica = codigoAtletica, Situacao = 1 }
                    : null);

            return result?.Select(qrCode => new GetAllQrCodeResponse(qrCode))?.ToList();
        }

        public Guid CriarQRCode(CriarQRCodeRequest request, Guid codigoAtletica)
        {
            var qrCode = new Entities.QRCode
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = codigoAtletica,
                DtCriacao = DateTime.Now,
                Descricao = request.Descricao,
                DuracaoDias = request.DuracaoDias,
                Situacao = Situacao.Ativo
            };

            _qrCodeRepository.Insert(qrCode);

            return qrCode.Codigo;
        }

        public void AssociarQRCode(Guid codigoUsuario, Guid codigoQrCode)
        {
            var qrcode = _qrCodeRepository.GetById<Entities.QRCode>(codigoQrCode);

            if (qrcode is null)
                throw new AtletiGoException("O QRCode informado não foi encontrado.");

            if (qrcode.DtCriacao.AddDays(qrcode.DuracaoDias) < DateTime.Now)
                throw new AtletiGoException("O QRCode escaneado já expirou.");

            if (qrcode.Situacao == Situacao.Inativo)
                throw new AtletiGoException("O QRCode escaneado está inativo.");

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            usuario.CodigoAtletica = qrcode.CodigoAtletica;
            usuario.TipoUsuario = TipoUsuario.Universitario;

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
