﻿using System.Security.Claims;
using System;
using AtletiGo.Core.Exceptions;

namespace AtletiGo.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected Guid? GetCodigoAtletica()
        {
            var codigoAtletica = User.FindFirstValue("codigo_atletica");

            if (!string.IsNullOrWhiteSpace(codigoAtletica))
                return new Guid(codigoAtletica);

            return null;
        }

        protected Guid GetCodigoUsuario()
        {
            var codigoOperador = User.FindFirstValue("codigo_operador");
            if (codigoOperador == null)
            {
                throw new AtletiGoException("Código do usuário não identificado.");
            }

            return new Guid(codigoOperador);
        }
    }
}
