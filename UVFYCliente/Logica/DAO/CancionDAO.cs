using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Logica.Clases;

namespace Logica.DAO
{
	public class CancionDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Metadatos/Canciones/");

		public CancionDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<List<Cancion>> CargarTodas()
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("Todas?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public Cancion CargarPorId(int idCancion)
		{
			Cancion cancionCargada = new Cancion();
			
			return cancionCargada;
		}

		public List<Cancion> CargarPorIdArtista(int idArtista)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();

			return cancionesCargadas;
		}

		public List<Cancion> CargarPodIdAlbum(int idAlbum)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();

			return cancionesCargadas;
		}

		public List<Cancion> CargarPorIdPlaylist(int idPlaylist)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();

			return cancionesCargadas;
		}

		public List<Cancion> CargarPorIdGenero(int idGenero)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			
			return cancionesCargadas;
		}
	}
}
