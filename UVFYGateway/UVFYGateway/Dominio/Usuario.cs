using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYGateway.Dominio;

namespace UVFYGateway
{
	public class Usuario
	{
		public string NombreDeusuario { get; set; }
		public string CorreoElectronico{ get; set; }
		public string Contraseña { get; set; }
		public TipoDeUsuario TipoDeUsuario{ get; set; }
	}
}
