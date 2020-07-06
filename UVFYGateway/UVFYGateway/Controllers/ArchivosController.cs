using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVFYArchivos;
using UVFYMetadatos;

namespace UVFYGateway.Controllers
{
	[Route("Archivos")]
	[ApiController]
	public class ArchivosController : ControllerBase
	{
		private GrpcChannel ServicioDeArchivos;
		private readonly ILogger<ArchivosController> _logger;
		public ArchivosController(ILogger<ArchivosController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			grpcChannelOptions.MaxReceiveMessageSize = 20000000;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.6:80", grpcChannelOptions);
		}

		[HttpGet]
		[Route("CaratulaDeCancion")]
		public IActionResult CargarCaratulaDeCancionPorId([FromQuery]string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeArchivos = new UVFYArchivos.Archivos.ArchivosClient(ServicioDeArchivos);
			UVFYArchivos.PeticionId peticionId = new UVFYArchivos.PeticionId
			{
				Token = new UVFYArchivos.Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion
			};

			RespuestaDeCaratula respuesta = new RespuestaDeCaratula();
			try
			{
				respuesta = clienteDeArchivos.CargarCaratulaDeCancionPorId(peticionId);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				byte[] caratula = respuesta.Caratula.ToArray();
				actionResult = Ok(caratula);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("AudioDeCancion")]
		public IActionResult CargarAudioDeCancion([FromQuery]string tokenDeAcceso, int idCancion, int calidad)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeArchivos = new UVFYArchivos.Archivos.ArchivosClient(ServicioDeArchivos);
			PeticionIdYCalidad peticionId = new PeticionIdYCalidad
			{
				Token = new UVFYArchivos.Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion,
				Calidad = (calidad)calidad
			};

			RespuestaDeCancion respuesta = new RespuestaDeCancion();
			try
			{
				respuesta = clienteDeArchivos.CargarAudioDeCancionPorIdYCalidad(peticionId);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				byte[] audio = respuesta.Autio.ToArray();
				actionResult = Ok(audio);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("CaratulaDeAlbum")]
		public IActionResult CargarCaratulaDeAlbum([FromQuery]string tokenDeAcceso, int idAlbum)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeArchivos = new UVFYArchivos.Archivos.ArchivosClient(ServicioDeArchivos);
			UVFYArchivos.PeticionId peticionId = new UVFYArchivos.PeticionId
			{
				Token = new UVFYArchivos.Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idAlbum,
			};

			RespuestaDeCaratula respuesta = new RespuestaDeCaratula();
			try
			{
				respuesta = clienteDeArchivos.CargarCaratulaDeAlbumPorId(peticionId);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				byte[] caratula = respuesta.Caratula.ToArray();
				actionResult = Ok(caratula);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

	}
}