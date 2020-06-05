using LogicaDeNegocios.Excepciones;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class ArtistaDAO
	{
		public List<UsuariosArtista> CargarTodos()
		{
			List<UsuariosArtista> artistasCargados = new List<UsuariosArtista>();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					artistasCargados = context.UsuariosArtista.ToList();
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
			}
			return artistasCargados;
		}

		public UsuariosArtista CargarPorId(int idArtista)
		{
			UsuariosArtista artistaCargado = new UsuariosArtista();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					artistaCargado = context.UsuariosArtista.Find(idArtista);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (artistaCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return artistaCargado;
		}

		public UsuariosArtista CargarPorIdCancion(int idCancion)
		{
			UsuariosArtista artistaCargado = new UsuariosArtista();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					artistaCargado = context.UsuariosArtista.FirstOrDefault(a => a.Canciones.Any(b => b.Id == idCancion));
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (artistaCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return artistaCargado;
		}

		public UsuariosArtista CargarPorIdAlbum(int idAlbum)
		{
			UsuariosArtista artistaCargado = new UsuariosArtista();
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					artistaCargado = context.UsuariosArtista.FirstOrDefault(a => a.Albumes.Any(b => b.Id == idAlbum));
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if(artistaCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return artistaCargado;
		}
	}
}
