using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Evento;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var eventos = new List<GetAllEventoResponse>();

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            var eventosSemAtletica = _eventoRepository.GetAll<Entities.Evento>(
                    new
                    {
                        VisivelSemAtletica = true,
                        Situacao = 1
                    });

            eventos.AddRange(eventosSemAtletica.Select(evento => new GetAllEventoResponse(evento)).ToList());

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
                            });

                        eventos.AddRange(eventosModalidade.Select(evento => new GetAllEventoResponse(evento)).ToList());
                    });
                }

                var eventosAtletica = _eventoRepository.GetAll<Entities.Evento>(
                    new
                    {
                        CodigoAtletica = codigoAtletica,
                        VisivelComAtletica = true,
                        Situacao = 1
                    });

                eventos.AddRange(eventosAtletica.Select(evento => new GetAllEventoResponse(evento)).ToList());
            }

            eventos = eventos.Distinct().ToList();

            return eventos;
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
