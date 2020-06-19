using Logica.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica.DAO
{
	public class GeneroDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Metadatos/Generos/");

		public GeneroDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<List<Genero>> CargarTodos()
		{
			List<Genero> generosCargados = new List<Genero>();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("Todos?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				generosCargados = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<List<Genero>>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return generosCargados;
		}

		public async Task<Genero> CargarPorId(int idGenero)
		{
			Genero generoCargado = new Genero();
			var query = HttpUtility.ParseQueryString(string.Empty);
			query["tokenDeAcceso"] = TokenDeAcceso;
			query["idGenero"] = idGenero.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("PorID?" + query.ToString());
			if (respuesta.IsSuccessStatusCode)
			{
				generoCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<Genero>(respuesta.Content.ReadAsStringAsync().Result);
			}
			return generoCargado;
		}
	}
}
