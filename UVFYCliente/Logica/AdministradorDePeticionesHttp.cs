using Logica.Excepciones;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Logica
{
	public class AdministradorDePeticionesHttp
	{
		static HttpClient Cliente = new HttpClient();

		public AdministradorDePeticionesHttp(string extensionDeDireccion)
		{
			HttpClientHandler manejadorDeCliente = new HttpClientHandler();
			manejadorDeCliente.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			Cliente = new HttpClient(manejadorDeCliente);
			Cliente.BaseAddress = new Uri(ConfigurationManager.AppSettings["DireccionBase"] + extensionDeDireccion);
		}

		public async Task<HttpResponseMessage> Get(string peticion)
		{
			HttpResponseMessage respuesta;
			try
			{
				respuesta = await Cliente.GetAsync(peticion);
			}
			catch (HttpException e)
			{
				throw new AccesoADatosException(e.Message, e);
			}

			if (!respuesta.IsSuccessStatusCode)
			{
				if (respuesta.StatusCode == (System.Net.HttpStatusCode) 404)
				{
					throw new RecursoNoExisteException();
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 403)
				{
					throw new TokenInvalidoException();
				} 
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 500)
				{
					throw new ErrorInternoDeServicioException();
				}
				else 
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString());
				}
			}

			return respuesta;
		}

		public async Task<HttpResponseMessage> Post<T>(string peticion, T objeto)
		{
			HttpResponseMessage respuesta;
			try
			{
				respuesta = await Cliente.PostAsJsonAsync(peticion, objeto);
			}
			catch (HttpException e)
			{
				throw new AccesoADatosException(e.Message, e);
			}

			if (!respuesta.IsSuccessStatusCode)
			{
				if (respuesta.StatusCode == (System.Net.HttpStatusCode)404)
				{
					throw new RecursoNoExisteException();
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)403)
				{
					throw new TokenInvalidoException();
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)500)
				{
					throw new ErrorInternoDeServicioException();
				}
				else
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString());
				}
			}

			return respuesta;
		}
	}
}
