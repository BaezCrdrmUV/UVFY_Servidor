using Logica.Excepciones;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Text;
using System.Net.Mime;

namespace Logica
{
	public class AdministradorDePeticionesHttp
	{
		static HttpClient Cliente = new HttpClient();
		static string Direccion;

		public AdministradorDePeticionesHttp(string extensionDeDireccion)
		{
			Direccion = ConfigurationManager.AppSettings["DireccionBase"] + extensionDeDireccion;
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
					throw new RecursoNoExisteException(peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 403)
				{
					throw new TokenInvalidoException(peticion);
				} 
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 500)
				{
					throw new ErrorInternoDeServicioException(peticion);
				}
				else 
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString() + " " + peticion);
				}
			}

			return respuesta;
		}

		public async Task<HttpResponseMessage> Post<T>(string peticion, T parametros)
		{
			HttpResponseMessage respuesta;
			HttpRequestMessage request = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(Direccion + peticion),
				Content = new StringContent(JsonConvert.SerializeObject(parametros), Encoding.UTF8, "application/json")
			};
			try
			{
				respuesta = await Cliente.SendAsync(request);
			}
			catch (HttpException e)
			{
				throw new AccesoADatosException(e.Message, e);
			}
			catch(HttpRequestException e)
			{
				throw new AccesoAServicioException(e.Message, e);
			}

			if (!respuesta.IsSuccessStatusCode)
			{
				if (respuesta.StatusCode == (System.Net.HttpStatusCode)404)
				{
					throw new RecursoNoExisteException(peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)403)
				{
					throw new TokenInvalidoException(peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)500)
				{
					throw new ErrorInternoDeServicioException(peticion);
				}
				else
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString() + " " + peticion);
				}
			}

			return respuesta;
		}

		public async Task<HttpResponseMessage> Delete(string peticion)
		{
			HttpResponseMessage respuesta;
			try
			{
				respuesta = await Cliente.DeleteAsync(peticion);
			}
			catch (HttpException e)
			{
				throw new AccesoADatosException(e.Message, e);
			}

			if (!respuesta.IsSuccessStatusCode)
			{
				if (respuesta.StatusCode == (System.Net.HttpStatusCode)404)
				{
					throw new RecursoNoExisteException(peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)403)
				{
					throw new TokenInvalidoException(peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)500)
				{
					throw new ErrorInternoDeServicioException(peticion);
				}
				else
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString() + " " + peticion);
				}
			}

			return respuesta;
		}
	}
}
