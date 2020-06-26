using Logica.Excepciones;
using System;

namespace UVFYCliente
{
	public static class EncadenadorDeExcepciones
	{
		public static MensajeDeErrorParaMessageBox ManejarExcepcion(Exception e)
		{
			LogearExcepcion(e);
			MensajeDeErrorParaMessageBox mensajeDeErrorParaMessageBox = ObtenerMensajeDeErrorParaMessageBox(e);
			return mensajeDeErrorParaMessageBox;
		}

		private static void LogearExcepcion(Exception e)
		{
			System.Console.WriteLine("Exepcion: " + e.Message + "StackTrace: " + e.StackTrace + System.Environment.NewLine);
		}

		private static MensajeDeErrorParaMessageBox ObtenerMensajeDeErrorParaMessageBox(Exception e)
		{
			MensajeDeErrorParaMessageBox mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox();

			if (e is AccesoADatosException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("Hubo un error accediendo a los datos. Por favor intentelo mas tarde", "Error");
			}
			else if (e is AccesoAServicioException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("No nos pudimos conectar al servicio, verifique su conexión e intentelo de nuevo", "Error");
			}
			else if (e is ErrorInternoDeServicioException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("El servicio no esta disponible por el momento", "Error");
			}
			else if (e is RecursoNoExisteException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("El recurso solicitado", "Error");
			}
			else if (e is TokenInvalidoException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("Hubo un error de autenticación, intente reiniciando la aplicación", "Error");
			}
			else if (e is RecursoYaExisteException)
			{
				mensajeDeErrorParaMessageBox = new MensajeDeErrorParaMessageBox("El recurso que esta intentando crear ya existe", "Error");
			}
			return mensajeDeErrorParaMessageBox;
		}
	}
}
