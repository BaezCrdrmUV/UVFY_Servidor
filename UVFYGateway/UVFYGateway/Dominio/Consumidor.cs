using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UVFYGateway.Dominio
{
	public class Consumidor : Usuario
	{
		public bool EstadoDeSuscripcion { get; set; }
		public DateTime FechaDeFinalDeSuscripcion { get; set; }
	}
}
