using Logica.Clases;
using Logica.DAO;
using System;
using System.Windows;
using System.Windows.Controls;
using static Logica.Servicios.ServiciosDeValidacion;

namespace UVFYCliente.Paginas
{
	/// <summary>
	/// Interaction logic for RegistroDeUsuario.xaml
	/// </summary>
	public partial class RegistroDeUsuario : Page
	{
		private IControladorDeCambioDePantalla Controlador { get; set; }
		public RegistroDeUsuario(IControladorDeCambioDePantalla controlador)
		{
			InitializeComponent();
			Controlador = controlador;
		}

		private async void ButtonGuardar_Click(object sender, RoutedEventArgs e)
		{
			if (ComboBoxTipoDeUsuario.SelectedIndex >= 0)
			{
				if (ComboBoxTipoDeUsuario.SelectedIndex == 0)
				{
					if (ValidarDatosDeConsumidor())
					{
						UsuarioDAO usuarioDAO = new UsuarioDAO();
						Artista usuarioARegistrar = new Artista
						{
							NombreDeusuario = TextBoxNombreDeUsuario.Text,
							CorreoElectronico = TextBoxCorreo.Text,
							Contraseña = PasswordBoxContraseña.Password,
							TipoDeUsuario = TipoDeUsuario.Consumidor,
							Nombre = string.Empty,
							Descripcion = string.Empty
						};
						bool resultado = false;
						try
						{
							resultado = await usuarioDAO.RegistrarUsuario(usuarioARegistrar);
						}
						catch (Exception ex)
						{
							MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
							MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						}
						if (resultado)
						{
							MessageBox.Show("Registro exitoso", "¡Exito!");
						}
						Controlador.Regresar();
					}
				}
				else if (ComboBoxTipoDeUsuario.SelectedIndex == 1)
				{
					if (ValidarDatosDeArtista())
					{
						UsuarioDAO usuarioDAO = new UsuarioDAO();
						Artista usuarioARegistrar = new Artista
						{
							NombreDeusuario = TextBoxNombreDeUsuario.Text,
							CorreoElectronico = TextBoxCorreo.Text,
							Contraseña = PasswordBoxContraseña.Password,
							TipoDeUsuario = TipoDeUsuario.Artista,
							Nombre = TextBoxNombreArtista.Text,
							Descripcion = TextBlockDescripcionArtista.Text
						};
						bool resultado = false;
						try
						{
							resultado = await usuarioDAO.RegistrarUsuario(usuarioARegistrar);
						}
						catch (Exception ex)
						{
							MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
							MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						}
						if (resultado)
						{
							MessageBox.Show("Registro exitoso", "¡Exito!");
						}
						Controlador.Regresar();
					}
				}
			}
		}

		private bool ValidarDatosDeArtista()
		{
			bool resultado = false;

			if (ValidarDatosDeConsumidor() &&
				ValidarCadena(TextBoxNombreArtista.Text) &&
				ValidarCadena(TextBlockDescripcionArtista.Text))
			{
				resultado = true;
			}

			return resultado;
		}

		private bool ValidarDatosDeConsumidor()
		{
			bool resultado = false;

			if (ValidarCadena(TextBoxNombreDeUsuario.Text) &&
				ValidarCorreoElectronico(TextBoxCorreo.Text) &&
				ValidarContraseña(PasswordBoxContraseña.Password) &&
				TextBoxCorreo.Text == TextBoxConfirmacionDeCorreo.Text &&
				PasswordBoxContraseña.Password == PasswordBoxConfirmacionDeContraseña.Password)
			{
				resultado = true;
			}

			return resultado;
		}

		private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
		{
			Controlador.Regresar();
		}

		private void ComboBoxTipoDeUsuario_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ComboBoxTipoDeUsuario.SelectedIndex >= 0)
			{
				if (ComboBoxTipoDeUsuario.SelectedIndex == 0)
				{
					Height = 380;
				}
				else if (ComboBoxTipoDeUsuario.SelectedIndex == 1)
				{
					Height = 500;
				}
			}
		}
	}
}
