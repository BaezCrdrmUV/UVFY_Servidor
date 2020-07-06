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
		private List<Playlist> PlaylistsEnMenuDeContexto { get; set; } = new List<Playlist>();
		private Cancion CancionSeleccionada { get; set; } 
		private string Token { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion;
		public  List<Cancion> Canciones { get { return cancionesCargadas; } set { cancionesCargadas = value; ActualizarLista(); } }
		private bool ModoConectado { get; set; } = true;


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

		public void AsignarPlaylistsEnMenuDeContexto(List<Playlist> playlists)
		{
			PlaylistsEnMenuDeContexto = playlists;
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
			if (Canciones != null)
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
		}

		internal void AsignarModoConectado(bool modoConectado)
		{
			ModoConectado = modoConectado;
		}

		private void ButtonDescargar_Click(object sender, RoutedEventArgs e)
		{
			if (ModoConectado)
			{
				Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
				ServiciosDeDescarga serviciosDeDescarga = new ServiciosDeDescarga();
				try
				{
					serviciosDeDescarga.DescargarAudioDeCancion(cancionSeleccionada.Id, Token);
					serviciosDeDescarga.DescargarCaratulaDeCancion(cancionSeleccionada.Id, Token);
					serviciosDeDescarga.DescargarInformacionDeCancion(cancionSeleccionada.Id, Token);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				ActualizarLista();
			}
		}

		private void ButtonAñadirACola_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			ControladorDeReproduccion.AgregarCancionAlFinal(cancionSeleccionada);
			MessageBox.Show("Cancion añadida");
		}

		private void DataGridCanciones_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (DataGridCanciones.SelectedItem != null)
			{


				while ((DataGridCanciones.ContextMenu.Items[0] as MenuItem).Items.Count > 1)
				{
					(DataGridCanciones.ContextMenu.Items[0] as MenuItem).Items.RemoveAt(1);
				}
				if (DataGridCanciones.ContextMenu.Items.Count > 1)
				{
					DataGridCanciones.ContextMenu.Items.RemoveAt(1);
				}
				if (CancionSeleccionada.CancionEstaDescargada())
				{
					MenuItem eliminarCancion = new MenuItem();
					eliminarCancion.Header = "Eliminar canción";
					eliminarCancion.Click += EliminarCancion_Click;
					DataGridCanciones.ContextMenu.Items.Add(eliminarCancion);
				}

				if (ModoConectado)
				{
					foreach (Playlist playlist in PlaylistsEnMenuDeContexto)
					{
						MenuItem opcionDePlaylist = new MenuItem
						{
							Header = playlist.Nombre,
						};
						opcionDePlaylist.Click += OpcionDePlaylists_Click;
						(DataGridCanciones.ContextMenu.Items[0] as MenuItem).Items.Add(opcionDePlaylist);
					}
				}
			}
		}

		private void EliminarCancion_Click(object sender, RoutedEventArgs e)
		{
			ServiciosDeDescarga serviciosDeDescarga = new ServiciosDeDescarga();
			serviciosDeDescarga.EliminarAudioDeCancion(CancionSeleccionada.Id);
			serviciosDeDescarga.EliminarCaratulaDeCancion(CancionSeleccionada.Id);
			ActualizarLista();
		}

		private async void OpcionDePlaylists_Click(object sender, RoutedEventArgs e)
		{
			MenuItem seleccion = sender as MenuItem;
			Playlist playlistSeleccionada = PlaylistsEnMenuDeContexto.FirstOrDefault(p => p.Nombre == seleccion.Header.ToString());
			if ( playlistSeleccionada != null)
			{
				PlaylistDAO playlistDAO = new PlaylistDAO(Token);
				bool resultado = false;
				try
				{
					resultado = await playlistDAO.AgregarCancionAPlaylist(playlistSeleccionada.Id, CancionSeleccionada.Id);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				if (resultado)
				{
					MessageBox.Show("Cancion agregada");
				}
				else
				{
					MessageBox.Show("Hubo un error agregando la cancion, intentelo mas tarde" ,"Vaya");
				}
			}
			else
			{
				MessageBox.Show("Hubo un error seleccionando la playlist. Intentelo de nuevo");
			}
		}

		private void DataGridCanciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				Cancion cancionSeleccionada = e.AddedItems[0] as Cancion;
				CancionSeleccionada = cancionSeleccionada;
			}
		}

		private void NuevaPlaylistClick(object sender, RoutedEventArgs e)
		{
			Paginas.PaginasDeConsumidor.RegistroDePlaylist registroDePlaylist = new Paginas.PaginasDeConsumidor.RegistroDePlaylist(new Usuario() { Token = Token });
			registroDePlaylist.ShowDialog();
		}

		private void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			Cancion cancionSeleccionada = ((FrameworkElement)sender).DataContext as Cancion;
			ControladorDeReproduccion.AsignarCanciones(CancionesVisibles.Skip(CancionesVisibles.IndexOf(cancionSeleccionada)).ToList());
		}
	}
}
