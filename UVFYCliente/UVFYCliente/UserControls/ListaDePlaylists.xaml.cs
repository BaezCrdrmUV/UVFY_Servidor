	using Logica.Clases;
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
		public ListaDePlaylists()
		{
			InitializeComponent();
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
			if (busqueda != string.Empty)
			{
				PlaylistsVisbles = Playlists.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
			}
			else
			{
				PlaylistsVisbles = playlistsCargadas;
			}
			ActualizarLista();
		}

		private void DataGridCanciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (DataGridPlaylists.SelectedItem != null)
			{
				Playlist albumSeleccionado = ((FrameworkElement)sender).DataContext as Playlist;
				PlaylistSeleccionada = albumSeleccionado;
			}
		}
	}
}
