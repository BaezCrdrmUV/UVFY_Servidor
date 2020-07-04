using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Servicios
{
	public class ServiciosDeIO
	{
		private static string idUsuarioActual = "0";
		private static string DireccionUVFY = "\\UVFY\\";
		public static bool ExisteDirectorioDeAplicacion()
		{
			bool resultado;
			string pathLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY;
			if (Directory.Exists(pathLocal))
			{
				resultado = true;
			}
			else
			{
				Directory.CreateDirectory(pathLocal);
				resultado = true;
			}

			return resultado;
		}

		public static void AsignarIdUsuarioActual(string id)
		{
			idUsuarioActual = id;
		}

		public static bool ExisteDirectorioDeUsuario()
		{
			bool resultado;
			string pathLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + idUsuarioActual;
			if (Directory.Exists(pathLocal))
			{
				resultado = true;
			}
			else
			{
				Directory.CreateDirectory(pathLocal);
				resultado = true;
			}

			return resultado;
		}

		public static bool CancionEstaGuardada(int idCancion)
		{
			bool resultado = false;
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				if (File.Exists(ConstruirDireccionDeCancion(idCancion)))
				{
					resultado = true;
				}
			}
			return resultado;
		}

		internal static void EliminarArchivosTemporales()
		{
			string pathTemporal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + "temp";
			List<string> archivosEncontrados = new List<string>();

			if (Directory.Exists(pathTemporal))
			{
				archivosEncontrados = Directory.GetFiles(pathTemporal).ToList();
				foreach(string archivo in archivosEncontrados)
				{
					File.Delete(archivo);
				}
			}
		}

		public static void EliminarCancion(int idCancion)
		{
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				string direccionDeCancion = ConstruirDireccionDeCancion(idCancion);
				if (File.Exists(direccionDeCancion))
				{
					File.Delete(direccionDeCancion);
				}
			}
		}

		public static void EliminarCaratula(int idCancion)
		{
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				string direccionDeCaratula = ConstruirDireccionDeCaratula(idCancion);
				if (File.Exists(direccionDeCaratula))
				{
					File.Delete(direccionDeCaratula);
				}
			}
		}
		
		public static List<int> ListarCancionesDescargadas()
		{
			string pathLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + idUsuarioActual;

			List<int> cancionesEncontradas = new List<int>();
			List<string> archivosEncontrados = new List<string>();
		
			if (Directory.Exists(pathLocal))
			{
				archivosEncontrados = Directory.GetFiles(pathLocal).ToList();
				List<string> listaTemporal = new List<string>();
				foreach (string archivo in archivosEncontrados)
				{
					listaTemporal.Add(Path.GetFileName(archivo));
				}
				archivosEncontrados = listaTemporal;
				archivosEncontrados = archivosEncontrados.Where(a => ServiciosDeValidacion.ValidarNumeroEntero(a)).ToList();
			}

			foreach (string archivo in archivosEncontrados)
			{
				int resultado = 0;
				if (int.TryParse(archivo, out resultado))
				{
					cancionesEncontradas.Add(resultado);
				}
			}

			return cancionesEncontradas;
		}

		internal static void GuardarCaratula(byte[] imagen, int idCancion)
		{
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				if (!CancionEstaGuardada(idCancion))
				{
					File.WriteAllBytes(ConstruirDireccionDeCaratula(idCancion), imagen);
				}
			}
		}

		public static bool CaratulaEstaGuardada(int idCancion)
		{
			bool resultado = false;
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				if (File.Exists(ConstruirDireccionDeCaratula(idCancion)))
				{
					resultado = true;
				}
			}
			return resultado;
		}

		public static string ConstruirDireccionDeCancion(int idCancion)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + idUsuarioActual + "\\" + idCancion;
			return path;
		}

		public static string ConstruirDireccionDeCaratula(int idCancion)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + idUsuarioActual + "\\" + idCancion + "png";
			return path;
		}

		public static void GuardarCancion(byte[] audio, int idCancion)
		{
			if (ExisteDirectorioDeAplicacion() && ExisteDirectorioDeUsuario())
			{
				if (!CancionEstaGuardada(idCancion))
				{
					File.WriteAllBytes(ConstruirDireccionDeCancion(idCancion), audio);
				}
			}
		}

		public static void GuardarCancionTemporal(byte[] audio, int idCancion)
		{
			if (ExisteDirectorioTemporal())
			{
				File.WriteAllBytes(ConstruirDireccionTemporalDeCancion(idCancion), audio);
			}
		}

		public static string ConstruirDireccionTemporalDeCancion(int idCancion)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + "temp\\" + idCancion;
			return path;
		}

		private static bool ExisteDirectorioTemporal()
		{
			bool resultado;
			string pathTemporal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + "temp";
			if (Directory.Exists(pathTemporal))
			{
				resultado = true;
			}
			else
			{
				Directory.CreateDirectory(pathTemporal);
				resultado = true;
			}

			return resultado;
		}

		public async static Task<byte[]> CargarCaratulaDeCancionPorId(int idCancion, string token)
		{
			byte[] imagen;
			if (CaratulaEstaGuardada(idCancion))
			{
				imagen = File.ReadAllBytes(ConstruirDireccionDeCaratula(idCancion));
			}
			else
			{
				ArchivosDAO archivosDAO = new ArchivosDAO(token);
				imagen = await archivosDAO.CargarCaratulaDeCancionPorId(idCancion);
			}

			return imagen;
		}

		public static bool ExisteArchivo(string direccionDeArchivo)
		{
			bool resultado = false;

			if (File.Exists(direccionDeArchivo))
			{
				resultado = true;
			}

			return resultado;
		}

		public static byte[] CargarBytesDeArchivo(string direccionDeArchivo)
		{
			byte[] resultado = File.ReadAllBytes(direccionDeArchivo);
			return resultado; 
		}

		public static int ObtenerDuracionDeCancion(string direccionDeArchivo)
		{
			int resultado = 0;
			//Mp3FileReader mp3FileReader = new Mp3FileReader(direccionDeArchivo);
			//resultado = (int)mp3FileReader.TotalTime.TotalSeconds;

			return resultado;
		}
	}
}
