using Newtonsoft.Json;
using System.Collections.Generic;

namespace Logica.Clases
{
	public class Artista : Usuario
	{
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}