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
			ServicioDeAutenticacion = GrpcChannel.ForAddress("http://172.17.0.4:80", grpcChannelOptions);
		}

		[HttpPost]
		[Route("Registrar")]
		public IActionResult RegistrarUsuario([FromBody] Usuario usuario)
		{
			IActionResult result = BadRequest();
			var clienteDeAutenticacion = new Authenticator.AuthenticatorClient(ServicioDeAutenticacion);
			try
			{
				if(usuario.TipoDeUsuario == TipoDeUsuario.Artista)
				{
					Artista artista = (Artista)usuario;
					RegistrationRequest registrationRequest = new RegistrationRequest
					{
						Name = artista.NombreDeusuario,
						Password = artista.Contraseña,
						Email = artista.CorreoElectronico,
						UserType = artista.TipoDeUsuario.ToString(),
						NombreDeArtista = artista.Nombre, 
						DescripcionDeArtista = artista.Descripcion
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
						Password = usuario.Contraseña,
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
				Password = usuario.Contraseña
			};
			var clienteDeAutenticacion = new Authenticator.AuthenticatorClient(ServicioDeAutenticacion);
			Authreply authreply = clienteDeAutenticacion.Authenticate(authRequest);
			return Ok(authreply);
		}
	}
}
