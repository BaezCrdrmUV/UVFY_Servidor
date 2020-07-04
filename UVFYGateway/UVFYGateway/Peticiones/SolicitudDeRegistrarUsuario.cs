using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYGateway.Dominio;

namespace UVFYGateway.Peticiones
{
	public class SolicitudDeRegistrarUsuario
	{
		public string NombreDeusuario { get; set; }
		public string CorreoElectronico { get; set; }
		public string Contrasena { get; set; }
		public TipoDeUsuario TipoDeUsuario { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }
	}
}
