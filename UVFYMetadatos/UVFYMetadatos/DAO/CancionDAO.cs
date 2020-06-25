using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UVFYMetadatos.Enumeradores;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class CancionDAO
	{
		public List<Canciones> CargarPublicas()
		{
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionesCargadas = context.Canciones.Where(p => p.Estado == (short)EstadoDeCancion.Publica).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return cancionesCargadas;
		}

		public Canciones CargarPorId(int idCancion)
		{
			Canciones cancionCargada;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionCargada = context.Canciones.Find(idCancion);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (cancionCargada == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return cancionCargada;
		}

		public List<Canciones> CargarPorIdArtista(int idArtista)
		{
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionesCargadas = context.Canciones.Where(c => c.ArtistaId == idArtista && c.Estado == (short)EstadoDeCancion.Publica).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return cancionesCargadas;
		}

		public List<Canciones> CargarPorIdAlbum(int idAlbum)
		{
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionesCargadas = context.Canciones.Where(c => c.AlbumsId == idAlbum).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return cancionesCargadas;
		}

		public List<Canciones> CargarPorIdPlaylist(int idPlaylist)
		{
			List<CancionPlaylist> relacionCancionPlaylist = new List<CancionPlaylist>();
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					relacionCancionPlaylist = context.CancionPlaylist.Where(p => p.PlaylistsId == idPlaylist && p.Cancion.Estado != (short)EstadoDeCancion.PrivadaDeArtista).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}

				foreach (CancionPlaylist cancionPlaylist in relacionCancionPlaylist)
				{
					try
					{
						Canciones cancion = CargarPorId(cancionPlaylist.CancionId);
						cancionesCargadas.Add(cancion);
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
				}
			}
			return cancionesCargadas;
		}

		public List<Canciones> CargarPorIdGenero(int idGenero)
		{
			List<CancionGenero> relacionCancionGenero = new List<CancionGenero>();
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					relacionCancionGenero = context.CancionGenero.Where(p => p.GenerosId == idGenero && p.Canciones.Estado == (short)EstadoDeCancion.Publica).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}

				foreach (CancionGenero cancionGenero in relacionCancionGenero)
				{
					try
					{
						Canciones cancion = CargarPorId(cancionGenero.CancionesId);
						cancionesCargadas.Add(cancion);
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
				}
			}
			return cancionesCargadas;
		}

		public List<Canciones> CargarCancionesPrivadasPorIdArtista(int idArtista)
		{
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionesCargadas = context.Canciones.Where(c => c.ArtistaId == idArtista && c.Estado == (short)EstadoDeCancion.PrivadaDeArtista).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return cancionesCargadas;
		}

		public int RegistrarCancionDeArtista(Canciones cancionARegistrar, List<int> generos)
		{
			ArtistaDAO artistaDAO = new ArtistaDAO();
			cancionARegistrar.Artista = artistaDAO.CargarPorId(cancionARegistrar.ArtistaId.GetValueOrDefault());
			cancionARegistrar.FechaDeLanzamiento = DateTime.Now;
			cancionARegistrar.Estado = (int)EstadoDeCancion.PrivadaDeArtista;
			foreach (int idGenero in generos)
			{
				CancionGenero cancionGenero = new CancionGenero();
				GeneroDAO generoDAO = new GeneroDAO();
				cancionGenero.Generos = generoDAO.CargarPorId(idGenero);
				cancionARegistrar.CancionGenero.Add(cancionGenero);
			}

			try
			{
				using (UVFYContext context = new UVFYContext())
				{
					context.Attach(cancionARegistrar.Artista);
					foreach(CancionGenero cancionGenero in cancionARegistrar.CancionGenero)
					{
						context.Attach(cancionGenero.Generos);
					}
					context.Canciones.Add(cancionARegistrar);
					context.SaveChanges();
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}

			return cancionARegistrar.Id;
		}

		public int RegistrarCancionDeConsumidor(Canciones cancionARegistrar)
		{
			cancionARegistrar.Estado = (int)EstadoDeCancion.PrivadaDeConsumidor;

			try
			{
				using (UVFYContext context = new UVFYContext())
				{
					context.Canciones.Add(cancionARegistrar);
					context.SaveChanges();
				}
			}
			catch (SqlException e)
			{
				Console.WriteLine(e.ToString());
				throw new AccesoADatosException(e.Message, e);
			}

			return cancionARegistrar.Id;
		}

		public bool CambiarEstadoDeCancion(int idCancion, EstadoDeCancion estadoNuevo)
		{
			bool resultado = false;
			Canciones cancionAEditar;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionAEditar = context.Canciones.Find(idCancion);
					cancionAEditar.Estado = (short)estadoNuevo;
					context.SaveChanges();
					resultado = true;
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (cancionAEditar == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return resultado;
		}
	}
}
