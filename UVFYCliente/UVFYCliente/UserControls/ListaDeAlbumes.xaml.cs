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
	/// Interaction logic for ListaDeAlbumes.xaml
	/// </summary>
	public partial class ListaDeAlbumes : UserControl
	{
		private List<Album> albumesCargados { get; set; }
		private List<Album> AlbumesVisibles { get; set; }
		public List<Album> Albumes { get { return albumesCargados; } set { albumesCargados = value; ActualizarLista(); } }
		public Album AlbumSeleccionado { get; set; }
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

		private void DataGridCanciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (DataGridAlbumes.SelectedItem != null)
			{
				Album albumSeleccionado = ((FrameworkElement)sender).DataContext as Album;
				AlbumSeleccionado = albumSeleccionado;
			}
		}
	}
}
