using Logica.Clases;
using Logica.ClasesDeComunicacion;
using Logica.DAO;
using System;
using System.Windows;
using System.Windows.Controls;
using Logica;
using static UVFYCliente.UtileriasGráficas;
using System.Configuration;

namespace UVFYCliente.Paginas
{
	/// <summary>
	/// Interaction logic for PageInicioDeSesion.xaml
	/// </summary>
	public partial class PageInicioDeSesion : Page
	{
		IControladorDeCambioDePantalla Controlador;
		public PageInicioDeSesion(IControladorDeCambioDePantalla controlador)
		{
			Controlador = controlador;
			InitializeComponent();
			TextBoxNombreDeUsuario.Focus();
		}

		private void TextBoxNombreDeUsuario_TextChanged(object sender, TextChangedEventArgs e)
		{
			MostrarEstadoDeValidacionCadena(sender as TextBox);
		}

		private void PasswordBoxContraseña_PasswordChanged(object sender, RoutedEventArgs e)
		{
			MostrarEstadoDeValidacionContraseña(sender as PasswordBox);
		}
		private async void ButtonIniciarSesion_Click(object sender, RoutedEventArgs e)
		{
			Usuario usuario = new Usuario()
			{
				CorreoElectronico = "pachy@correo.com",
				Contrasena = "perros"
			};
			UsuarioDAO usuarioDAO = new UsuarioDAO();
			RespuestaDeAutenticacion respuesta = new RespuestaDeAutenticacion();
			try
			{
				respuesta = await usuarioDAO.ValidarUsuario(usuario);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				usuario.Id = int.Parse(ConfigurationManager.AppSettings["IdUltimoUsuario"]);
				if ( usuario.Id != 0)
				{
					MessageBoxResult resultadoDeMessageBox = MessageBox.Show("Puede inciar sesion como el ultimo usuario que se conecto, sin embargo solo tendra acceso a sus canciones descargadas.\n ¿Desea iniciar sesion de esta forma?", "Aviso", MessageBoxButton.YesNo);
					if (resultadoDeMessageBox == MessageBoxResult.Yes)
					{
						Consumidor.PantallaPrincipalDeConsumidor pantallaPrincipalDeConsumidor = new Consumidor.PantallaPrincipalDeConsumidor(usuario, Controlador, false);
						Controlador.CambiarANuevaPage(pantallaPrincipalDeConsumidor);
					}
						return;
				}
			}

			if (respuesta.Response)
			{
				usuario.Token = respuesta.Token;
				usuario.TipoDeUsuario = respuesta.TipoDeUsuario;
				usuario.Id = respuesta.IdUsuario;
				
				if (respuesta.TipoDeUsuario == TipoDeUsuario.Consumidor)
				{
					Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					config.AppSettings.Settings["IdUltimoUsuario"].Value = usuario.Id.ToString();
					config.Save(ConfigurationSaveMode.Modified);
					Consumidor.PantallaPrincipalDeConsumidor pantallaPrincipalDeConsumidor = new Consumidor.PantallaPrincipalDeConsumidor(usuario, Controlador, true);
					Controlador.CambiarANuevaPage(pantallaPrincipalDeConsumidor);
				}
				else if (respuesta.TipoDeUsuario == TipoDeUsuario.Artista)
				{
					PaginasDeArtista.PantallaPrincipalDeArtista pantallaPrincipalDeArtista = new PaginasDeArtista.PantallaPrincipalDeArtista(usuario, Controlador);
					Controlador.CambiarANuevaPage(pantallaPrincipalDeArtista);
				}
				else
				{
					MessageBox.Show("Error, usuario indefinido" + respuesta.Token + " " + respuesta.IdUsuario);
				}
			}
			else
			{
				MessageBox.Show("Correo electronico o contraseña invalidos");
			}
		}

		private void ButtonCrearCuenta_Click(object sender, RoutedEventArgs e)
		{
			RegistroDeUsuario registroDeUsuario = new RegistroDeUsuario(Controlador);
			Controlador.CambiarANuevaPage(registroDeUsuario);
		}
	}
}
