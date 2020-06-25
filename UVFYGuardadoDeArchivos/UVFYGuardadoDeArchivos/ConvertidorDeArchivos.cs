using Google.Protobuf.WellKnownTypes;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using NLayer;
using NLayer.NAudioSupport;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UVFYGuardadoDeArchivos
{
	public static class ConvertidorDeArchivos
	{
		private static string DirectorioTemporal = "/app/Temp/";
		public static List<byte[]> ConvertirACalidadesEstandar(byte[] audio, int idCancion)
		{
			List<byte[]> resultado = new List<byte[]>();
			if (VerificarExistenciaDeDirectorioTemporal())
			{
				File.WriteAllBytes(DirectorioTemporal + "recibido", audio);
				resultado.Add(ConvertirAFormatos(audio, CalidadDeAudio._128));
				resultado.Add(ConvertirAFormatos(audio, CalidadDeAudio._256));
				resultado.Add(ConvertirAFormatos(audio, CalidadDeAudio._320));
			}

			Limpiar();

			return resultado;
		}

		private static void Limpiar()
		{
			if (File.Exists("/app/Temp/recibido"))
			{
				File.Delete("/app/Temp/recibido");
			}
			if (File.Exists("/app/Temp/128.mp3"))
			{
				File.Delete("/app/Temp/128.mp3");
			}
			if (File.Exists("/app/Temp/256.mp3"))
			{
				File.Delete("/app/Temp/256.mp3");
			}
			if (File.Exists("/app/Temp/320.mp3"))
			{
				File.Delete("/app/Temp/320.mp3");
			}
		}

		private static byte[] ConvertirAFormatos(byte[] audio, CalidadDeAudio calidad)
		{

			ProcessStartInfo psi = new ProcessStartInfo("/usr/bin/ffmpeg")
			{
				UseShellExecute = false
			};
			switch (calidad)
			{
				case CalidadDeAudio._128:
					psi.Arguments = "-i /app/Temp/recibido -codec:a libmp3lame -b:a 128k /app/Temp/128.mp3";
				break;
				case CalidadDeAudio._256:
					psi.Arguments = "-i /app/Temp/recibido -codec:a libmp3lame -b:a 256k /app/Temp/256.mp3";
				break;
				case CalidadDeAudio._320:
					psi.Arguments = "-i /app/Temp/recibido -codec:a libmp3lame -b:a 320k /app/Temp/320.mp3";
				break;
			}
			Process p = Process.Start(psi);
			p.WaitForExit();
			int ec = p.ExitCode;
			byte[] resultado = null;
			switch (calidad)
			{
				case CalidadDeAudio._128:
					if (File.Exists("/app/Temp/128.mp3"))
					{
						resultado = File.ReadAllBytes("/app/Temp/128.mp3");
					}
					break;
				case CalidadDeAudio._256:
					if (File.Exists("/app/Temp/256.mp3"))
					{
						resultado = File.ReadAllBytes("/app/Temp/256.mp3");
					}
					break;
				case CalidadDeAudio._320:
					if (File.Exists("/app/Temp/320.mp3"))
					{
						resultado = File.ReadAllBytes("/app/Temp/320.mp3");
					}
					break;
			}

			return resultado;
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

	}
}
