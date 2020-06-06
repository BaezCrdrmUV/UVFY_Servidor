using Newtonsoft.Json;
using System.Collections.Generic;

namespace Logica.Clases
{
	public class Album
	{
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("generos")]
        public List<Genero> Generos { get; set; }
        [JsonProperty("artista")]
        public Artista Artista { get; set; }
        [JsonProperty("imagen")]
        public byte[] Imagen { get; set; }
    }
}