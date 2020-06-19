using Google.Protobuf.WellKnownTypes;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace UVFYGuardadoDeArchivos
{
	public static class ConvertidorDeArchivos
	{
		private static string DirectorioTemporal = "/app/Temp";
		private static int rateDeSalida = 48000;
		public static List<byte[]> ConvertirACalidadesEstandar(byte[] audio, int idCancion)
		{
			List<byte[]> resultado = new List<byte[]>();
			byte[] audioEnWav = null;
			if (VerificarExistenciaDeDirectorioTemporal())
			{
				audioEnWav = ConvertirMp3AWav(audio, idCancion);
				resultado.Add(ConvertirWavAMp3(audioEnWav, CalidadDeAudio._128));
				resultado.Add(ConvertirWavAMp3(audioEnWav, CalidadDeAudio._256));
				resultado.Add(ConvertirWavAMp3(audioEnWav, CalidadDeAudio._320));
			}

			return resultado;
		}

		private static byte[] ConvertirWavAMp3(byte[] audio, CalidadDeAudio calidad)
		{
			using var resultado = new MemoryStream();
			using var streamDeAudio = new MemoryStream(audio);
			using var reader = new WaveFileReader(streamDeAudio);
			using var writer = new LameMP3FileWriter(resultado, reader.WaveFormat, int.Parse(calidad.ToString().TrimStart('_')));
			reader.CopyTo(writer);
			return resultado.ToArray();
		}

		private static bool VerificarExistenciaDeDirectorioTemporal()
		{
			bool respuesta = false;

			if (Directory.Exists(DirectorioTemporal))
			{
				respuesta = true;
			}
			else
			{
				Directory.CreateDirectory(DirectorioTemporal);
				respuesta = true;
			}

			return respuesta;
		}


		private static byte[] ConvertirMp3AWav(byte[] audio, int idCancion)
		{
			
			string directorioDeCancionInicial = DirectorioTemporal + "/" + idCancion.ToString();
			string directorioDeCancionConvertida = DirectorioTemporal + "/" + idCancion.ToString() + "wav";
			File.WriteAllBytes(directorioDeCancionInicial, audio);
			using (var reader = new Mp3FileReader(directorioDeCancionInicial))
			{
				var formatoDeSalida = new WaveFormat(rateDeSalida, reader.WaveFormat.Channels);
				using var resampler = new MediaFoundationResampler(reader, formatoDeSalida);
				WaveFileWriter.CreateWaveFile(directorioDeCancionConvertida, resampler);
			}

			byte[] resultado = File.ReadAllBytes(directorioDeCancionConvertida);
			if (File.Exists(directorioDeCancionInicial))
			{
				File.Delete(directorioDeCancionInicial);
			}
			if (File.Exists(directorioDeCancionConvertida))
			{
				File.Delete(directorioDeCancionConvertida);
			}
			return resultado;
		}
	}
}
