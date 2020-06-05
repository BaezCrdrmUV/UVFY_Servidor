using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class GeneroDAO
	{
		public List<Generos> CargarTodos()
		{
			List<Generos> generosCargados = new List<Generos>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					generosCargados = context.Generos.ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return generosCargados;
		}

		public Generos CargarPorId(int idGenero)
		{
			Generos generoCargado;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					generoCargado = context.Generos.Find(idGenero);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if(generoCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return generoCargado;
		}

		public List<Generos> CargarPorIdAlbum(int idAlbum)
		{
			List<AlbumGenero> relacionAlbumGenero = new List<AlbumGenero>();
			List<Generos> generosCargados = new List<Generos>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					relacionAlbumGenero = context.AlbumGenero.Where(p => p.AlbumesId == idAlbum).ToList();
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
						Generos cancion = CargarPorId(albumGenero.GenerosId);
						generosCargados.Add(cancion);
					}
					catch (SqlException e)
					{
						Console.WriteLine(e.ToString());
						throw new AccesoADatosException(e.Message, e);
					}
				}
			}
			return generosCargados;
		}
	}
}
