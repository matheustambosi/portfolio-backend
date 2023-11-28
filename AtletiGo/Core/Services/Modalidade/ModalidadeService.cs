using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Services.Modalidade
{
    public class ModalidadeService
    {
        private readonly IModalidadeRepository _modalidadeRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAtletaRepository _atletaRepository;

        public ModalidadeService(IModalidadeRepository modalidadeRepository, IUsuarioRepository usuarioRepository, IAtletaRepository atletaRepository)
        {
            _modalidadeRepository = modalidadeRepository;
            _usuarioRepository = usuarioRepository;
            _atletaRepository = atletaRepository;
        }

        public List<GetAllModalidadeResponse> GetAll(Guid codigoUsuario, Guid? codigoAtletica)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            var result = _modalidadeRepository.GetAll<Entities.Modalidade>(
                usuario.TipoUsuario != TipoUsuario.Administrador
                    ? new { CodigoAtletica = codigoAtletica, Situacao = 1 }
                    : null);

            return result?.Select(modalidade => new GetAllModalidadeResponse(modalidade))?.ToList();
        }

        public List<GetAllModalidadesBuscandoAtletasResponse> GetAllModalidadesBuscandoAtletas(Guid codigoUsuario, Guid codigoAtletica)
        {
            var usuario = _usuarioRepository.GetById<Entities.Usuario>(codigoUsuario);

            var modalidades = _modalidadeRepository.GetAll<Entities.Modalidade>(
                usuario?.TipoUsuario != TipoUsuario.Administrador ?
                new
                {
                    CodigoAtletica = codigoAtletica,
                    BuscandoAtletas = true,
                    Situacao = 1
                } : null).ToList();

            var modalidadesInscritas = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoUsuario = codigoUsuario }).ToList();

            return modalidades?.Select(modalidade => new GetAllModalidadesBuscandoAtletasResponse(modalidade, modalidadesInscritas))?.ToList();
        }

        public void CadastrarModalidade(CriarModalidadeRequest request, Guid codigoAtletica)
        {
            request.Validar();

            var modalidade = _modalidadeRepository.GetAll<Entities.Modalidade>(new { Descricao = request.Descricao }).FirstOrDefault();

            if (modalidade != null)
            {
                if (modalidade.Situacao == Situacao.Ativo)
                    throw new AtletiGoException("Já existe uma modalidade com esta descrição.");

                modalidade.AlterarModalidade(request);

                _modalidadeRepository.Update(modalidade);
            }
            else
            {
                modalidade = new Entities.Modalidade
                {
                    Codigo = Guid.NewGuid(),
                    CodigoAtletica = codigoAtletica,
                    Descricao = request.Descricao,
                    BuscandoAtletas = request.BuscandoAtletas,
                    Situacao = Situacao.Ativo,
                    DtCriacao = DateTime.Now,
                    DtAlteracao = DateTime.Now
                };

                _modalidadeRepository.Insert(modalidade);
            }
        }

        public void EditarModalidade(Guid codigo, CriarModalidadeRequest request)
        {
            var modalidade = _modalidadeRepository.GetById<Entities.Modalidade>(codigo);

            if (modalidade == null)
                throw new AtletiGoException("Modalidade inválida.");

            modalidade.AlterarModalidade(request);

            _modalidadeRepository.Update(modalidade);
        }

        public void DeletarModalidadeAtletica(Guid codigoModalidade)
        {
            var modalidade = _modalidadeRepository.GetById<Entities.Modalidade>(codigoModalidade);

            if (modalidade == null)
                throw new AtletiGoException("Modalidade inválida.");

            var atletas = _atletaRepository.GetAll<Entities.Atleta>(new { CodigoModalidade = modalidade.Codigo }).ToList();

            atletas.ForEach(atleta =>
            {
                _atletaRepository.Delete(atleta);
            });

            modalidade.Situacao = Situacao.Inativo;
            modalidade.DtAlteracao = DateTime.Now;

            _modalidadeRepository.Update(modalidade);
        }
    }
}
