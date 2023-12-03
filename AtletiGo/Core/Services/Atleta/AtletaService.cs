using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Messaging.RawQueryResult;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public List<GetAllAtletasRawQueryResult> GetAllAtletas(Guid codigoUsuario, Guid? codigoAtletica)
        {
            List<GetAllAtletasRawQueryResult> result;

            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            string sql = @$"SELECT
	                            atleta.codigo AS CodigoAtleta,
	                            usuario.nome AS NomeAtleta,
	                            modalidade.descricao AS Modalidade
                            FROM atleta atleta
                                LEFT JOIN usuario usuario
	                                ON usuario.codigo = atleta.codigousuario
                                LEFT JOIN modalidade modalidade
	                                ON modalidade.codigo = atleta.codigomodalidade
                            WHERE 1 = 1 ";

            string sqlWhere = "";

            if (usuario.TipoUsuario != TipoUsuario.Administrador)
            {
                sqlWhere += " AND usuario.codigoatletica = @CodigoAtletica";
                result = _atletaRepository.Query<GetAllAtletasRawQueryResult>(sql + sqlWhere,
                    new
                    {
                        CodigoAtletica = codigoAtletica
                    }).ToList();
            }
            else
            {
                result = _atletaRepository.Query<GetAllAtletasRawQueryResult>(sql + sqlWhere).ToList();
            }

            return result;
        }

        public void DesvincularAtletaAtletica(Guid codigoAtleta, Guid codigoUsuario, Guid? codigoAtletica)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            if (usuario.TipoUsuario != TipoUsuario.Administrador && usuario.CodigoAtletica != codigoAtletica)
                throw new AtletiGoException("Este atleta não pertence a sua atlética");

            var atleta = _atletaRepository.GetById<Entities.Atleta>(codigoAtleta);

            if (atleta == null)
                throw new AtletiGoException("Atleta não encontrado.");

            _atletaRepository.Delete(atleta);
        }

        public void SalvarModalidadeAtleta(Guid codigoUsuario, SalvarModalidadeAtletaRequest request)
        {
            var modalidadesAtuais = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoUsuario = codigoUsuario })?.ToList();

            foreach (var modalidadesAtivas in request.ModalidadesAtivas)
            {
                var modalidadeExiste = modalidadesAtuais?.Find(modalidadeAtual => modalidadesAtivas == modalidadeAtual.CodigoModalidade);

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

            modalidadesAtuais?.ForEach(modalidadeAtual => _atletaRepository.Delete(modalidadeAtual));
        }
    }
}
