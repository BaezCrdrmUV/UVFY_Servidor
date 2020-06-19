using Grpc.Core;
using Grpc.Net.Client;
using LogicaDeNegocios.Excepciones;
using System;
using UVFYSesion;

namespace UVFYMetadatos.Servicios
{
	public class ValidacionDeSesiones
	{
		private GrpcChannel ServicioDeSesiones;

		public ValidacionDeSesiones()
		{
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeSesiones = GrpcChannel.ForAddress("http://172.17.0.4:80", grpcChannelOptions);

		}

		public bool ValidarSesion(string token)
		{
			bool resultado = false;
			AdministradorDeSesiones.AdministradorDeSesionesClient cliente = new AdministradorDeSesiones.AdministradorDeSesionesClient(ServicioDeSesiones);
			SesionPeticion sesionPeticion = new SesionPeticion()
			{
				IdSesion = token
			};
			ExistenciaDeSesion existenciaDeSesion = new ExistenciaDeSesion();
			try
			{
				existenciaDeSesion = cliente.ExisteSesion(sesionPeticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			catch (Grpc.Core.RpcException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			resultado = existenciaDeSesion.ExistenciaDeSesion_;
			return resultado;
		}

		public bool TokenTienePermisos(string token, int idUsuario)
		{
			bool resultado = false;
			AdministradorDeSesiones.AdministradorDeSesionesClient cliente = new AdministradorDeSesiones.AdministradorDeSesionesClient(ServicioDeSesiones);
			SesionPeticion sesionPeticion = new SesionPeticion()
			{
				IdSesion = token
			};
			ExistenciaDeSesion existenciaDeSesion = new ExistenciaDeSesion();
			try
			{
				existenciaDeSesion = cliente.ExisteSesion(sesionPeticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			catch (Grpc.Core.RpcException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			if (existenciaDeSesion.IdUsuario == idUsuario)
			{
				resultado = true;
			}
			return resultado;
		}

		public int ObtenerIdUsuarioPorToken(string token)
		{
			int idUsuario;
			AdministradorDeSesiones.AdministradorDeSesionesClient cliente = new AdministradorDeSesiones.AdministradorDeSesionesClient(ServicioDeSesiones);
			SesionPeticion sesionPeticion = new SesionPeticion()
			{
				IdSesion = token
			};

			UsuarioDeSesion usuarioDeSesion;

			try
			{
				usuarioDeSesion = cliente.ObtenerIdDeToken(sesionPeticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			catch (Grpc.Core.RpcException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}
			idUsuario = usuarioDeSesion.IdUsuario;
			return idUsuario;
		}
	}
}
