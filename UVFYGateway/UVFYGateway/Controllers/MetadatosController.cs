using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVFYMetadatos;

namespace UVFYGateway.Controllers
{
    [Route("Metadatos")]
    [ApiController]
    public class MetadatosController : ControllerBase
    {
		private GrpcChannel ServicioDeMetadatos;
		

		private readonly ILogger<MetadatosController> _logger;

		public MetadatosController(ILogger<MetadatosController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeMetadatos = GrpcChannel.ForAddress("http://172.17.0.7:80", grpcChannelOptions);

		}
		#region Cancion
		[HttpGet]
		[Route("Canciones/Todas")]
		public IActionResult Todas([FromQuery] string tokenDeAcceso)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = tokenDeAcceso
			};
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones();
			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesTodas(token);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}
			
			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorID")]
		public IActionResult CargarCancionPorId([FromQuery]string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion
			};
			
			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorArtista")]
		public IActionResult CargarCancionesPorIdArtista([FromQuery]string tokenDeAcceso, int idArtista)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idArtista
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdArtista(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorAlbum")]
		public IActionResult CargarCancionesPorIdAlbum([FromQuery]string tokenDeAcceso, int idAlbum)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idAlbum
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdAlbum(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorPlaylist")]
		public IActionResult CargarCancionesPorIdPlaylist([FromQuery]string tokenDeAcceso, int idPlaylist)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idPlaylist
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorGenero")]
		public IActionResult CargarCancionesPorIdGenero([FromQuery]string tokenDeAcceso, int idGenero)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idGenero
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdGenero(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}
		#endregion Cancion
	}
}