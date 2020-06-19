using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NAudio.Wave;

namespace UVFYGuardadoDeArchivos
{
	public class ServicioDeGuardado : Guardado.GuardadoBase 
	{
		public override Task<Respuesta> GuardarAudioDeCancionDeArtista(PeticionDeGuardadoDeCancion request, ServerCallContext context)
		{
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			List<byte[]> audiosConvertidos;
			try
			{
				 audiosConvertidos = ConvertidorDeArchivos.ConvertirACalidadesEstandar(request.AudioDeCancion.ToArray(), request.IdCancion);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			catch (ResultadoDeServicioFallidoException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			GuardadoDeArchivos guardadoDeArchivos = new GuardadoDeArchivos();
			try
			{


				guardadoDeArchivos.GuardarAudioDeCancionDeArtista(request.IdCancion, audiosConvertidos[0], UVFYArchivos.calidad.Baja);
				guardadoDeArchivos.GuardarAudioDeCancionDeArtista(request.IdCancion, audiosConvertidos[1], UVFYArchivos.calidad.Media);
				guardadoDeArchivos.GuardarAudioDeCancionDeArtista(request.IdCancion, audiosConvertidos[2], UVFYArchivos.calidad.Alta);

			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			catch (ResultadoDeServicioFallidoException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			respuesta.Exitosa = true;

			return Task.FromResult(respuesta);
		}
	}
}
