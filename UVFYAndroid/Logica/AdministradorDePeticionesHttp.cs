using Logica.Excepciones;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Net.Mime;

namespace Logica
{
	public class AdministradorDePeticionesHttp
	{
		private readonly HttpClient Cliente = new HttpClient();
		private string Direccion { get; set; } = "https://192.168.1.69:32802/";

		public AdministradorDePeticionesHttp(string extensionDeDireccion)
		{
			HttpClientHandler manejadorDeCliente = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
			};
			Cliente = new HttpClient(manejadorDeCliente)
			{
				BaseAddress = new Uri(Direccion + extensionDeDireccion),
				Timeout = TimeSpan.FromSeconds(50)
			};
		}

		public async Task<HttpResponseMessage> Get(string peticion)
		{
			HttpResponseMessage respuesta;
			try
			{
				respuesta = await Cliente.GetAsync(peticion);
			}
			catch (Exception e)
			{
				throw new AccesoADatosException(e.Message, e);
			}

			if (!respuesta.IsSuccessStatusCode)
			{
				if (respuesta.StatusCode == (System.Net.HttpStatusCode) 404)
				{
					throw new RecursoNoExisteException(Cliente.BaseAddress + peticion);
				}
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 403)
				{
					throw new TokenInvalidoException(Cliente.BaseAddress + peticion);
				} 
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode) 500)
				{
					throw new ErrorInternoDeServicioException(Cliente.BaseAddress + peticion);
				}
				
				else 
				{
					throw new Exception("Se recibio un codigo de error inesperado: " + respuesta.StatusCode.ToString() + " " +Cliente.BaseAddress+ peticion);
				}
			}

			return respuesta;
		}

		public async Task<HttpResponseMessage> Post<T>(string peticion, T parametros)
		{
			HttpResponseMessage respuesta;
			var datos = JsonConvert.SerializeObject(parametros);
			var data = new StringContent(datos, Encoding.UTF8, "application/json");
			try
			{
				respuesta = await Cliente.PostAsync(peticion, data);
			}
			catch (Exception e)
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
				else if (respuesta.StatusCode == (System.Net.HttpStatusCode)409)
				{
					throw new RecursoYaExisteException(peticion);
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
			catch (Exception e)
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
