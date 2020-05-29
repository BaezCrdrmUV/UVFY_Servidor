using System;
using System.Collections.Generic;
using System.Text;

namespace UVFYTestClient
{
	public class Consumidor : Usuario
	{
		public bool EstadoDeSuscripcion { get; set; }
		public DateTime FechaDeFinalDeSuscripcion { get; set; }
	}
}
