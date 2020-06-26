using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Logica.Servicios;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UVFYCliente.Paginas.PaginasDeConsumidor
{
	/// <summary>
	/// Interaction logic for RegistroDePlaylist.xaml
	/// </summary>
	public partial class RegistroDePlaylist : Window
	{
		private Usuario usuarioActual { get; set; } 
		public RegistroDePlaylist(Usuario usuario)
		{
			InitializeComponent();
			usuarioActual = usuario;
		}

		private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private async void ButtonAceptar_Click(object sender, RoutedEventArgs e)
		{
			PlaylistDAO playlistDAO = new PlaylistDAO(usuarioActual.Token);
			bool resultadoDeGuardado = false;
			if (ValidarCampos())
			{
				try
				{
					resultadoDeGuardado = await playlistDAO.CrearPlaylist(TextBoxNombreDePlaylist.Text);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
			}
			else
			{
				MessageBox.Show("El nombre de la playlist que ingreso no es valido", "Error");
				return;
			}
			if (resultadoDeGuardado)
			{
				MessageBox.Show("El la playlist fue creada", "¡Exito!");
			}
			else
			{
				MessageBox.Show("No se pudo guardar la playlist :( /n Intentelo mas tarde", "Vaya");
			}
			Close();
		}

		private bool ValidarCampos()
		{
			bool resultado = false;
			if (ServiciosDeValidacion.ValidarCadena(TextBoxNombreDePlaylist.Text))
			{
				resultado = true;
			}
			return resultado;
		}
	}
}
