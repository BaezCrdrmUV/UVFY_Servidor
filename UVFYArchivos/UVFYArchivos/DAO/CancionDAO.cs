using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYArchivos.Excepciones;
using UVFYArchivos.Models;

namespace UVFYArchivos.DAO
{
	public class CancionDAO
	{
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
			}
			return cancionCargada;
		}
	}
}
