using System.IO;

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
					Directory.CreateDirectory(PathCanciones);
					resultado = true;
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

			File.WriteAllBytes(path, datos);
		}

		public static void GuardarCaratulaDeAlbum(string id, byte[] datos)
		{
			string path = PathAlbumes + id;
			if (File.Exists(path))
			{
				File.Delete(path);
			}

			File.WriteAllBytes(path, datos);
		}

		public static byte[] CargarCaratulaDeCancion(string id)
		{
			byte[] caratula;
			caratula = File.ReadAllBytes(PathCanciones + id + "/png");
			return caratula;
		}

		public static byte[] CargarCaratulaDeAlbum(string id)
		{
			byte[] caratula;
			caratula = File.ReadAllBytes(PathAlbumes + id);
			return caratula;
		}

		public static bool VerificarEstructuraDeArchivosAlbum()
		{
			bool resultado = false;
			if (Directory.Exists(PathAlbumes))
			{
				resultado = true;
			}
			else
			{
				Directory.CreateDirectory(PathAlbumes);
				resultado = true;
			}
			return resultado;
		}

		public static byte[] CargarAudioDeCancionPorCalidad(string id, TipoDeArchivo tipoDeArchivo)
		{
			byte[] caratula;
			caratula = File.ReadAllBytes(PathCanciones + id + "/" + tipoDeArchivo.ToString());
			return caratula;
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
