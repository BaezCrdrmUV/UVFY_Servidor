using Logica.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.ClasesDeComunicacion
{
	public class SolicitudDeRegistrarUsuario
	{
		public string NombreDeusuario { get; set; }
		public string CorreoElectronico { get; set; }
		public string Contraseña { get; set; }
		public TipoDeUsuario TipoDeUsuario { get; set; }
		public string Descripcion { get; set; }
		public string Nombre { get; set; }
	}
}
