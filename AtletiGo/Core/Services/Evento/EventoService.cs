using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Evento;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AtletiGo.Core.Services.Evento
{
    public class EventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAtletaRepository _atletaRepository;

        public EventoService(IEventoRepository eventoRepository, IUsuarioRepository usuarioRepository, IAtletaRepository atletaRepository)
        {
            _eventoRepository = eventoRepository;
            _usuarioRepository = usuarioRepository;
            _atletaRepository = atletaRepository;
        }

        public List<GetAllEventoResponse> GetAllEventos(Guid codigoUsuario, Guid codigoAtletica)
        {
            var result = new List<Entities.Evento>();

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            var eventosSemAtletica = _eventoRepository.GetAll<Entities.Evento>(
                    new
                    {
                        VisivelSemAtletica = true,
                        Situacao = 1
                    }).ToList();

            result.AddRange(eventosSemAtletica);

            if (usuario.CodigoAtletica != null)
            {
                var atletaModalidades = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoUsuario = codigoUsuario }).ToList();
                var isAtleta = atletaModalidades.Any();

                if (isAtleta)
                {
                    atletaModalidades.ForEach(modalidade =>
                    {
                        var eventosModalidade = _eventoRepository.GetAll<Entities.Evento>(
                            new
                            {
                                CodigoModalidade = modalidade.CodigoModalidade,
                                CodigoAtletica = codigoAtletica,
                                VisivelAtleta = true,
                                Situacao = 1
                            }).ToList();

                        result.AddRange(eventosModalidade);
                    });
                }

                var eventosAtletica = _eventoRepository.GetAll<Entities.Evento>(
                    new
                    {
                        CodigoAtletica = codigoAtletica,
                        VisivelComAtletica = true,
                        Situacao = 1
                    });

                result.AddRange(eventosAtletica);
            }

            result = result.Distinct().ToList();

            var response = new List<GetAllEventoResponse>();

            var eventosAgrupadosPorData = result.GroupBy(evento => evento.DtEvento);

            foreach(var eventoAgrupadoData in eventosAgrupadosPorData)
            {
                var dtEvento = new GetAllEventoResponse
                {
                    DtEvento = eventoAgrupadoData.Key,
                    Eventos = new List<GetAllEventoResponseData>()
                };

                foreach (var eventoData in eventoAgrupadoData)
                {
                    var evento = new GetAllEventoResponseData(eventoData);

                    dtEvento.Eventos.Add(evento);
                }

                response.Add(dtEvento);
            }

            return response;
        }

        public void CriarEvento(CriarEventoRequest request, Guid codigoAtletica)
        {
            request.Validar();

            var evento = new Entities.Evento
            {
                Codigo = Guid.NewGuid(),
                CodigoAtletica = codigoAtletica,
                NomeEvento = request.NomeEvento,
                EnderecoEvento = request.EnderecoEvento,
                DtEvento = request.DtEvento,
                Situacao = request.Situacao,
                VisivelAtleta = request.VisivelAtleta,
                CodigoModalidade = request.CodigoModalidade,
                VisivelComAtletica = request.VisivelComAtletica,
                VisivelSemAtletica = request.VisivelSemAtletica,
                DtCriacao = DateTime.Now,
                DtAlteracao = DateTime.Now
            };

            _eventoRepository.Insert(evento);
        }

        public void EditarEvento(Guid codigo, CriarEventoRequest request)
        {
            var evento = _usuarioRepository.GetById<Entities.Evento>(codigo);

            if (evento == null)
                throw new AtletiGoException("Evento inválido.");

            evento.AlterarEvento(request);

            _eventoRepository.Update(evento);
        }

        public void DesativarEvento(Guid codigo, Guid codigoUsuario)
        {
            var evento = _eventoRepository.GetById<Entities.Evento>(codigo);
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            if (evento is null)
                throw new AtletiGoException("Evento inválido.");

            if (usuario.TipoUsuario != TipoUsuario.Administrador && usuario.CodigoAtletica != evento.CodigoAtletica)
                throw new AtletiGoException("Este evento não pertence a sua atlética.");

            evento.Situacao = Situacao.Inativo;

            _eventoRepository.Update(evento);
        }
    }
}
