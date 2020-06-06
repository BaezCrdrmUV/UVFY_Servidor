using Newtonsoft.Json;

namespace Logica.Clases
{
	public class Usuario
	{
        [JsonProperty("nombreDeUsuario")]
		public string NombreDeusuario { get; set; }
        [JsonProperty("contraseña")]
		public string Contraseña { get; set; }
        [JsonProperty("correoElectronico")]
		public string CorreoElectronico { get; set; }
        [JsonProperty("tipoDeUsuario")]
		public TipoDeUsuario TipoDeUsuario { get; set; }
	}
}
