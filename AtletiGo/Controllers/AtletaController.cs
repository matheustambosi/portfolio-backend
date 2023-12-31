﻿using AtletiGo.Core.Entities;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging;
using AtletiGo.Core.Messaging.Modalidade;
using AtletiGo.Core.Messaging.RawQueryResult;
using AtletiGo.Core.Services.Atleta;
using AtletiGo.Core.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace AtletiGo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AtletaController : ControllerBase
    {
        private readonly ILogger<AtletaController> _logger;

        private readonly AtletaService _atletaService;

        public AtletaController(ILogger<AtletaController> logger, AtletaService atletaService)
        {
            _logger = logger;
            _atletaService = atletaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GetAllAtletasRawQueryResult>> GetAll()
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                var response = _atletaService.GetAllAtletas(codigoUsuario, codigoAtletica.GetValueOrDefault());

                return Ok(response);
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }

        [HttpDelete("{codigo}")]
        public IActionResult Delete(Guid codigo)
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();
                var codigoAtletica = GetCodigoAtletica();

                _atletaService.DesvincularAtletaAtletica(codigo, codigoUsuario, codigoAtletica.GetValueOrDefault());

                return NoContent();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }

        [HttpPost("SalvarAtleta")]
        public IActionResult Criar([FromBody] SalvarModalidadeAtletaRequest request)
        {
            try
            {
                var codigoUsuario = GetCodigoUsuario();

                _atletaService.SalvarModalidadeAtleta(codigoUsuario, request);

                return Ok();
            }
            catch (AtletiGoException atEx)
            {
                return BadRequest(ResponseBase.ErroAtletiGo(atEx));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseBase.ErroGenerico());
            }
        }
    }
}
