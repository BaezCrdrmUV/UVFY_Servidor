using Logica.Clases;
using Logica.ClasesDeComunicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica.DAO
{
	public class UsuarioDAO
	{
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Autenticacion/");

		public UsuarioDAO()
		{
			
		}

		public async Task<RespuestaDeAutenticacion> ValidarUsuario(Usuario usuario)
		{
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Post("Autenticar", usuario);
			RespuestaDeAutenticacion respuestaDeAutenticacion = new RespuestaDeAutenticacion();
			if (respuesta.IsSuccessStatusCode)
			{
				respuestaDeAutenticacion = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<RespuestaDeAutenticacion>(respuesta.Content.ReadAsStringAsync().Result);
			}

			return respuestaDeAutenticacion;
		}
	}
}
