using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYArchivos.Excepciones;
using UVFYArchivos.Models;
using UVFYArchivos.Services;

namespace UVFYArchivos.DAO
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
			}
			return albumCargado;
		}
	}
}
