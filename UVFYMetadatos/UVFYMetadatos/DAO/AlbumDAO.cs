using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
	}
}
