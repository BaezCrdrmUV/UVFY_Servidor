using Logica.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.ClasesDeComunicacion
{
	public class RespuestaDeAutenticacion
	{
		[JsonProperty("response")]
		public bool Response { get; set; }
		[JsonProperty("token")]
		public string Token { get; set; }
		[JsonProperty("idUsuario")]
		public int IdUsuario { get; set; }
		[JsonProperty("tipoDeUsuario")]
		public TipoDeUsuario TipoDeUsuario { get; set; }
	}
}
