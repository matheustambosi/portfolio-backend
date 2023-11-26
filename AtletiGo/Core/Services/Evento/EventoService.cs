using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Usuario;
using System;
using System.Linq;

namespace AtletiGo.Core.Services.Evento
{
    public class EventoService
    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAtletaRepository _atletaRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public void GetAllEventos(Guid codigoUsuario, Guid codigoAtletica)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);
            var atletaModalidades = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoUsuario = codigoUsuario }).ToList();

            var isAtleta = atletaModalidades.Any();
            var temAtletica = usuario.CodigoAtletica != null;

            string sql = $@"SELECT 
                                ev.Codigo, 
                                ev.NomeEvento, 
                                ev.EnderecoEvento, 
                                ev.DtEvento
	                        FROM
                                public.Evento ev
                            LEFT JOIN public.Atleta at
                                ON ev.CodigoModalidade = at.CodigoModalidade
	                        WHERE
                                1 == 1 ";

            string sqlWhere = @"";

            if (temAtletica)
                sqlWhere += " OR VisivelComAtletica = 1";

            if (!temAtletica)
                sqlWhere += " OR VisivelSemAtletica = 1";

            if (isAtleta)
                sqlWhere += " OR VisivelAtleta = 1";

            var eventos = _eventoRepository.Query<Entities.Evento>(sql + sqlWhere, new { CodigoAtletica = codigoAtletica });
        }
    }
}
