using Logica.Clases;
using Logica.ClasesDeComunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Logica.DAO
{
	public class PlaylistDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Metadatos/Playlists/");

		public PlaylistDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<Playlist> CargarPorId(int idPlaylist)
		{
			Playlist playlistCargada = new Playlist();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idPlaylist"] = idPlaylist.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorID?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				playlistCargada = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<Playlist>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return playlistCargada;
		}

		public async Task<List<Playlist>> CargarPorIdConsumidor(int idConsumidor)
		{
			List<Playlist> playlistsCargadas = new List<Playlist>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idConsumidor"] = idConsumidor.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorConsumidor?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				playlistsCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Playlist>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return playlistsCargadas;
		}

		public async Task<bool> CrearPlaylist(string nombre)
		{
			bool resultado = false;
			SolicitudDeAgregarPlaylist peticion = new SolicitudDeAgregarPlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				nombre = nombre
			};

			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("Crear", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<bool> AgregarCancionAPlaylist(int idPlaylist, int idCancion)
		{
			bool resultado = false;
			SolicitudDeAgregarCancionAPlaylist peticion = new SolicitudDeAgregarCancionAPlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				idPlaylist = idPlaylist,
				idCancion = idCancion
			};

			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("AgregarCancion", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<bool> EliminarCancionDePlaylist(int idPlaylist, int idCancion)
		{
			bool resultado = false;
			SolicitudDeEliminarCancionDePlaylist peticion = new SolicitudDeEliminarCancionDePlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				idPlaylist = idPlaylist,
				idCancion = idCancion
			};

			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("EliminarCancion", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<bool> RenombrarPlaylist(int idPlaylist, string nuevoNombre)
		{
			bool resultado = false;
			SolicitudDeRenombrarPlaylist peticion = new SolicitudDeRenombrarPlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				idPlaylist = idPlaylist,
				nombre = nuevoNombre
			};

			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("Renombrar", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<bool> EliminarPlaylist(int idPlaylist)
		{
			bool resultado = false;
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idPlaylist"] = idPlaylist.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Delete("Eliminar?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}
	}
}
