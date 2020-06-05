using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

		public override Task<SesionPeticion> NuevaSesion(UsuarioDeSesion request, ServerCallContext context)
		{
			SesionPeticion respuesta = new SesionPeticion();
			respuesta.IdSesion = new Guid().ToString();
			respuesta.IdSesion = ControladorDeSesiones.AñadirSesion(request.IdUsuario);
			return Task.FromResult(respuesta);
		}

		public override Task<ExistenciaDeSesion> ExisteSesion(SesionPeticion request, ServerCallContext context)
		{

			ExistenciaDeSesion existenciaDeSesion = new ExistenciaDeSesion()
			{
				ExistenciaDeSesion_ = ControladorDeSesiones.SesionExiste(request.IdSesion)
			};

			if (existenciaDeSesion.ExistenciaDeSesion_)
			{
				existenciaDeSesion.IdUsuario = ControladorDeSesiones.ObtenerIdUsuarioPorIdSesion(request.IdSesion);
			}
			else
			{
				existenciaDeSesion.IdUsuario = 0;
			}

			return Task.FromResult(existenciaDeSesion);
		}

	}
}
