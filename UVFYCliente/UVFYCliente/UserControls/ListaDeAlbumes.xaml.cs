using Logica.Clases;
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

		private void DataGridAlbumes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Album albumSeleccionado = e.AddedItems[0] as Album;
				AlbumSeleccionado = albumSeleccionado;
			}
		}

		private void ButtonDescargar_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonAñadirACola_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
