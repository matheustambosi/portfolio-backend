using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Linq;

namespace AtletiGo.Core.Services.Atleta
{
    public class AtletaService
    {
        private readonly IAtletaRepository _atletaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AtletaService(IAtletaRepository atletaRepository, IUsuarioRepository usuarioRepository)
        {
            _atletaRepository = atletaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public void SalvarModalidadeAtleta(Guid codigoUsuario, SalvarModalidadeAtletaRequest request)
        {
            var modalidadesAtuais = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoUsuario = codigoUsuario })?.ToList();

            foreach (var modalidadesAtivas in request.ModalidadesAtivas)
            {
                var modalidadeExiste = modalidadesAtuais.Find(modalidadeAtual => modalidadesAtivas == modalidadeAtual.CodigoModalidade);

                if (modalidadeExiste == null)
                {
                    var atleta = new Entities.Atleta
                    {
                        Codigo = Guid.NewGuid(),
                        CodigoUsuario = codigoUsuario,
                        CodigoModalidade = modalidadesAtivas
                    };

                    _atletaRepository.Insert(atleta);
                }
                else
                {
                    modalidadesAtuais.Remove(modalidadeExiste);
                }
            }

            modalidadesAtuais.ForEach(modalidadeAtual => _atletaRepository.Delete(modalidadeAtual));
        }
    }
}
