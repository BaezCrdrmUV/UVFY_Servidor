﻿using Logica.Clases;
using Logica.ClasesDeComunicacion;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Logica.DAO
{
	public class AlbumDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Metadatos/Albumes/");

		public AlbumDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<Album> CargarPorId(int idAlbum)
		{
			Album albumCargado = new Album();
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idAlbum=" + idAlbum.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorID?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				albumCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Album>>(respuesta.Content.ReadAsStringAsync().Result)[0];
			}
			return albumCargado;
		}

		public async Task<Album> CargarPorIdCancion(int idCancion)
		{
			Album albumCargado = new Album();
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idCancion=" + idCancion.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorCancion?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				albumCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<Album>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return albumCargado;
		}

		public async Task<List<Album>> CargarPorIdArtista(int idArtista)
		{
			List<Album> albumesCargados = new List<Album>();
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idArtista=" + idArtista.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorArtista?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				albumesCargados = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Album>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return albumesCargados;
		}


		public async Task<List<Album>> CargarPorIdGenero(int idGenero)
		{
			List<Album> albumesCargados = new List<Album>();
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idGenero=" + idGenero.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorGenero?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				albumesCargados = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Album>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return albumesCargados;
		}

		public async Task<bool> RegistrarAlbum(string nombre, string descripcion, List<int> generos, byte[] imagen)
		{
			bool resultado = false;
			SolicitudDeRegistrarAlbum peticion = new SolicitudDeRegistrarAlbum()
			{
				token = new Token()
				{
					tokenDeAcceso = TokenDeAcceso
				},
				nombre = nombre,
				generos = generos,
				descripcion = descripcion,
				imagen = imagen
			};

			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("Registrar", peticion);

			if (respuesta.IsSuccessStatusCode)
			{
				resultado = true;
			}

			return resultado;
		} 

		public async Task<bool> AgregarCancionAAlbum(int idAlbum, int idCancion)
		{
			bool resultado = false;
			SolicitudDeAgregarCancionAPlaylist peticion = new SolicitudDeAgregarCancionAPlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				idPlaylist = idAlbum,
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

		public async Task<bool> EliminarCancionDeAlbum(int idAlbum, int idCancion)
		{
			bool resultado = false;
			SolicitudDeEliminarCancionDePlaylist peticion = new SolicitudDeEliminarCancionDePlaylist()
			{
				token = new Token
				{
					tokenDeAcceso = TokenDeAcceso
				},
				idPlaylist = idAlbum,
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

		public async Task<bool> Eliminar(int idAlbum)
		{
			bool resultado = false;
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idAlbum=" + idAlbum.ToString();
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
