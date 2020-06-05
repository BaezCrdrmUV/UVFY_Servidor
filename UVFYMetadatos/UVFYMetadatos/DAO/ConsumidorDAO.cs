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
	public class ConsumidorDAO
	{
		public UsuariosConsumidor CargarPorId(int idConsumidor)
		{
			UsuariosConsumidor consumidorCargado;
			using (UVFYContext context = new UVFYContext())
			{
				try
				{
					consumidorCargado = context.UsuariosConsumidor.Find(idConsumidor);
				}
				catch (SqlException e)
				{
					Console.WriteLine(e.ToString());
					throw new AccesoADatosException(e.Message, e);
				}
				if (consumidorCargado == null)
				{
					throw new RecursoNoExisteException();
				}
			}
			return consumidorCargado;
		}
	}
}
