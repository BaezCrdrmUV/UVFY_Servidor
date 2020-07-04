using Newtonsoft.Json;

namespace Logica.Clases
{
	public class Usuario
	{
		[JsonProperty("idUsuario")]
		public int Id { get; set; }
        [JsonProperty("nombreDeUsuario")]
		public string NombreDeusuario { get; set; }
        [JsonProperty("contrasena")]
		public string Contrasena { get; set; }
        [JsonProperty("correoElectronico")]
		public string CorreoElectronico { get; set; }
        [JsonProperty("tipoDeUsuario")]
		public TipoDeUsuario TipoDeUsuario { get; set; }
		public string Token { get; set; }
	}
}
