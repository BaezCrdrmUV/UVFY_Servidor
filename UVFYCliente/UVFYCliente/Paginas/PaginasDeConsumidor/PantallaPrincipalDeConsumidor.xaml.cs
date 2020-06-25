using Logica;
using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Controls;

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
		public PantallaPrincipalDeConsumidor(Usuario usuario, IControladorDeCambioDePantalla controlador)
		{
			InitializeComponent();
			Controlador = controlador;
			UsuarioActual = usuario;
			Inicializar();
		}

		private async void CargarCanciones()
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			var respuesta = await cancionDAO.CargarTodas();
			await CargarArtistasDeCanciones(respuesta);
			ListaDeCanciones.AsignarCanciones(respuesta);
			await CargarAlbumDeCanciones(respuesta);
			ListaDeCanciones.AsignarCanciones(respuesta);
		}

		private async Task<bool> CargarArtistasDeCanciones(List<Cancion> canciones)
		{
			ArtistaDAO artistaDAO = new ArtistaDAO(UsuarioActual.Token);
			foreach (Cancion cancion in canciones)
			{
				var respuesta = await artistaDAO.CargarPorId(cancion.Artista.Id);
				cancion.Artista = respuesta;
			}
			return true;
		}

		private async Task<bool> CargarAlbumDeCanciones(List<Cancion> canciones)
		{
			AlbumDAO albumDAO = new AlbumDAO(UsuarioActual.Token);
			foreach (Cancion cancion in canciones)
			{
				var respuesta = await albumDAO.CargarPorId(cancion.Album.Id);
				cancion.Album = respuesta;
			}
			return true;
		}

		private async void CargarArtistas()
		{
			ArtistaDAO artistaDAO = new ArtistaDAO(UsuarioActual.Token);
			var respuesta = await artistaDAO.CargarTodos();
			ListaDeArtistas.AsignarArtistas(respuesta);
		}

		private async void CargarGeneros()
		{
			GeneroDAO generoDAO = new GeneroDAO(UsuarioActual.Token);
			var respuesta = await generoDAO.CargarTodos();
			ListaDeGeneros.AsignarGeneros(respuesta);
		}

		private async void CargarPlaylists()
		{
			PlaylistDAO playlistDAO = new PlaylistDAO(UsuarioActual.Token);
			var respuesta = await playlistDAO.CargarPorIdConsumidor(UsuarioActual.Id);
			ListaDePlaylists.AsignarPlaylists(respuesta);
		}

		private void Inicializar()
		{
			AsignarDisparadores();
			PropagarTokens();
			PropagarControladorDeReproduccion();
			CargarCanciones();
			Reproductor.AsignarControlador(ControladorDeReproduccion);
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
		}

		private void PropagarControladorDeReproduccion()
		{
			ListaDeCanciones.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDeAlbum.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDeArtista.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			ListaDeCancionesDescargadas.AsignarControladorDeReproduccion(ControladorDeReproduccion);
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

			}
			else if (controlDePestañas.SelectedIndex == 4)
			{

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
			var respuesta = await cancionDAO.CargarPorIdPlaylist(playlist.Id);
			ListaDeCancionesDePlaylist.AsignarCanciones(respuesta);
		}

		private void DataGridGeneros_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDeGenero(ListaDeGeneros.GeneroSeleccionado);
		}

		private async void MostrarCancionesDeGenero(Genero genero)
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			var respuesta = await cancionDAO.CargarPorIdGenero(genero.Id);
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
			await CargarArtistasDeCanciones(respuesta);
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
			await CargarAlbumDeCanciones(respuesta);
			ListaDeCancionesDeAlbum.AsignarCanciones(respuesta);
		}

		private void DataGridAlbumesDeArtista_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MostrarCancionesDeAlbumDeArtista(ListaDeAlbumesDeArtista.AlbumSeleccionado);
		}

		private async void MostrarCancionesDeAlbumDeArtista(Album album)
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			var respuesta = await cancionDAO.CargarPorIdAlbum(album.Id);
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
			var respuesta = await albumDAO.CargarPorIdArtista(artista.Id);
			ListaDeAlbumesDeArtista.AsignarAlbumes(respuesta);
		}

		#endregion Eventos
	}
}
