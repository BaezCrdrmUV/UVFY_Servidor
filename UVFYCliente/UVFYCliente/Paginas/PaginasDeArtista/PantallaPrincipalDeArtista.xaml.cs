using Logica.Clases;
using Logica.DAO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UVFYCliente.Paginas.PaginasDeArtista
{
	/// <summary>
	/// Interaction logic for PantallaPrincipalDeArtista.xaml
	/// </summary>
	public partial class PantallaPrincipalDeArtista : Page
	{
		private Usuario Artista { get; set; }
		private IControladorDeCambioDePantalla Controlador { get; set; }
		private List<Cancion> CancionesCargadas { get; set; }
		public List<Album> AlbumesCargados { get; set; }
		public PantallaPrincipalDeArtista(Usuario usuarioCargado, IControladorDeCambioDePantalla controlador)
		{
			InitializeComponent();
			Artista = usuarioCargado;
			Controlador = controlador;
			CargarDatos();
		}

		private void ButtonEstudioDeCanciones_Click(object sender, RoutedEventArgs e)
		{
			EstudioDeCanciones estudioDeCanciones = new EstudioDeCanciones(Artista, Controlador);
			Controlador.CambiarANuevaPage(estudioDeCanciones);
		}

		private async void CargarDatos()
		{
			try
			{
				CancionDAO cancionDAO = new CancionDAO(Artista.Token);
				CancionesCargadas = await cancionDAO.CargarPorIdArtista(Artista.Id);
				AlbumDAO albumDAO = new AlbumDAO(Artista.Token);
				AlbumesCargados = await albumDAO.CargarPorIdArtista(Artista.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
			}
			if(CancionesCargadas.Count == 1 && CancionesCargadas[0].Id == 0)
			{
				DataGridListaDeCanciones.Visibility = Visibility.Collapsed;
				LabelNoHayCanciones.Visibility = Visibility.Visible;
			}
			else
			{
				DataGridListaDeCanciones.Visibility = Visibility.Visible;
				LabelNoHayCanciones.Visibility = Visibility.Collapsed;
			}

			if (AlbumesCargados.Count == 1 && AlbumesCargados[0].Id == 0)
			{
				DataGridListaDeAlbumes.Visibility = Visibility.Collapsed;
				LabelNoHayAlbumes.Visibility = Visibility.Visible;
			}
			else
			{
				DataGridListaDeAlbumes.Visibility = Visibility.Visible;
				LabelNoHayAlbumes.Visibility = Visibility.Collapsed;
			}
			DataGridListaDeAlbumes.ItemsSource = AlbumesCargados;
			DataGridListaDeCanciones.ItemsSource = CancionesCargadas;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Controlador.Regresar();
		}
	}
}
