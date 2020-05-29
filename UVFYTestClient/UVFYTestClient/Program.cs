using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace UVFYTestClient
{
	class Program
	{
		static HttpClient client = new HttpClient();
		static void Main(string[] args)
		{

			HttpClientHandler clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
			client = new HttpClient(clientHandler);
			client.BaseAddress = new Uri("https://127.0.0.1:32772/");
			CrearUsuario(new Usuario
			{
				NombreDeusuario = "Pepito56",
				Contraseña = "pepin89",
				CorreoElectronico = "Pepo@correo.com",
				TipoDeUsuario = TipoDeUsuario.Consumidor
			}).GetAwaiter().GetResult();
			
		}

		static async Task CrearUsuario(Usuario usuario)
		{
			HttpResponseMessage respuesta = await client.PostAsJsonAsync("Autenticacion/Registrar", usuario);
			Console.WriteLine(respuesta.StatusCode.ToString() + respuesta.Content);
		}
	}
}
