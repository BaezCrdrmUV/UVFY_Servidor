using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using UVFYAuth.DAOS;
using UVFYAuth.Dominio;
using UVFYAuth.LocalServices;
using UVFYAuth.Models;
using UVFYSesion;

namespace UVFYAuth.ExposedServices
{
	public class AuthService : Authenticator.AuthenticatorBase
	{
		private readonly ILogger<AuthService> _logger;
		private GrpcChannel ServicioDeSesiones;
		public AuthService(ILogger<AuthService> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeSesiones = GrpcChannel.ForAddress("http://172.17.0.4:80", grpcChannelOptions);
		}

		public override Task<Authreply> Authenticate(AuthRequest request, ServerCallContext context)
		{
			Usuario usuarioRespuesta = new Usuario();
			Authreply respuesta = new Authreply()
			{
				Response = false,
				Token = string.Empty
			};
			if(AuthenticationServices.AuthenticateUserCredentials(request.Name, request.Password))
			{
				UVFYSesion.AdministradorDeSesiones.AdministradorDeSesionesClient clienteDeSesiones = new AdministradorDeSesiones.AdministradorDeSesionesClient(ServicioDeSesiones);
				usuarioRespuesta = UsuarioDAO.CargarUsuarioPorCorreo(request.Name);
				TipoDeUsuario tipoDeUsuario = UsuarioDAO.ObtenerTipoDeUsuarioPorID(usuarioRespuesta.Id);
				UsuarioDeSesion usuarioDeSesion = new UsuarioDeSesion()
				{
					IdUsuario = usuarioRespuesta.Id
				};
				SesionCreada sesionCreada = clienteDeSesiones.NuevaSesion(usuarioDeSesion);
				respuesta = new Authreply()
				{
					Response = true,
					Token = sesionCreada.IdSesion,
					IdUsuario = usuarioRespuesta.Id,
					TipoDeUsuario = (TipoDeUsuarioRespuesta)(int)tipoDeUsuario
				};
			}
			return Task.FromResult(respuesta);
		}

		public override Task<ResgitrationResponse> RegisterUser(RegistrationRequest request, ServerCallContext context)
		{
			ResgitrationResponse respuesta = new ResgitrationResponse()
			{
				Response = false
			};
			Usuario usuario = new Usuario()
			{
				CorreoElectronico = request.Email,
				NombreDeUsuario = request.Name,
				Contrasena = ServiciosDeCifrado.EncriptarCadena(request.Password)
			};
			if (request.UserType == TipoDeUsuario.Artista.ToString())
			{
				usuario.UsuariosArtista = new UsuarioArtista()
				{
					Descripcion = request.DescripcionDeArtista,
					Nombre = request.NombreDeArtista

				};
			}
			else if (request.UserType == TipoDeUsuario.Consumidor.ToString())
			{
				usuario.UsuariosConsumidor = new UsuarioConsumidor();
				usuario.UsuariosConsumidor.FechaDeFinalDeSuscripcion = DateTime.Now.AddDays(30);
			}
			if (usuario.Validar())
			{

				UsuarioDAO.GuardarUsuario(usuario);
				respuesta.Response = true;
			}
			return Task.FromResult(respuesta);
		}
	}
}
