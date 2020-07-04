using Logica.Clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logica.DAO
{
	public class ArchivosDAO
	{
		public string TokenDeAcceso { get; set; }
		private AdministradorDePeticionesHttp AdministradorDePeticionesHttp = new AdministradorDePeticionesHttp("Archivos/");

		public ArchivosDAO(string tokenDeAcceso)
		{
			TokenDeAcceso = tokenDeAcceso;
		}

		public async Task<byte[]> CargarAudioDeCancionPorId(int idCancion)
		{
			//CalidadDeAudio calidad = (CalidadDeAudio)int.Parse(ConfigurationManager.AppSettings["CalidadDescarga"]);
			CalidadDeAudio calidad = CalidadDeAudio.mp3_320;
			byte[] audioCargado = null;
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idCancion=" + idCancion.ToString() + "&calidad=" + ((int)calidad).ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("AudioDeCancion?" + query.ToString());

			if (respuesta.IsSuccessStatusCode)
			{
				audioCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<byte[]>(respuesta.Content.ReadAsStringAsync().Result);
			}

			return audioCargado;
		}

		public async Task<byte[]> CargarCaratulaDeCancionPorId(int idCancion)
		{
			byte[] audioCargado = null;
			var query = "tokenDeAcceso=" + TokenDeAcceso + "&idCancion=" + idCancion.ToString();
			HttpResponseMessage respuesta;
			respuesta = await AdministradorDePeticionesHttp.Get("CaratulaDeCancion?" + query.ToString());

			if (respuesta.IsSuccessStatusCode)
			{
				audioCargado = Servicios.ServicioDeConversionDeJson.ConvertJsonToClass<byte[]>(respuesta.Content.ReadAsStringAsync().Result);
			}

			return audioCargado;
		}
	}
}
