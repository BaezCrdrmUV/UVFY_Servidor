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
	/// Interaction logic for ListaDeArtistas.xaml
	/// </summary>
	public partial class ListaDeArtistas : UserControl
	{
		private List<Artista> artistasCargados { get; set; }
		private List<Artista> ArtistasVisibles { get; set; }
		public List<Artista> Artistas { get { return artistasCargados; } set { artistasCargados = value; ActualizarLista(); } }
		public Artista ArtistaSeleccionado { get; set; }
		public ListaDeArtistas()
		{
			InitializeComponent();
		}

		private void ActualizarLista()
		{
			DataGridArtistas.ItemsSource = null;
			DataGridArtistas.ItemsSource = ArtistasVisibles;
		}

		public void AsignarArtistas(List<Artista> artistasAMostrar)
		{
			Artistas = artistasAMostrar;
			ArtistasVisibles = Artistas;
			ActualizarLista();
		}

		public void Buscar(string busqueda)
		{
			if (busqueda != string.Empty)
			{
				ArtistasVisibles = Artistas.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
			}
			else
			{
				ArtistasVisibles = artistasCargados;
			}
			ActualizarLista();
		}

		private void DataGridCanciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			if (e.AddedItems.Count == 1)
			{
				Artista artistaSeleccionado = e.AddedItems[0] as Artista;
				ArtistaSeleccionado = artistaSeleccionado;
			}
		}
	}
}
