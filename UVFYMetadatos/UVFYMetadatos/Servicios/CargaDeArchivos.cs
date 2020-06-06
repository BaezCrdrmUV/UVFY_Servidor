using Grpc.Core;
using Grpc.Net.Client;
using LogicaDeNegocios.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYArchivos;
using UVFYMetadatos.Exceptiones;

namespace UVFYMetadatos.Servicios
{
	public class CargaDeArchivos
	{
		private GrpcChannel ServicioDeArchivos;

		public CargaDeArchivos()
		{
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.7:80", grpcChannelOptions);
		}

		public byte[] CargarCaratulaDeCancionPorId(int id)
		{
			byte[] caratula = null;
			RespuestaDeCaratula respuesta = new RespuestaDeCaratula();
			var cliente = new Archivos.ArchivosClient(ServicioDeArchivos);
			UVFYArchivos.PeticionId peticion = new UVFYArchivos.PeticionId
			{
				IdPeticion = id
			};
			try
			{
				respuesta = cliente.CargarCaratulaDeCancionPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException e)
			{
				throw new AccesoAServicioException("Archivos", e);
			}
			catch (Grpc.Core.RpcException e)
			{
				throw new AccesoAServicioException("Sesiones", e);
			}

			if (respuesta.Respuesta.Exitosa)
			{
				caratula = respuesta.Caratula.ToArray();
			}
			else
			{
				throw new ResultadoDeServicioFallidoException("Sesiones");
			}

			return caratula;
		}
	}
}
