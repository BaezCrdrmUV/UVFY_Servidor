using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYMetadatos.Models;

namespace UVFYMetadatos.DAO
{
	public class CancionDAO
	{

		public List<Canciones> CargarTodas()
		{
			List<Canciones> cancionesCargadas;
			using (UVFYContext context = new UVFYContext())
			{
				//Catch entity exception
				cancionesCargadas = context.Canciones.ToList();
			}
			return cancionesCargadas;
		}
	}
}
