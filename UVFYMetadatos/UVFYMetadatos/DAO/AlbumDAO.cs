using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class AlbumDAO
	{
		public Albumes CargarPorId(int idAlbum)
		{
			Albumes albumCargado;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					albumCargado = context.Albumes.Find(idAlbum);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (albumCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return albumCargado;
		}

		public Albumes CargarPorIdCancion(int idCancion)
		{
			Albumes albumCargado = new Albumes();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					albumCargado = context.Albumes.FirstOrDefault(a => a.Canciones.Any(b => b.Id == idCancion));
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (albumCargado == null)
				{
					throw new RecursoNoExisteException();
				}

			}
			return albumCargado;
		}

		public List<Albumes> CargarPorIdArtista(int idArtista)
		{
			List<Albumes> albumesCargados = new List<Albumes>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					albumesCargados = context.Albumes.Where(a => a.ArtistasId == idArtista).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return albumesCargados;
		}

		public List<Albumes> CargarPorIdGenero(int idGenero)
		{
			List<AlbumGenero> relacionAlbumGenero = new List<AlbumGenero>();
			List<Albumes> albumesCargados = new List<Albumes>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					relacionAlbumGenero = context.AlbumGenero.Where(a => a.GenerosId == idGenero).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}

				foreach (AlbumGenero albumGenero in relacionAlbumGenero)
				{
					try
					{
						Albumes album = CargarPorId(albumGenero.AlbumesId);
						albumesCargados.Add(album);
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
				}

			}
			return albumesCargados;
		}

		public int RegistrarAlbum(Albumes albumARegistrar, List<int> generos)
		{
			int idDeAlbumGuardado = 0;
			if (ValidarParaGuardado(albumARegistrar))
			{
				using (UVFYContext context = new UVFYContext())
				{
					try
					{
						context.Albumes.Add(albumARegistrar);
						context.SaveChanges();
						idDeAlbumGuardado = albumARegistrar.Id;
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
					try
					{
						foreach (int idGenero in generos)
						{
							AlbumGenero albumGenero = new AlbumGenero()
							{
								AlbumesId = albumARegistrar.Id,
								GenerosId = idGenero
							};
							albumARegistrar.AlbumGenero.Add(albumGenero);
						}
						context.SaveChanges();
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
				}
			}
			else
			{
				throw new ValidacionFallidaException("Album invalido");
			}

			return idDeAlbumGuardado;
		}

		public bool AgregarCancionAAlbum(int idCancion, int idAlbum)
		{
			bool respuesta = false;
			try
			{
				Albumes album = CargarPorId(idAlbum);
				CancionDAO cancionDAO = new CancionDAO();
				Canciones cancion = cancionDAO.CargarPorId(idCancion);
				using (UVFYContext context = new UVFYContext())
				{
					album.Canciones.Add(cancion);
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

		public bool EliminarCancionDeAlbum(int idCancion, int idAlbum)
		{
			bool respuesta = false;
			try
			{
				Albumes album = CargarPorId(idAlbum);
				CancionDAO cancionDAO = new CancionDAO();
				Canciones cancion = cancionDAO.CargarPorId(idCancion);
				using (UVFYContext context = new UVFYContext())
				{
					album.Canciones.Remove(cancion);
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

		public bool Eliminar(int idAlbum)
		{
			bool respuesta = false;
			Albumes album = CargarPorId(idAlbum);
			CancionDAO cancionDAO = new CancionDAO();
			try
			{
				List<Canciones> cancionesEnAlbum = cancionDAO.CargarPorIdAlbum(album.Id);
				foreach(Canciones cancion in cancionesEnAlbum)
				{
					cancionDAO.CambiarEstadoDeCancion(cancion.Id, Enumeradores.EstadoDeCancion.PrivadaDeArtista);
				}

				using (UVFYContext context = new UVFYContext())
				{
					context.Albumes.Remove(album);
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

		public bool ValidarParaGuardado(Albumes albumAValidar)
		{
			bool resultado = false;

			if (Servicios.ServiciosDeValidacion.ValidarCadena(albumAValidar.Nombre))
			{
				if (Servicios.ServiciosDeValidacion.ValidarCadena(albumAValidar.Descripcion))
				{
					resultado = true;
				}
			}

			return resultado;
		}
	}
}
