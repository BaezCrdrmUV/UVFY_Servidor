using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Clases
{
	public class Playlist
	{
		[JsonProperty("id")]
		public int Id { get; set; }
		[JsonProperty("nombre")]
		public string Nombre { get; set; }
		public List<Cancion> Canciones { get; set; }
	}
}
