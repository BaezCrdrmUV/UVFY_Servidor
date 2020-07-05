using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVFYAuth;
using UVFYGateway.Dominio;

namespace UVFYGateway.Controllers
{
	[ApiController]
	[Route("Autenticacion")]
	public class AutenticacionController : ControllerBase
	{
		private GrpcChannel ServicioDeAutenticacion;

		private readonly ILogger<AutenticacionController> _logger;

		public AutenticacionController(ILogger<AutenticacionController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeAutenticacion = GrpcChannel.ForAddress("http://UVFYAuth:80", grpcChannelOptions);
		}

		[HttpPost]
		[Route("Registrar")]
		public IActionResult RegistrarUsuario([FromBody] Artista usuario)
		{
			IActionResult result = ValidationProblem();
			var clienteDeAutenticacion = new Authenticator.AuthenticatorClient(ServicioDeAutenticacion);
			try
			{
				if(usuario.TipoDeUsuario == TipoDeUsuario.Artista)
				{
					RegistrationRequest registrationRequest = new RegistrationRequest
					{
						Name = usuario.NombreDeusuario,
						Password = usuario.Contrasena,
						Email = usuario.CorreoElectronico,
						UserType = usuario.TipoDeUsuario.ToString(),
						NombreDeArtista = usuario.Nombre, 
						DescripcionDeArtista = usuario.Descripcion
					};
					ResgitrationResponse respuesta = clienteDeAutenticacion.RegisterUser(registrationRequest);
					if(respuesta.Response == true)
					{
						result = Ok();
					}
					else
					{
						result = ValidationProblem();
					}
				} 
				else if(usuario.TipoDeUsuario == TipoDeUsuario.Consumidor)
				{
					RegistrationRequest registrationRequest = new RegistrationRequest
					{
						Name = usuario.NombreDeusuario,
						Password = usuario.Contrasena,
						Email = usuario.CorreoElectronico,
						UserType = usuario.TipoDeUsuario.ToString()
					};
					ResgitrationResponse respuesta = clienteDeAutenticacion.RegisterUser(registrationRequest);
					if (respuesta.Response == true)
					{
						result = Ok();
					}
					else
					{
						result = ValidationProblem();
					}
				}
				else
				{
					result = BadRequest();
				}
			} 
			catch (RpcException e)
			{
				result = ValidationProblem();
			}
			catch (HttpRequestException e)
			{
				result = ValidationProblem();
			}
			return result;
		}

		[HttpPost]
		[Route("Autenticar")]
		public IActionResult AutenticarUsuario([FromBody] Usuario usuario)
		{
			IActionResult result = BadRequest();
			AuthRequest authRequest = new AuthRequest()
			{
				Name = usuario.CorreoElectronico,
				Password = usuario.Contrasena
			};
			var clienteDeAutenticacion = new Authenticator.AuthenticatorClient(ServicioDeAutenticacion);
			Authreply authreply = clienteDeAutenticacion.Authenticate(authRequest);
			return Ok(authreply);
		}
	}
}
