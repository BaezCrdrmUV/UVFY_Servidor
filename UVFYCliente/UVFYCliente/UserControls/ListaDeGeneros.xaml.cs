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
	/// Interaction logic for ListaDeGeneros.xaml
	/// </summary>
	public partial class ListaDeGeneros : UserControl
	{
		private List<Genero> generosCargados { get; set; }
		private List<Genero> GenerosVisibles { get; set; }
		public List<Genero> Generos { get { return generosCargados; } set { generosCargados = value; ActualizarLista(); } }
		public Genero GeneroSeleccionado { get; set; }
		public ListaDeGeneros()
		{
			InitializeComponent();
		}

		private void ActualizarLista()
		{
			DataGridGeneros.ItemsSource = null;
			DataGridGeneros.ItemsSource = GenerosVisibles;
		}

		public void AsignarGeneros(List<Genero> generosAMostrar)
		{
			Generos = generosAMostrar;
			GenerosVisibles = Generos;
			ActualizarLista();
		}

		public void Buscar(string busqueda)
		{
			if (busqueda != string.Empty)
			{
				GenerosVisibles = Generos.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
			}
			else
			{
				GenerosVisibles = generosCargados;
			}
			ActualizarLista();
		}

		private void DataGridGeneros_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Genero generoSeleccionado = e.AddedItems[0] as Genero;
				GeneroSeleccionado = generoSeleccionado;
			}
		}
	}
}
