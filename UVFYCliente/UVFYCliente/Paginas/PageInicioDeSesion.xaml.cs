using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static UVFYCliente.UtileriasGráficas;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Logica.Clases;
using Logica.ClasesDeComunicacion;
using Logica.DAO;

namespace UVFYCliente.Paginas
{
	/// <summary>
	/// Interaction logic for PageInicioDeSesion.xaml
	/// </summary>
	public partial class PageInicioDeSesion : Page
	{
		ControladorDeCambioDePantalla Controlador;
		public PageInicioDeSesion(ControladorDeCambioDePantalla controlador)
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
				CorreoElectronico = TextBoxNombreDeUsuario.Text,
				Contraseña = PasswordBoxContraseña.Password
			};
			UsuarioDAO usuarioDAO = new UsuarioDAO();
			var respuestaDeAutenticacion = await usuarioDAO.ValidarUsuario(usuario);
			RespuestaDeAutenticacion respuesta = respuestaDeAutenticacion;

			if (respuesta.Response)
			{
				MessageBox.Show("Exito!" + respuesta.Token + " " + respuesta.IdUsuario + " " + respuesta.TipoDeUsuario);
			}
			else
			{
				MessageBox.Show("Correo electronico o contraseña invalidos");
			}
		}
	}
}
