using Grpc.Core;
using Grpc.Net.Client;
using LogicaDeNegocios.Excepciones;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYArchivos;
using UVFYMetadatos.Exceptiones;

namespace UVFYMetadatos.Servicios
{
	public class GuardadoDeImagenes
	{
		private GrpcChannel ServicioDeArchivos;

		public GuardadoDeImagenes()
		{
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeArchivos = GrpcChannel.ForAddress("http://172.17.0.6:80", grpcChannelOptions);
		}


		public bool GuardarAudioDeCancionDeConsumidor(int idCancion, byte[] audio)
		{
			UVFYArchivos.Respuesta respuesta = new UVFYArchivos.Respuesta();
			var cliente = new Archivos.ArchivosClient(ServicioDeArchivos);
			PeticionGuardadoIdYCalidad peticion = new PeticionGuardadoIdYCalidad();
			peticion.Calidad = calidad.Alta;
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

		public bool GuardarCaratulaDeCancion(int idCancion, byte[] caratula)
		{
			UVFYArchivos.Respuesta respuesta = new UVFYArchivos.Respuesta();
			var cliente = new Archivos.ArchivosClient(ServicioDeArchivos);
			PeticionGuardadoId peticion = new PeticionGuardadoId();
			peticion.Datos = Google.Protobuf.ByteString.CopyFrom(caratula);
			peticion.IdPeticion = idCancion;
			
			try
			{
				respuesta = cliente.GuardarCaratulaDeCancionPorId(peticion);
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

		public bool GuardarCaratulaDeAlbum(int idAlbum, byte[] caratula)
		{
			UVFYArchivos.Respuesta respuesta = new UVFYArchivos.Respuesta();
			var cliente = new Archivos.ArchivosClient(ServicioDeArchivos);
			PeticionGuardadoId peticion = new PeticionGuardadoId();
			peticion.Datos = Google.Protobuf.ByteString.CopyFrom(caratula);
			peticion.IdPeticion = idAlbum;

			try
			{
				respuesta = cliente.GuardarCaratulaDeAlbumPorId(peticion);
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
