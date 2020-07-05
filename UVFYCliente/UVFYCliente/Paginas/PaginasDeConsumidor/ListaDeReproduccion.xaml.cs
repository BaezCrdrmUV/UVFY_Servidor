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
using UVFYCliente.UserControls;

namespace UVFYCliente.Paginas.PaginasDeConsumidor
{
	/// <summary>
	/// Interaction logic for ListaDeReproduccion.xaml
	/// </summary>
	public partial class ListaDeReproduccion : Window
	{
		private ControladorDeReproduccion Controlador { get; set; }
		public ListaDeReproduccion(ControladorDeReproduccion controladorDeReproduccion)
		{
			InitializeComponent();
			Controlador = controladorDeReproduccion;
			DataGridListaDeReproduccion.ItemsSource = Controlador.CancionesEnCola;
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
			DataGridListaDeReproduccion.ItemsSource = Controlador.CancionesEnCola;
		}

		private void ButtonDescargar_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
