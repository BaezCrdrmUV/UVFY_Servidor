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
	/// Interaction logic for ListaDePlaylists.xaml
	/// </summary>
	public partial class ListaDePlaylists : UserControl
	{
		private List<Playlist> playlistsCargadas { get; set; }
		private List<Playlist> PlaylistsVisbles { get; set; }
		public List<Playlist> Playlists { get { return playlistsCargadas; } set { playlistsCargadas = value; ActualizarLista(); } }
		public Playlist PlaylistSeleccionada { get; set; }
		private string Token { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion { get; set; }
		public void AsignarControladorDeReproduccion(ControladorDeReproduccion controlador)
		{
			ControladorDeReproduccion = controlador;
		}
		public ListaDePlaylists()
		{
			InitializeComponent();
		}
		public void AsignarToken(string token)
		{
			Token = token;
		}

		private void ActualizarLista()
		{
			DataGridPlaylists.ItemsSource = null;
			DataGridPlaylists.ItemsSource = PlaylistsVisbles;
		}

		public void AsignarPlaylists(List<Playlist> playlistsAMostrar)
		{
			Playlists = playlistsAMostrar;
			PlaylistsVisbles = Playlists;
			ActualizarLista();
		}

		public void Buscar(string busqueda)
		{
			if (Playlists != null)
			{
				if (busqueda != string.Empty)
				{
					PlaylistsVisbles = Playlists.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
				}
				else
				{
					PlaylistsVisbles = playlistsCargadas;
				}
			}
			ActualizarLista();
		}

		private void DataGridPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Playlist playlistSeleccionada = e.AddedItems[0] as Playlist;
				PlaylistSeleccionada = playlistSeleccionada;
			}
		}

		private async void ButtonAñadirACola_Click(object sender, RoutedEventArgs e)
		{
			Playlist playlistSeleccionada = ((FrameworkElement)sender).DataContext as Playlist;
			CancionDAO cancionDAO = new CancionDAO(Token);
			List<Cancion> cancionesDePlaylist;
			try
			{
				cancionesDePlaylist = await cancionDAO.CargarPorIdPlaylist(playlistSeleccionada.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			foreach (Cancion cancion in cancionesDePlaylist)
			{
				ControladorDeReproduccion.AgregarCancionAlFinal(cancion);
			}
		}

		private async void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			Playlist playlistSeleccionada = ((FrameworkElement)sender).DataContext as Playlist;
			CancionDAO cancionDAO = new CancionDAO(Token);
			List<Cancion> cancionesDePlaylist;
			try
			{
				cancionesDePlaylist = await cancionDAO.CargarPorIdPlaylist(playlistSeleccionada.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}

			ControladorDeReproduccion.AsignarCanciones(cancionesDePlaylist);
		}
	}
}
