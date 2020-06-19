using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYArchivos;

namespace UVFYGuardadoDeArchivos
{
	public class GuardadoDeArchivos
	{
		private GrpcChannel ServicioDeArchivos;

		public GuardadoDeArchivos()
		{
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.6:80", grpcChannelOptions);
		}


		public bool GuardarAudioDeCancionDeArtista(int idCancion, byte[] audio, calidad calidad)
		{
			UVFYArchivos.Respuesta respuesta = new UVFYArchivos.Respuesta();
			var cliente = new Archivos.ArchivosClient(ServicioDeArchivos);
			UVFYArchivos.PeticionGuardadoIdYCalidad peticion = new UVFYArchivos.PeticionGuardadoIdYCalidad();
			peticion.Calidad = calidad;
			peticion.Datos = Google.Protobuf.ByteString.CopyFrom(audio);
			peticion.IdPeticion = idCancion;

			try
			{
				respuesta = cliente.GuardarAudioDeCancionPorIdYCalidad(peticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Archivos", e);
			}
			catch (RpcException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}

			if (respuesta.Exitosa)
			{
				return true;
			}
			else
			{
				throw new ResultadoDeServicioFallidoException("Sesiones");
			}
		}
	}
}
