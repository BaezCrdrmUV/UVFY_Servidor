using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Servicios
{
	public static class ServicioDeConversionDeJson
	{
		public static T ConvertJsonToClass<T>(string jsonData)
		{
			var result = default(T);

			if (!string.IsNullOrEmpty(jsonData))
			{
				result = JsonConvert.DeserializeObject<T>(jsonData);
			}
			return result;
		}

		public static ByteArrayContent SerializarPeticion<T>(T peticion)
		{
			var peticionSerliazada = JsonConvert.SerializeObject(peticion);
			var buffer = Encoding.UTF8.GetBytes(peticionSerliazada);
			var bytesDePeticion = new ByteArrayContent(buffer);
			bytesDePeticion.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return bytesDePeticion;
		}

		public static HttpRequestMessage SerializarDelete()
		{
			var request = new HttpRequestMessage(HttpMethod.Delete, "http://www.example.com/");
			request.Content = new StringContent(JsonConvert.SerializeObject(object), Encoding.UTF8, "application/json");
		}
	}
}
