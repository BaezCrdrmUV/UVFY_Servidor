using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UVFYGateway.Peticiones
{
	public class SolicitudDeRegistrarCancion
	{
		public Token token { get; set; }
		public string nombre { get; set; }
		public List<int> generos { get; set; }
		public byte[] audio { get; set; }
		public byte[] imagen { get; set; }
		public int duracion { get; set; }
	}
}
