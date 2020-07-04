using Newtonsoft.Json;

namespace Logica.Clases
{
	public class Genero
	{
        [JsonProperty("id")]
		public int Id { get; set; }
        [JsonProperty("nombre")]
		public string Nombre { get; set; }
        [JsonProperty("descripcion")]
		public string Descripcion { get; set; }
	}
}