using Logica;
using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using UVFYCliente.Paginas.PaginasDeArtista;
using UVFYCliente.Paginas.PaginasDeConsumidor;
using UVFYCliente.UserControls;

namespace UVFYCliente.Paginas.Consumidor
{
	/// <summary>
	/// Interaction logic for PantallaPrincipalDeConsumidor.xaml
	/// </summary>
	public partial class PantallaPrincipalDeConsumidor : Page
	{
		private IControladorDeCambioDePantalla Controlador { get; set; }
		private Usuario UsuarioActual { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion { get; set; } = new ControladorDeReproduccion();
		private List<Playlist> PlaylistsDeUsuarioActual { get; set; } = new List<Playlist>();
		private bool CargarCancionesLibre { get; set; } = true;
		private bool CargarArtistasLibre { get; set; } = true;
		private bool CargarGenerosLibre { get; set; } = true;
		private bool CargarCancionesDescargadasLibre { get; set; } = true;
		private bool CargarPlaylistsLibre { get; set; } = true;
		private bool CargarPrivadasLibre { get; set; } = true;
		private bool ModoConectado { get; set; } = true;
		private int IndiceDePestañaSeleccionada { get; set; } = 0;
		public PantallaPrincipalDeConsumidor(Usuario usuario, IControladorDeCambioDePantalla controlador, bool modoConectado)
		{
			InitializeComponent();
			Controlador = controlador;
			UsuarioActual = usuario;
			ModoConectado = modoConectado;
			if (ModoConectado)
			{
				Inicializar();
			}
			else
			{
				InicializarSinConexion();
			}
		}

		private void InicializarSinConexion()
		{
			ServiciosDeIO.AsignarIdUsuarioActual(UsuarioActual.Id.ToString());
			Reproductor.AsignarControlador(ControladorDeReproduccion);
			ControladorDeReproduccion.AsignarInterfaz(Reproductor);
			(TabControlPaneles.Items[0] as TabItem).IsEnabled = false;
			(TabControlPaneles.Items[1] as TabItem).IsEnabled = false;
			(TabControlPaneles.Items[2] as TabItem).IsEnabled = false;
			(TabControlPaneles.Items[4] as TabItem).IsEnabled = false;
			(TabControlPaneles.Items[5] as TabItem).IsEnabled = false;
			TabControlPaneles.SelectedIndex = 3;
			IndiceDePestañaSeleccionada = TabControlPaneles.SelectedIndex;
			Reproductor.AsignarModoConectado(false);
			ControladorDeReproduccion.AsignarModoConectado(false);
			ListaDeCancionesDescargadas.AsignarModoConectado(false);
			PropagarControladorDeReproduccion();
		}

		private async Task<bool> CargarCanciones()
		{
			if (CargarCancionesLibre)
			{
				CargarCancionesLibre = false;
				CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
				var respuesta = await cancionDAO.CargarTodas();
				if (respuesta.Count == 1 && respuesta[0].Id == 0)
				{
					ListaDeCanciones.Visibility = Visibility.Collapsed;
					LabelNoHayCanciones.Visibility = Visibility.Visible;
				}
				else
				{
					ListaDeCanciones.Visibility = Visibility.Visible;
					LabelNoHayCanciones.Visibility = Visibility.Collapsed;
					ListaDeCanciones.AsignarCanciones(respuesta);
					await CargarArtistasDeCanciones(respuesta);
					ListaDeCanciones.AsignarCanciones(respuesta);
					await CargarAlbumDeCanciones(respuesta);
					ListaDeCanciones.AsignarCanciones(respuesta);
				}
				CargarCancionesLibre = true;
			}
			return true;
		}

		private async Task<bool> CargarArtistasDeCanciones(List<Cancion> canciones)
		{
			ArtistaDAO artistaDAO = new ArtistaDAO(UsuarioActual.Token);
			foreach (Cancion cancion in canciones)
			{
				if (cancion.Artista != null)
				{
					Artista respuesta;
					try
					{
						respuesta = await artistaDAO.CargarPorId(cancion.Artista.Id);
					}
					catch (Exception ex)
					{
						MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
						MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						return false;
					}

					cancion.Artista = respuesta;
				}
			}
			return true;
		}

		private async Task<bool> CargarAlbumDeCanciones(List<Cancion> canciones)
		{
			AlbumDAO albumDAO = new AlbumDAO(UsuarioActual.Token);
			foreach (Cancion cancion in canciones)
			{
				if (cancion.Album != null)
				{
					Album respuesta;
					try
					{
						respuesta = await albumDAO.CargarPorId(cancion.Album.Id);
					}
					catch (Exception ex)
					{
						MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
						MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
						return false;
					}

					cancion.Album = respuesta;
				}
			}
			return true;
		}

		private async void CargarArtistas()
		{
			if (CargarArtistasLibre)
			{
				CargarArtistasLibre = false;
				ArtistaDAO artistaDAO = new ArtistaDAO(UsuarioActual.Token);
				List<Artista> respuesta;
				try
				{
					respuesta = await artistaDAO.CargarTodos();
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				if (respuesta.Count == 1 && respuesta[0].Id == 0)
				{
					ListaDeArtistas.Visibility = Visibility.Collapsed;
					LabelNoHayArtistas.Visibility = Visibility.Visible;
					ListaDeAlbumesDeArtista.Visibility = Visibility.Collapsed;
					LabelNoHayCancionesDeAlbum.Visibility = Visibility.Collapsed;
					ListaDeCancionesDeArtista.Visibility = Visibility.Collapsed;
					LabelNoHayCancionesDeAlbum.Visibility = Visibility.Collapsed;
				}
				else
				{

					ListaDeArtistas.AsignarArtistas(respuesta);
					ListaDeArtistas.Visibility = Visibility.Visible;
					LabelNoHayArtistas.Visibility = Visibility.Collapsed;
				}
				CargarArtistasLibre = true;
			}
		}

		private async void CargarGeneros()
		{
			if (CargarGenerosLibre)
			{
				CargarGenerosLibre = false;
				GeneroDAO generoDAO = new GeneroDAO(UsuarioActual.Token);
				List<Genero> respuesta;
				try
				{
					respuesta = await generoDAO.CargarTodos();
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				if (respuesta.Count == 1 && respuesta[0].Id == 0)
				{
					ListaDeGeneros.Visibility = Visibility.Collapsed;
					LabelNoHayGeneros.Visibility = Visibility.Visible;
				}
				else
				{
					ListaDeGeneros.AsignarGeneros(respuesta);
					ListaDeGeneros.Visibility = Visibility.Visible;
					LabelNoHayGeneros.Visibility = Visibility.Collapsed;
				}

				CargarGenerosLibre = true;
			}
		}

		private async void CargarPlaylists()
		{
			if (CargarPlaylistsLibre)
			{
				CargarPlaylistsLibre = false;
				PlaylistDAO playlistDAO = new PlaylistDAO(UsuarioActual.Token);
				try
				{
					PlaylistsDeUsuarioActual = await playlistDAO.CargarPorIdConsumidor(UsuarioActual.Id);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				if (PlaylistsDeUsuarioActual.Count == 1 && PlaylistsDeUsuarioActual[0].Id == 0)
				{
					ListaDePlaylists.Visibility = Visibility.Collapsed;
					LabelNoHayPlaylists.Visibility = Visibility.Visible;
					ListaDeCancionesDePlaylist.Visibility = Visibility.Collapsed;
					LabelNoHayCancionesDescargadas.Visibility = Visibility.Collapsed;
				}
				else
				{
					ListaDePlaylists.AsignarPlaylists(PlaylistsDeUsuarioActual);
					PropagarPlaylists();
					ListaDePlaylists.Visibility = Visibility.Visible;
					LabelNoHayPlaylists.Visibility = Visibility.Collapsed;
				}

				CargarPlaylistsLibre = true;
			}
		}

		private async void Inicializar()
		{
			Reproductor.AsignarControlador(ControladorDeReproduccion);
			ControladorDeReproduccion.AsignarInterfaz(Reproductor);
			LabelNombreDeUsuario.Content = UsuarioActual.CorreoElectronico;
			LabelNombreDeUsuario.Visibility = Visibility.Visible;
			ServiciosDeIO.AsignarIdUsuarioActual(UsuarioActual.Id.ToString());
			AsignarDisparadores();
			PropagarTokens();
			PropagarControladorDeReproduccion();
			await CargarCanciones();
			CargarPlaylists();
			ControladorDeReproduccion.AsignarCanciones(ListaDeCanciones.Canciones);
			ControladorDeReproduccion.Pausar();
		}

		private void AsignarDisparadores()
		{
			ListaDeArtistas.DataGridArtistas.SelectionChanged += DataGridArtistas_SelectionChanged;
			ListaDeAlbumesDeArtista.DataGridAlbumes.SelectionChanged += DataGridAlbumesDeArtista_SelectionChanged;
			ListaDeGeneros.DataGridGeneros.SelectionChanged += DataGridGeneros_SelectionChanged;
			ListaDePlaylists.DataGridPlaylists.SelectionChanged += DataGridPlaylists_SelectionChanged;
		}

		private void PropagarTokens()
		{
			string tokenActual = UsuarioActual.Token;
			ListaDeCanciones.AsignarToken(tokenActual);
			ListaDeCancionesDeAlbum.AsignarToken(tokenActual);
			ListaDeCancionesDeArtista.AsignarToken(tokenActual);
			ListaDeCancionesDescargadas.AsignarToken(tokenActual);
			ListaDeCancionesDePlaylist.AsignarToken(tokenActual);
			ListaDeCancionesPrivadas.AsignarToken(tokenActual);
			ListaDeAlbumesDeArtista.AsignarToken(tokenActual);
			ListaDePlaylists.AsignarToken(tokenActual);
			Reproductor.AsignarToken(tokenActual);
			ControladorDeReproduccion.AsignarToken(tokenActual);
		}

		private void PropagarPlaylists()
		{
			ListaDeCanciones.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
			ListaDeCancionesDeAlbum.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
			ListaDeCancionesDeArtista.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
			ListaDeCancionesDePlaylist.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
			ListaDeCancionesDescargadas.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
			ListaDeCancionesPrivadas.AsignarPlaylistsEnMenuDeContexto(PlaylistsDeUsuarioActual);
		}

		private void PropagarControladorDeReproduccion()
		{
			ListaDeCanciones.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDeAlbum.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDeArtista.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDePlaylist.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDescargadas.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeAlbumesDeArtista.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDePlaylists.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesPrivadas.AsignarControladorDeReproduccion(ControladorDeReproduccion);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeCanciones.Buscar((sender as TextBox).Text);
		}

		private async void TabControlPaneles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TabControl controlDePestañas = sender as TabControl;
			if (controlDePestañas.SelectedIndex != IndiceDePestañaSeleccionada)
			{
				IndiceDePestañaSeleccionada = controlDePestañas.SelectedIndex;
				if (controlDePestañas.SelectedIndex == 0)
				{
					await CargarCanciones();
				}
				else if (controlDePestañas.SelectedIndex == 1)
				{
					CargarArtistas();
				}
				else if (controlDePestañas.SelectedIndex == 2)
				{
					CargarGeneros();
				}
				else if (controlDePestañas.SelectedIndex == 3)
				{
					CargarCancionesDescargadas();
				}
				else if (controlDePestañas.SelectedIndex == 4)
				{
					CargarPlaylists();
				}
				else if (controlDePestañas.SelectedIndex == 5)
				{
					CargarPrivadas();
				}
			}
		}

		private async void CargarPrivadas()
		{
			if (CargarPrivadasLibre)
			{
				CargarPrivadasLibre = false;
				CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
				List<Cancion> respuesta;
				try
				{
					respuesta = await cancionDAO.CargarPrivadasPorIdConsumidor(UsuarioActual.Id);
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
					return;
				}
				if (respuesta.Count == 1 && respuesta[0].Id == 0)
				{
					ListaDeCancionesPrivadas.Visibility = Visibility.Collapsed;
					LabelNoHayBiblioteca.Visibility = Visibility.Visible;
				}
				else
				{
					ListaDeCancionesPrivadas.AsignarCanciones(respuesta);
					ListaDeCancionesPrivadas.Visibility = Visibility.Visible;
					LabelNoHayBiblioteca.Visibility = Visibility.Collapsed;
				}
				
				CargarPrivadasLibre = true;
			}
		}

		#region Eventos
		private void DataGridPlaylists_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDePlaylist(ListaDePlaylists.PlaylistSeleccionada);
		}

		private async void MostrarCancionesDePlaylist(Playlist playlist)
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			List<Cancion> respuesta;
			try
			{
				respuesta = await cancionDAO.CargarPorIdPlaylist(playlist.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			if (respuesta.Count == 1 && respuesta[0].Id == 0)
			{
				ListaDeCancionesDePlaylist.Visibility = Visibility.Collapsed;
				LabelNoHayCancionesEnPlaylist.Visibility = Visibility.Visible;
			}
			else
			{
				ListaDeCancionesDePlaylist.Visibility = Visibility.Visible;
				LabelNoHayCancionesEnPlaylist.Visibility = Visibility.Collapsed;
				ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
				await CargarArtistasDeCanciones(respuesta);
				ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
				await CargarAlbumDeCanciones(respuesta);
				ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
			}

		}

		private void DataGridGeneros_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDeGenero(ListaDeGeneros.GeneroSeleccionado);
			LabelNombreDeGenero.Content = ListaDeGeneros.GeneroSeleccionado.Nombre;
			LabelDescripcionDeGenero.Content = ListaDeGeneros.GeneroSeleccionado.Descripcion;
		}

		private async void MostrarCancionesDeGenero(Genero genero)
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			List<Cancion> respuesta;
			try
			{
				respuesta = await cancionDAO.CargarPorIdGenero(genero.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			if (respuesta.Count == 1 && respuesta[0].Id == 0)
			{
				ListaDeCancionesDeAlbum.Visibility = Visibility.Collapsed;
				LabelNoHayCancionesDeGenero.Visibility = Visibility.Visible;
			}
			else
			{
				ListaDeCancionesDeAlbum.Visibility = Visibility.Visible;
				LabelNoHayCancionesDeGenero.Visibility = Visibility.Collapsed;
				ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
				await CargarArtistasDeCanciones(respuesta);
				ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
				await CargarAlbumDeCanciones(respuesta);
				ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
			}
		}

		private async void CargarCancionesDescargadas()
		{
			if (CargarCancionesDescargadasLibre)
			{
				CargarCancionesDescargadasLibre = false;
				List<int> idsCancionesDescargadas = ServiciosDeIO.ListarCancionesDescargadas();
				List<Cancion> cancionesDescargadas = new List<Cancion>();
				foreach (int idCancion in idsCancionesDescargadas)
				{
					if (ModoConectado)
					{
						CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
						try
						{
							cancionesDescargadas.Add(await cancionDAO.CargarPorId(idCancion));
						}
						catch (Exception ex)
						{
							MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
							MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
							return;
						}
					}
					else
					{
						cancionesDescargadas.Add(ServiciosDeIO.ObtenerCancionLocal(idCancion));
					}
				}
				ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
				if (ModoConectado)
				{
					if (cancionesDescargadas.Count == 1 && cancionesDescargadas[0].Id == 0)
					{
						ListaDeCancionesDescargadas.Visibility = Visibility.Collapsed;
						LabelNoHayCancionesDescargadas.Visibility = Visibility.Visible;
					}
					else
					{
						ListaDeCancionesDescargadas.Visibility = Visibility.Visible;
						LabelNoHayCancionesDescargadas.Visibility = Visibility.Collapsed;
						ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
						await CargarArtistasDeCanciones(cancionesDescargadas);
						ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
						await CargarAlbumDeCanciones(cancionesDescargadas);
						ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
					}
				}
				CargarCancionesDescargadasLibre = true;
			}
		}

		private void DataGridAlbumesDeArtista_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDeAlbumDeArtista(ListaDeAlbumesDeArtista.AlbumSeleccionado);
		}

		private async void MostrarCancionesDeAlbumDeArtista(Album album)
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			List<Cancion> respuesta;
			try
			{
				respuesta = await cancionDAO.CargarPorIdAlbum(album.Id);

			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
			if (respuesta.Count == 1 && respuesta[0].Id == 0)
			{
				ListaDeCancionesDeArtista.Visibility = Visibility.Collapsed;
				LabelNoHayCancionesDeAlbum.Visibility = Visibility.Visible;
			}
			else
			{
				ListaDeCancionesDeArtista.Visibility = Visibility.Visible;
				LabelNoHayCancionesDeAlbum.Visibility = Visibility.Collapsed;
				ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
				await CargarArtistasDeCanciones(respuesta);
				ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
				await CargarAlbumDeCanciones(respuesta);
				ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
			}
			ImageCaratulaDeAlbum.Source = CargarImagen(await ServiciosDeIO.CargarCaratulaDeAlbumPorId(album.Id, UsuarioActual.Token));
		}

		private static BitmapImage CargarImagen(byte[] bytesDeImagen)
		{
			if (bytesDeImagen == null || bytesDeImagen.Length == 0) return null;
			BitmapImage imagen = new BitmapImage();
			using (MemoryStream stream = new MemoryStream(bytesDeImagen))
			{
				stream.Position = 0;
				imagen.BeginInit();
				imagen.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				imagen.CacheOption = BitmapCacheOption.OnLoad;
				imagen.UriSource = null;
				imagen.StreamSource = stream;
				imagen.EndInit();
			}
			imagen.Freeze();
			return imagen;
		}

		private void DataGridArtistas_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarAlbumesDeArtista(ListaDeArtistas.ArtistaSeleccionado);
			LabelNombreDeArtista.Content = ListaDeArtistas.ArtistaSeleccionado.Nombre;
			LabelDescripcionDeArtista.Content = ListaDeArtistas.ArtistaSeleccionado.Descripcion;
		}

		private async void MostrarAlbumesDeArtista(Artista artista)
		{
			AlbumDAO albumDAO = new AlbumDAO(UsuarioActual.Token);
			List<Album> respuesta;
			try
			{
				respuesta = await albumDAO.CargarPorIdArtista(artista.Id);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}

			if (respuesta.Count == 1 && respuesta[0].Id == 0)
			{
				ListaDeAlbumesDeArtista.Visibility = Visibility.Collapsed;
				LabelNoHayAlbumesDeArtista.Visibility = Visibility.Visible;
				ListaDeCancionesDeArtista.Visibility = Visibility.Collapsed;
				LabelNoHayCancionesDeAlbum.Visibility = Visibility.Collapsed;
			}
			else
			{
				ListaDeAlbumesDeArtista.Visibility = Visibility.Visible;
				LabelNoHayAlbumesDeArtista.Visibility = Visibility.Collapsed;
				ListaDeAlbumesDeArtista.AsignarAlbumes(respuesta);
			}
		}

		#endregion Eventos

		private void ButtonNuevaPlaylist_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			RegistroDePlaylist registroDePlaylist = new RegistroDePlaylist(UsuarioActual);
			registroDePlaylist.ShowDialog();
			CargarPlaylists();
		}

		private void TextBoxBusquedaBiblioteca_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeCancionesPrivadas.Buscar((sender as TextBox).Text);
		}

		private void ButtonAgregarCancion_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			RegistroDeCancion registroDeCancion = new RegistroDeCancion(UsuarioActual, TipoDeUsuario.Consumidor);
			registroDeCancion.ShowDialog();
			CargarPrivadas();
		}

		private void TextBoxBusquedaArtista_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeArtistas.Buscar((sender as TextBox).Text);
			ListaDeAlbumesDeArtista.Buscar((sender as TextBox).Text);
			ListaDeCancionesDeArtista.Buscar((sender as TextBox).Text);
		}

		private void TextBoxBusquedaAlbum_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeGeneros.Buscar((sender as TextBox).Text);
			ListaDeCancionesDeAlbum.Buscar((sender as TextBox).Text);
		}

		private void TextBoxBusquedaDescargas_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeCancionesDescargadas.Buscar((sender as TextBox).Text);
		}

		private void TextBoxBusquedaPlaylists_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDePlaylists.Buscar((sender as TextBox).Text);
			ListaDeCancionesDePlaylist.Buscar((sender as TextBox).Text);
		}

		private void ButtonConfiguracion_Click(object sender, RoutedEventArgs e)
		{
			Configuracion configuracion = new Configuracion();
			configuracion.Show();
		}
	}
}
