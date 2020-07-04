using Logica;
using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
		public PantallaPrincipalDeConsumidor(Usuario usuario, IControladorDeCambioDePantalla controlador)
		{
			InitializeComponent();
			Controlador = controlador;
			UsuarioActual = usuario;
			Inicializar();
		}

		private async void CargarCanciones()
		{
			if (CargarCancionesLibre)
			{
				CargarCancionesLibre = false;
				CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
				var respuesta = await cancionDAO.CargarTodas();
				await CargarArtistasDeCanciones(respuesta);
				ListaDeCanciones.AsignarCanciones(respuesta);
				await CargarAlbumDeCanciones(respuesta);
				ListaDeCanciones.AsignarCanciones(respuesta);
				ControladorDeReproduccion.AsignarCanciones(respuesta);
				ControladorDeReproduccion.Pausar();
				CargarCancionesLibre = true;
			}
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
				ListaDeArtistas.AsignarArtistas(respuesta);
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
				ListaDeGeneros.AsignarGeneros(respuesta);
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
				ListaDePlaylists.AsignarPlaylists(PlaylistsDeUsuarioActual);
				PropagarPlaylists();
				CargarPlaylistsLibre = true;
			}
		}

		private void Inicializar()
		{
			Reproductor.AsignarControlador(ControladorDeReproduccion);
			ControladorDeReproduccion.AsignarInterfaz(Reproductor);
			ServiciosDeIO.AsignarIdUsuarioActual(UsuarioActual.Id.ToString());
			AsignarDisparadores();
			PropagarTokens();
			PropagarControladorDeReproduccion();
			CargarCanciones();
			CargarPlaylists();
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

		private void TabControlPaneles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			TabControl controlDePestañas = sender as TabControl;
			if (controlDePestañas.SelectedIndex == 0)
			{
				CargarCanciones();
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
				ListaDeCancionesPrivadas.AsignarCanciones(respuesta);
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
			ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
			await CargarArtistasDeCanciones(respuesta);
			ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
			await CargarAlbumDeCanciones(respuesta);
			ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
		}

		private void DataGridGeneros_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDeGenero(ListaDeGeneros.GeneroSeleccionado);
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
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
			await CargarArtistasDeCanciones(respuesta);
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
			await CargarAlbumDeCanciones(respuesta);
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
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
				ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
				await CargarArtistasDeCanciones(cancionesDescargadas);
				ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
				await CargarAlbumDeCanciones(cancionesDescargadas);
				ListaDeCancionesDescargadas.AsignarCanciones(cancionesDescargadas);
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
			ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
			await CargarArtistasDeCanciones(respuesta);
			ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
			await CargarAlbumDeCanciones(respuesta);
			ListaDeCancionesDeArtista.AsignarCanciones(respuesta);
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
			ListaDeAlbumesDeArtista.AsignarAlbumes(respuesta);
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
	}
}
