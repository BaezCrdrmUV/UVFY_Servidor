using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace UVFYArchivos.ServiciosInternos
{
	public static class ServiciosDeIO
	{
		private static string PathCanciones = "/app/Archivos/Canciones/";
		private static string PathAlbumes = "/app/Archivos/Albums/";
		public static bool VerificarEstructuraDeArchivosCancion(string id)
		{
			bool resultado = false;
			if (Directory.Exists(PathCanciones + id))
			{
				resultado = true;
			}
			else
			{
				if (Directory.Exists(PathCanciones))
				{
					Directory.CreateDirectory(PathCanciones + id);
					resultado = true;
				}
				else
				{
					resultado = false;
				}
			}
			return resultado;
		}

		public static void GuardarArchivoDeCancion(string id, byte[] datos, TipoDeArchivo tipoDeArchivo)
		{
			string path = PathCanciones + id + "/" + tipoDeArchivo.ToString();
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			File.Create(path);
			File.WriteAllBytes(path, datos);
		}

		public static void GuardarCaratulaDeAlbum(string id, byte[] datos)
		{
			string path = PathAlbumes + id;
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			File.Create(path);
			File.WriteAllBytes(path, datos);
		}

		public static byte[] CargarCaratulaDeCancion(string id)
		{
			byte[] caratula = null;
			caratula = File.ReadAllBytes(PathCanciones + id + "/png");
			return caratula;
		}

		public static bool VerificarEstructuraDeArchivosAlbum(string id)
		{
			bool resultado = false;
			throw new NotImplementedException();
			return resultado;
		}
	}

	public enum TipoDeArchivo
	{
		mp3_128 = 1,
		mp3_256 = 2,
		mp3_320 = 3,
		png = 0

	}
}
