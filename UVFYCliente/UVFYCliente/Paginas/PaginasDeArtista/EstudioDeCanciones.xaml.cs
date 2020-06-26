using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace UVFYCliente.Paginas.PaginasDeArtista
{
	/// <summary>
	/// Interaction logic for EstudioDeCanciones.xaml
	/// </summary>
	public partial class EstudioDeCanciones : Page
	{
		private Usuario Artista { get; set; }
		private IControladorDeCambioDePantalla Controlador { get; set; }
		private List<Cancion> CancionesPrivadas { get; set; }
		private List<Cancion> CancionesDeAlbum { get; set; }
		private List<Album> AlbumesCargados { get; set; }
		public EstudioDeCanciones(Usuario usuarioCargado, IControladorDeCambioDePantalla controlador)
		{
			InitializeComponent();
			Artista = usuarioCargado;
			Controlador = controlador;
			CargarDatos();
		}

		private async void CargarDatos()
		{
			try
			{
				CancionDAO cancionDAO = new CancionDAO(Artista.Token);
				CancionesPrivadas = await cancionDAO.CargarPrivadasPorIdArtista(Artista.Id);
				AlbumDAO albumDAO = new AlbumDAO(Artista.Token);
				AlbumesCargados = await albumDAO.CargarPorIdArtista(Artista.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
			}
			DataGridListaDeAlbumes.ItemsSource = AlbumesCargados;
			DataGridListaDeCancionesSinAlbum.ItemsSource = CancionesPrivadas;
			if (DataGridListaDeAlbumes.Items.Count > 0)
			{
				DataGridListaDeAlbumes.SelectedItem = DataGridListaDeAlbumes.Items[0];
			}
		}

		private async void ButtonEliminarAlbum_Click(object sender, RoutedEventArgs e)
		{
			Album albumSeleccionado = ((FrameworkElement)sender).DataContext as Album;
			AlbumDAO albumDAO = new AlbumDAO(Artista.Token);
			try
			{
				bool resultado = await albumDAO.Eliminar(albumSeleccionado.Id);
				CargarDatos();
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
			}
		}

		private async void ButtonEliminarCancionDeAlbum_Click(object sender, RoutedEventArgs e)
		{
			Album albumSeleccionado = (Album)DataGridListaDeAlbumes.SelectedItem;
			if (albumSeleccionado != null)
			{
				Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
				AlbumDAO albumDAO = new AlbumDAO(Artista.Token);
				try
				{
					bool resultado = await albumDAO.EliminarCancionDeAlbum(albumSeleccionado.Id, cancionSeleccionada.Id);
					CargarDatos();
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				}
			}
			else
			{
				MessageBox.Show("Debe elegir un album", "Ningun album seleccionado");
			}
		}

		private async void ButtonEliminarCancion_Click(object sender, RoutedEventArgs e)
		{
			Cancion albumSeleccionado = ((FrameworkElement)sender).DataContext as Cancion;
			CancionDAO cancionDAO = new CancionDAO(Artista.Token);
			try
			{
				bool resultado = await cancionDAO.Eliminar(albumSeleccionado.Id);
				CargarDatos();
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
			}
		}

		private async void ButtonAgregarCancionAAlbum_Click(object sender, RoutedEventArgs e)
		{
			Album albumSeleccionado = (Album)DataGridListaDeAlbumes.SelectedItem;
			if (albumSeleccionado != null)
			{
				Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
				AlbumDAO albumDAO = new AlbumDAO(Artista.Token);
				try
				{
					bool resultado = await albumDAO.AgregarCancionAAlbum(albumSeleccionado.Id, cancionSeleccionada.Id);
					CargarDatos();
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				}
			}
			else
			{
				MessageBox.Show("Debe elegir un album", "Ningun album seleccionado");
			}
		}
		private async void DataGridListaDeAlbumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Album albumSeleccionado = e.AddedItems[0] as Album;

				CancionDAO cancionDAO = new CancionDAO(Artista.Token);
				try
				{
					CancionesDeAlbum = await cancionDAO.CargarPorIdAlbum(albumSeleccionado.Id);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				}
				DataGridListaDeCancionesEnAlbum.ItemsSource = CancionesDeAlbum;
			}
		}

		private void ButtonNuevaCancion_Click(object sender, RoutedEventArgs e)
		{
			RegistroDeCancion registroDeCancion = new RegistroDeCancion(Artista, TipoDeUsuario.Artista);
			registroDeCancion.ShowDialog();
			CargarDatos();
		}

		private void ButtonNuevoAlbum_Click(object sender, RoutedEventArgs e)
		{
			RegistroDeAlbum registroDeAlbum = new RegistroDeAlbum(Artista);
			registroDeAlbum.ShowDialog();
			CargarDatos();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Controlador.Regresar();
		}
	}
}
