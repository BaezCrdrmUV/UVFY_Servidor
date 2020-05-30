using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly ILogger<ControladorDeMetadatosController> _logger;

		public ControladorDeMetadatosController(ILogger<ControladorDeMetadatosController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeMetadatos = GrpcChannel.ForAddress("http://172.17.0.6:80", grpcChannelOptions);


			
		}

		[HttpGet]
		[Route("Canciones")]
		public IActionResult Todas()
		{

			string TokenDeAcceso = "bla";
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = TokenDeAcceso
			};
			//Catch grpc exception
			RespuestaDeCanciones respuesta = clienteDeMetadatos.CargarCancionesTodas(token);
			if (respuesta.Valida)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}

			return actionResult;
		}
	}
}