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
    public class ControladorDeMetadatosController : ControllerBase
    {
		private GrpcChannel ServicioDeMetadatos;
		private GrpcChannel ServicioDeArchivos;

		private readonly ILogger<ControladorDeMetadatosController> _logger;

		public ControladorDeMetadatosController(ILogger<ControladorDeMetadatosController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeMetadatos = GrpcChannel.ForAddress("http://172.17.0.5:80", grpcChannelOptions);
			ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.2:80", grpcChannelOptions);


		}

		[HttpGet]
		[Route("Canciones")]
		public IActionResult Todas([FromBody] string TokenDeAcceso)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = TokenDeAcceso
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
		[Route("Cancion")]
		public IActionResult CargarCancionPorId()
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = "bla"
				},
				IdPeticion = 4
			};
			
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones();

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

		[HttpPost]
		[Route("GuardarCancion")]
		public IActionResult GuardarCancion([FromBody] string TokenDeAcceso)
		{
			var clienteDeArchivos = new UVFYArchivos.Archivos.ArchivosClient(ServicioDeArchivos);
			IActionResult actionResult = BadRequest();

			return actionResult;
		}
	}
}