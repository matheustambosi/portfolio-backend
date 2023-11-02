using System.Security.Claims;
using System;

namespace AtletiGo.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected Guid GetCodigoAtletica()
        {
            var codigoAtletica = User.FindFirstValue("codigo_atletica");
            if (codigoAtletica == null)
            {
                throw new Exception("Código da atlética não identificado.");
            }

            return new Guid(codigoAtletica);
        }

        protected Guid GetCodigoUsuario()
        {
            var codigoOperador = User.FindFirstValue("codigo_usuario");
            if (codigoOperador == null)
            {
                throw new Exception("Código do usuário não identificado.");
            }

            return new Guid(codigoOperador);
        }
    }
}
