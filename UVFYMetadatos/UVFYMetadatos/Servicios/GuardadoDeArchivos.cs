using Grpc.Core;
using Grpc.Net.Client;
using LogicaDeNegocios.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYGuardadoDeArchivos;
using UVFYMetadatos.Exceptiones;

namespace UVFYMetadatos.Servicios
{
	public class GuardadoDeAudio
	{
		private GrpcChannel ServidorDeGuardado;

		public GuardadoDeAudio()
		{
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServidorDeGuardado = GrpcChannel.ForAddress("http://172.17.0.5:80", grpcChannelOptions);

		}

		public bool GuardarAudioDeCancionDeArtista(int idCancion, byte[] audio)
		{
			UVFYGuardadoDeArchivos.Respuesta respuesta = new UVFYGuardadoDeArchivos.Respuesta();
			var cliente = new Guardado.GuardadoClient (ServidorDeGuardado);
			PeticionDeGuardadoDeCancion peticion = new PeticionDeGuardadoDeCancion()
			{
				IdCancion = idCancion, 
				AudioDeCancion = Google.Protobuf.ByteString.CopyFrom(audio)
			};

			try
			{
				respuesta = cliente.GuardarAudioDeCancionDeArtista(peticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Archivos", e);
			}
			catch (Grpc.Core.RpcException e)
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
