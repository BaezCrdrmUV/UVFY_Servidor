using Logica.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica.DAO
{
	public class ArtistaDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Metadatos/Artistas/");

		public ArtistaDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<List<Artista>> CargarTodos()
		{
			List<Artista> artistasCargados = new List<Artista>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("Todos?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				artistasCargados = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Artista>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return artistasCargados;
		}

		public async Task<Artista> CargarPorId(int idArtista)
		{
			Artista artistaCargado = new Artista();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idArtista"] = idArtista.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorID?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				artistaCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Artista>>(respuesta.Content.ReadAsStringAsync().Result)[0];
			}
			return artistaCargado;
		}

		public async Task<Artista> CargarPorIdCancion(int idCancion)
		{
			Artista artistaCargado = new Artista();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idCancion"] = idCancion.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorCancion?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				artistaCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<Artista>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return artistaCargado;
		}

		public async Task<Artista> CargarPorIdAlbum(int idAlbum)
		{
			Artista artistaCargado = new Artista();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idAlbum"] = idAlbum.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorAlbum?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				artistaCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<Artista>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return artistaCargado;
		}
	}
}
