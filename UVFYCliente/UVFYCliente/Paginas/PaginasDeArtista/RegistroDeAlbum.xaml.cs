using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static UVFYCliente.UtileriasGráficas;

namespace UVFYCliente.Paginas.PaginasDeArtista
{
	/// <summary>
	/// Interaction logic for RegistroDeAlbum.xaml
	/// </summary>
	public partial class RegistroDeAlbum : Window
	{


		private const string ARCHIVO_SELECCIONADO = "Archivo no seleccionado";

		private string DireccionDeArchivoDeCaratula { get; set; }
		private List<int> GenerosSeleccionados { get; set; } = new List<int>();
		private List<Genero> GenerosCargados { get; set; } = new List<Genero>();
		private Usuario UsuarioActual { get; set; } = new Usuario();

		public RegistroDeAlbum(Usuario usuario)
		{
			InitializeComponent();
			ImagenVistaPreviaDeCaratula.Visibility = Visibility.Hidden;
			UsuarioActual = usuario;
			CargarGeneros();
		}

		private async void CargarGeneros()
		{
			GeneroDAO generoDAO = new GeneroDAO(UsuarioActual.Token);
			try
			{
				GenerosCargados = await generoDAO.CargarTodos();
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
			}
			DataGridGeneros.ItemsSource = GenerosCargados;
		}

		private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}


		private void ButtonElegirArchivoDeCaratula_Click(object sender, RoutedEventArgs e)
		{
			DireccionDeArchivoDeCaratula = MostrarVentanaDeSeleccionDeArchivosDeCaratula();

			if (!string.IsNullOrEmpty(DireccionDeArchivoDeCaratula))
			{
				LabelDireccionDeArchivoDeCaratula.Content = DireccionDeArchivoDeCaratula;
				ImagenVistaPreviaDeCaratula.Source = new BitmapImage(new Uri(DireccionDeArchivoDeCaratula));
				ImagenVistaPreviaDeCaratula.Visibility = Visibility.Visible;
			}
			else
			{
				LabelDireccionDeArchivoDeCaratula.Content = ARCHIVO_SELECCIONADO;
				ImagenVistaPreviaDeCaratula.Visibility = Visibility.Hidden;
			}
		}

		private async void ButtonGuardar_Click(object sender, RoutedEventArgs e)
		{
			if (ValidarCampos())
			{
				byte[] datosDeCaratula = ServiciosDeIO.CargarBytesDeArchivo(DireccionDeArchivoDeCaratula);
				foreach (Genero item in DataGridGeneros.ItemsSource)
				{
					if (((CheckBox)CheckBoxColumn.GetCellContent(item)).IsChecked == true)
					{
						GenerosSeleccionados.Add(item.Id);
					}
				}
				AlbumDAO albumDAO = new AlbumDAO(UsuarioActual.Token);
				try
				{
					bool resultado = await albumDAO.RegistrarAlbum(TextBoxNombreDeAlbum.Text, TextBoxDescripcionDeAlbum.Text, GenerosSeleccionados, datosDeCaratula);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				}
				GenerosSeleccionados = new List<int>();
			}
			else
			{

			}
		}

		private bool ValidarCampos()
		{
			bool respuesta = false;
			if (ServiciosDeIO.ExisteArchivo(DireccionDeArchivoDeCaratula))
			{
				if (ServiciosDeValidacion.ValidarCadena(TextBoxNombreDeAlbum.Text) && ServiciosDeValidacion.ValidarCadena(TextBoxDescripcionDeAlbum.Text))
				{
					respuesta = true;
				}
			}

			return respuesta;
		}
	}
}
