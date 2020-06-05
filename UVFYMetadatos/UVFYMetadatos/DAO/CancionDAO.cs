using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class CancionDAO
	{

		public List<Canciones> CargarTodas()
		{
			List<Canciones> cancionesCargadas = new List<Canciones>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					cancionesCargadas = context.Canciones.ToList();
				}
				catch(SqlException e)
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
				if(cancionCargada == null)
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
					cancionesCargadas = context.Canciones.Where(c => c.ArtistaId == idArtista).ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return cancionesCargadas;
		}

		public List<Canciones> CargarPodIdAlbum(int idAlbum)
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
					relacionCancionPlaylist = context.CancionPlaylist.Where(p => p.PlaylistsId == idPlaylist).ToList();
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
					relacionCancionGenero = context.CancionGenero.Where(p => p.GenerosId == idGenero).ToList();
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
	}
}
