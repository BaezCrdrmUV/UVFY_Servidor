using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Logica;
using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using UVFYCliente.UserControls;

namespace UVFYCliente.Paginas.PaginasDeConsumidor
{
	/// <summary>
	/// Interaction logic for ListaDeReproduccion.xaml
	/// </summary>
	public partial class ListaDeReproduccion : Window
	{
		private ControladorDeReproduccion Controlador { get; set; }
		private bool ModoConectado { get; set; }
		private List<Cancion> Canciones { get; set; }
		private string Token { get; set; }
		public ListaDeReproduccion(ControladorDeReproduccion controladorDeReproduccion, string token, bool modoConectado)
		{
			InitializeComponent();
			Controlador = controladorDeReproduccion;
			Canciones = Controlador.CancionesEnCola;
			Token = token;
			DataGridListaDeReproduccion.ItemsSource = Canciones;
			CargarDatos();
		}

		private async void CargarDatos()
		{
			await CargarArtistasDeCanciones(Canciones);
			ActualizarLista();
			await CargarAlbumDeCanciones(Canciones);
			ActualizarLista();
		}

		private async Task<bool> CargarArtistasDeCanciones(List<Cancion> canciones)
		{
			ArtistaDAO artistaDAO = new ArtistaDAO(Token);
			foreach (Cancion cancion in canciones)
			{
				if (cancion.Artista != null)
				{
					Artista respuesta;
					try
					{
						respuesta = await artistaDAO.CargarPorId(cancion.Artista.Id);
					}
					catch (Exception ex)
					{
						MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
						MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						return false;
					}

					cancion.Artista = respuesta;
				}
			}
			return true;
		}

		private async Task<bool> CargarAlbumDeCanciones(List<Cancion> canciones)
		{
			AlbumDAO albumDAO = new AlbumDAO(Token);
			foreach (Cancion cancion in canciones)
			{
				if (cancion.Album != null)
				{
					Album respuesta;
					try
					{
						respuesta = await albumDAO.CargarPorId(cancion.Album.Id);
					}
					catch (Exception ex)
					{
						MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
						MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						return false;
					}

					cancion.Album = respuesta;
				}
			}
			return true;
		}

		private void ButtonSubir_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			int indiceDeCancion = Controlador.CancionesEnCola.IndexOf(cancionSeleccionada);
			if (indiceDeCancion != 0)
			{
				Intercambiar(indiceDeCancion, indiceDeCancion - 1);
			}
			ActualizarLista();
		}

		private void ButtonBajar_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			int indiceDeCancion = Controlador.CancionesEnCola.IndexOf(cancionSeleccionada);
			if (indiceDeCancion <= Controlador.CancionesEnCola.Count - 2)
			{
				Intercambiar(indiceDeCancion, indiceDeCancion + 1);
			}
			ActualizarLista();
		}

		private void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			int indiceDeCancion = Controlador.CancionesEnCola.IndexOf(cancionSeleccionada);
			Controlador.SaltarA(indiceDeCancion);
		}

		private void Intercambiar(int indiceA, int indiceB)
		{
			Cancion aux = Controlador.CancionesEnCola[indiceA];
			Controlador.CancionesEnCola[indiceA] = Controlador.CancionesEnCola[indiceB];
			Controlador.CancionesEnCola[indiceB] = aux;
			if(indiceA == Controlador.CancionActual)
			{
				Controlador.CancionActual = indiceB;
			}
			else if(indiceB == Controlador.CancionActual)
			{
				Controlador.CancionActual = indiceA;
			}
		}

		private void ActualizarLista()
		{
			DataGridListaDeReproduccion.ItemsSource = null;
			DataGridListaDeReproduccion.ItemsSource = Canciones;
		}

		private void ButtonDescargar_Click(object sender, RoutedEventArgs e)
		{
			if (ModoConectado)
			{
				Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
				ServiciosDeDescarga serviciosDeDescarga = new ServiciosDeDescarga();
				try
				{
					serviciosDeDescarga.DescargarAudioDeCancion(cancionSeleccionada.Id, Token);
					serviciosDeDescarga.DescargarCaratulaDeCancion(cancionSeleccionada.Id, Token);
					serviciosDeDescarga.DescargarInformacionDeCancion(cancionSeleccionada.Id, Token);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				ActualizarLista();
			}
		}
	}
}
