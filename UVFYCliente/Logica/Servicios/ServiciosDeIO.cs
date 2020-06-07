using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Logica.Servicios
{
	public class ServiciosDeIO
	{
		private static string DireccionUVFY = "\\UVFY";
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

		public static bool CancionEstaGuardada(int idCancion)
		{
			bool resultado = false;
			if (ExisteDirectorioDeAplicacion())
			{
				if (File.Exists(ConstruirDireccionDeCancion(idCancion)))
				{
					resultado = true;
				}
			}
			return resultado;
		}

		internal static void GuardarCaratula(byte[] imagen, int idCancion)
		{
			if (ExisteDirectorioDeAplicacion())
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
			if (ExisteDirectorioDeAplicacion())
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
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + "\\" + idCancion;
			return path;
		}

		public static string ConstruirDireccionDeCaratula(int idCancion)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + DireccionUVFY + "\\" + idCancion + "png";
			return path;
		}

		public static void GuardarCancion(byte[] audio, int idCancion)
		{
			if (ExisteDirectorioDeAplicacion())
			{
				if (!CancionEstaGuardada(idCancion))
				{
					File.WriteAllBytes(ConstruirDireccionDeCancion(idCancion), audio);
				}
			}
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
	}
}
