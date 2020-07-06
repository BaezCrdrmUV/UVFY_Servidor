using Logica;
using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace UVFYCliente.UserControls
{
	/// <summary>
	/// Interaction logic for ListaDeAlbumes.xaml
	/// </summary>
	public partial class ListaDeAlbumes : UserControl
	{
		private List<Album> albumesCargados { get; set; }
		private List<Album> AlbumesVisibles { get; set; }
		public List<Album> Albumes { get { return albumesCargados; } set { albumesCargados = value; ActualizarLista(); } }
		public Album AlbumSeleccionado { get; set; }
		private string Token { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion { get; set; }
		public void AsignarControladorDeReproduccion(ControladorDeReproduccion controlador)
		{
			ControladorDeReproduccion = controlador;
		}

		public void AsignarToken(string token)
		{
			Token = token;
		}

		public ListaDeAlbumes()
		{
			InitializeComponent();
		}

		private void ActualizarLista()
		{
			DataGridAlbumes.ItemsSource = null;
			DataGridAlbumes.ItemsSource = AlbumesVisibles;
		}

		public void AsignarAlbumes(List<Album> albumesAMostrar)
		{
			Albumes = albumesAMostrar;
			AlbumesVisibles = Albumes;
			ActualizarLista();
		}
		public void Buscar(string busqueda)
		{
			if (Albumes != null)
			{
				if (busqueda != string.Empty)
				{
					AlbumesVisibles = Albumes.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
				}
				else
				{
					AlbumesVisibles = albumesCargados;
				}
				ActualizarLista();
			}
		}

		private void DataGridAlbumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Album albumSeleccionado = e.AddedItems[0] as Album;
				AlbumSeleccionado = albumSeleccionado;
			}
		}


		private async void ButtonAñadirACola_Click(object sender, RoutedEventArgs e)
		{
			Album albumSeleccionado = ((FrameworkElement)sender).DataContext as Album;
			CancionDAO cancionDAO = new CancionDAO(Token);
			List<Cancion> cancionesDeAlbum;
			try
			{
				cancionesDeAlbum = await cancionDAO.CargarPorIdAlbum(albumSeleccionado.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			foreach (Cancion cancion in cancionesDeAlbum)
			{
				ControladorDeReproduccion.AgregarCancionAlFinal(cancion);
			}
		}

		private async void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			Album albumSeleccionado = ((FrameworkElement)sender).DataContext as Album;
			CancionDAO cancionDAO = new CancionDAO(Token);
			List<Cancion> cancionesDeAlbum;
			try
			{
				cancionesDeAlbum = await cancionDAO.CargarPorIdAlbum(albumSeleccionado.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			ControladorDeReproduccion.AsignarCanciones(cancionesDeAlbum);
		}
	}
}
