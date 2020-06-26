using Logica.Clases;
using Logica.ClasesDeComunicacion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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

		public async Task<Cancion> CargarPorId(int idCancion)
		{
			Cancion cancionCargada = new Cancion();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idCancion"] = idCancion.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorID?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionCargada = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result)[0];
			}
			return cancionCargada;
		}

		public async Task<List<Cancion>> CargarPorIdArtista(int idArtista)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idArtista"] = idArtista.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorArtista?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<List<Cancion>> CargarPorIdAlbum(int idAlbum)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idAlbum"] = idAlbum.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorAlbum?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<List<Cancion>> CargarPorIdPlaylist(int idPlaylist)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idPlaylist"] = idPlaylist.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorPlaylist?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<List<Cancion>> CargarPrivadasPorIdArtista(int idArtista)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idArtista"] = idArtista.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PrivadasPorArtista?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<List<Cancion>> CargarPorIdGenero(int idGenero)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idGenero"] = idGenero.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorGenero?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<bool> RegistrarCancionDeArtista(string nombre, List<int> generos, byte[] audio, byte[] imagen, int duracion)
		{
			bool resultado = false;
			SolicitudDeRegistrarCancion peticion = new SolicitudDeRegistrarCancion()
			{
				token = new Token()
				{
					tokenDeAcceso = TokenDeAcceso
				},
				nombre = nombre,
				generos = generos,
				audio = audio,
				imagen = imagen,
				duracion = duracion
			};
			ByteArrayContent peticionSerializada = Servicios.ServicioDeConversionDeJson.SerializarPeticion(peticion);
			HttpResponseMessage respuesta;
		
			respuesta = await AdministradorDePeticionesHttp.Post("RegistrarDeArtista", peticion);
			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<bool> RegistrarCancionDeConsumidor(string nombre, byte[] audio, byte[] imagen, int duracion)
		{
			bool resultado = false;
			SolicitudDeRegistrarCancion peticion = new SolicitudDeRegistrarCancion()
			{
				token = new Token()
				{
					tokenDeAcceso = TokenDeAcceso
				},
				nombre = nombre,
				generos = new List<int>(),
				audio = audio,
				imagen = imagen,
				duracion = duracion
			};
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("RegistrarDeConsumidor", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		}

		public async Task<List<Cancion>> CargarPrivadasPorIdConsumidor(int idConsumidor)
		{
			List<Cancion> cancionesCargadas = new List<Cancion>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idConsumidor"] = idConsumidor.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PrivadasPorConsumidor?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				cancionesCargadas = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Cancion>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return cancionesCargadas;
		}

		public async Task<bool> Eliminar(int idCancion)
		{
			bool resultado = false;
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idCancion"] = idCancion.ToString();
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
