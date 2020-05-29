using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace UVFYSesion
{
	public class ServicioDeSesiones : AdministradorDeSesiones.AdministradorDeSesionesBase
	{
		private readonly ILogger<ServicioDeSesiones> _logger;
		private ControladorDeSesiones ControladorDeSesiones = ControladorDeSesiones.Instancia;
		public ServicioDeSesiones(ILogger<ServicioDeSesiones> logger)
		{
			_logger = logger;
		}

		public override Task<SesionCreada> NuevaSesion(UsuarioDeSesion request, ServerCallContext context)
		{
			SesionCreada respuesta = new SesionCreada();
			respuesta.IdSesion = new Guid().ToString();
			respuesta.IdSesion = ControladorDeSesiones.AñadirSesion(request.IdUsuario);
			return Task.FromResult(respuesta);
		}
	}
}
