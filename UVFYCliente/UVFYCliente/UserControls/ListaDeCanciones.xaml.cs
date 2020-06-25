using Logica;
using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
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
	/// Interaction logic for ListaDeCanciones.xaml
	/// </summary>
	public partial class ListaDeCanciones : UserControl
	{
		private List<Cancion> cancionesCargadas { get; set; }
		private List<Cancion> CancionesVisibles { get; set; }
		private string Token { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion;
		public  List<Cancion> Canciones { get { return cancionesCargadas; } set { cancionesCargadas = value; ActualizarLista(); } }

		private void ActualizarLista()
		{
			DataGridCanciones.ItemsSource = null;
			DataGridCanciones.ItemsSource = CancionesVisibles;
		}

		public ListaDeCanciones()
		{
			InitializeComponent();
		}

		public void AsignarCanciones(List<Cancion> cancionesAMostrar)
		{
			Canciones = cancionesAMostrar;
			CancionesVisibles = Canciones;
			ActualizarLista();
		}

		public void AsignarControladorDeReproduccion(ControladorDeReproduccion controlador)
		{
			ControladorDeReproduccion = controlador;
		}


		public void AsignarToken(string token)
		{
			Token = token;
		}

		public void Buscar(string busqueda)
		{
			if (busqueda != string.Empty)
			{
				CancionesVisibles = Canciones.FindAll(c => c.Nombre.ToLower().Contains(busqueda.ToLower())).ToList();
			}
			else
			{
				CancionesVisibles = cancionesCargadas;
			}
			ActualizarLista();
		}

		private void ButtonDescargar_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			ServiciosDeDescarga serviciosDeDescarga = new ServiciosDeDescarga();
			serviciosDeDescarga.DescargarAudioDeCancion(cancionSeleccionada.Id, Token);
			serviciosDeDescarga.DescargarCaratulaDeCancion(cancionSeleccionada.Id, Token);
			ActualizarLista();
		}

		private void ButtonAñadirACola_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			ControladorDeReproduccion.AgregarCancionAlFinal(cancionSeleccionada);
		}
	}
}
