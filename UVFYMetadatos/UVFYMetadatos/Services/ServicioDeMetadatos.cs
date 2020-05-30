using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using UVFYMetadatos.DAO;
using UVFYSesion;

namespace UVFYMetadatos
{
	public class ServicioDeMetadatos : Metadata.MetadataBase
	{
		private readonly ILogger<ServicioDeMetadatos> _logger;
		private GrpcChannel ServicioDeSesiones;
		public ServicioDeMetadatos(ILogger<ServicioDeMetadatos> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeSesiones = GrpcChannel.ForAddress("http://172.17.0.3:80", grpcChannelOptions);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesTodas(Token request, ServerCallContext context)
		{
			AdministradorDeSesiones.AdministradorDeSesionesClient cliente = new AdministradorDeSesiones.AdministradorDeSesionesClient(ServicioDeSesiones);
			SesionPeticion sesionPeticion = new SesionPeticion()
			{
				IdSesion = request.TokenDeAcceso
			};
			//Catch grpc exception
			ExistenciaDeSesion existenciaDeSesion = cliente.ExisteSesion(sesionPeticion);
			bool existeSesion = existenciaDeSesion.ExistenciaDeSesion_;
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Valida = true //existeSesion
			};
			if (true)//existeSesion)
			{
				CancionDAO cancionDAO = new CancionDAO();
				List<Models.Canciones> canciones = cancionDAO.CargarTodas();
				foreach (Models.Canciones cancion in canciones)
				{
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						IdArtista = cancion.ArtistaId,
						IdAlbum = cancion.AlbumsId
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			return Task.FromResult(respuesta);
		}
	}
}
