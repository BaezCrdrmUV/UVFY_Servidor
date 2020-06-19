using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;
using UVFYMetadatos.Servicios;

namespace UVFYMetadatos.DAO
{
	public class PlaylistDAO
	{
		public Playlists CargarPorId(int idPlaylist)
		{
			Playlists playlistCargada;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					playlistCargada = context.Playlists.Find(idPlaylist);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (playlistCargada == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return playlistCargada;
		}

		public List<Playlists> CargarPorIdUsuario(int idUsuario)
		{
			List<Playlists> playlistsCargadas = new List<Playlists>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					playlistsCargadas = context.Playlists.Where(c => c.ConsumidorId == idUsuario).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (playlistsCargadas == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return playlistsCargadas;
		}

		public bool Registrar(string nombreDePlaylist, int idUsuario)
		{
			bool respuesta = false;
			Playlists playlistConvertida = new Playlists()
			{
				Nombre = nombreDePlaylist,
			};
			if (ValidarParaGuardar(playlistConvertida))
			{
				UsuarioDAO consumidorDAO = new UsuarioDAO();
				playlistConvertida.Consumidor = consumidorDAO.CargarPorId(idUsuario);
				try
				{
					using (UVFYContext context = new UVFYContext())
					{
						context.Playlists.Add(playlistConvertida);
						context.SaveChanges();
						respuesta = true;
					}
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return respuesta;
		}

		private bool ValidarParaGuardar(Playlists playlist)
		{
			bool resultado = false;

			if (ServiciosDeValidacion.ValidarCadena(playlist.Nombre))
			{
				resultado = true;
			}

			return resultado;
		}

		public bool AgregarCancion(int idPlaylist, int idCancion)
		{
			bool respuesta = false;
			try
			{
				Playlists playlist = CargarPorId(idPlaylist);
				CancionDAO cancionDAO = new CancionDAO();
				Canciones cancion = cancionDAO.CargarPorId(idCancion);
				CancionPlaylist cancionPlaylist = new CancionPlaylist
				{
					Cancion = cancion,
					Playlists = playlist
				};
				using (UVFYContext context = new UVFYContext())
				{
					context.CancionPlaylist.Add(cancionPlaylist);
				}
				respuesta = true;
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}
			return respuesta;
		}

		public bool EliminarCancion(int idPlaylist, int idCancion)
		{
			bool respuesta = false;
			CancionPlaylist cancionPlaylist;
			try
			{
				using (UVFYContext context = new UVFYContext())
				{
					cancionPlaylist = context.CancionPlaylist.FirstOrDefault(cp => cp.CancionId == idCancion && cp.PlaylistsId == idPlaylist);
					if (cancionPlaylist != null)
					{ 
						context.CancionPlaylist.Remove(cancionPlaylist);
					}
					else
					{
						throw new RecursoNoExisteException();
					}
				}
				respuesta = true;
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}
			return respuesta;
		}

		public bool Renombrar(int idPlaylist, string nuevoNombre)
		{
			bool respuesta = false;
			try
			{
				Playlists playlistCargada = CargarPorId(idPlaylist);
				using (UVFYContext context = new UVFYContext())
				{
					playlistCargada = context.Playlists.Find(idPlaylist);
					if (playlistCargada == null)
					{
						throw new RecursoNoExisteException();
					}
					playlistCargada.Nombre = nuevoNombre;
					context.SaveChanges();
				}
				respuesta = true;
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}

			return respuesta;
		}

		public bool Eliminar(int idPlaylist)
		{
			bool respuesta;
			try
			{
				Playlists playlistCargada = CargarPorId(idPlaylist);
				using (UVFYContext context = new UVFYContext())
				{
					playlistCargada = context.Playlists.Find(idPlaylist);
					if (playlistCargada == null)
					{
						throw new RecursoNoExisteException();
					}
					context.Playlists.Remove(playlistCargada);
					context.SaveChanges();
				}
				respuesta = true;
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}
			return respuesta;
		}
	}
}
