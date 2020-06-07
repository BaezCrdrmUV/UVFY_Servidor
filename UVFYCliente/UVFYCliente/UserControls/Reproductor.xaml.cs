using Logica;
using Logica.Clases;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using UVFYCliente.ConvertidoresDeDatos;

namespace UVFYCliente.UserControls
{
	/// <summary>
	/// Interaction logic for Reproductor.xaml
	/// </summary>
	public partial class Reproductor : UserControl
	{

		private ControladorDeReproduccion ControladorDeReproduccion { get; set; }
		private string Token { get; set; }
		DispatcherTimer Contador { get; set; }
		TimeSpan TiempoContado { get; set; }
		public Reproductor()
		{
			InitializeComponent();
			Contador = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
			{

				if (ControladorDeReproduccion.EstaReproduciendo())
				{
					TiempoContado = TiempoContado.Add(TimeSpan.FromSeconds(1));
					ProgressBarProgresoDeCancion.Value++;
					if(TiempoContado.Seconds < 10)
					{
						LabelTiempoTranscurrido.Content = TiempoContado.Minutes + ":0" + TiempoContado.Seconds;
					}
					else
					{
						LabelTiempoTranscurrido.Content = TiempoContado.Minutes + ":" + TiempoContado.Seconds;
					}
				}
			}, this.Dispatcher);
		}

		public void AsignarControlador(ControladorDeReproduccion controlador)
		{
			ControladorDeReproduccion = controlador;
		}

		public void AsignarToken (string token)
		{
			Token = token;
		}

		private void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.PausarOReproducir();
			CargarDatosDeCancionActual();
		}

		private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.Anterior();
			TiempoContado = TimeSpan.FromSeconds(0);
			LabelTiempoTranscurrido.Content = "0:00";
			CargarDatosDeCancionActual();
		}

		private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.Siguiente();
			TiempoContado = TimeSpan.FromSeconds(0);
			LabelTiempoTranscurrido.Content = "0:00";
			CargarDatosDeCancionActual();
		}

		private void CargarDatosDeCancionActual()
		{
			Cancion cancionActual = ControladorDeReproduccion.CancionesEnCola[ControladorDeReproduccion.CancionActual];
			LabelNombreDaCancionActual.Content = cancionActual.Nombre;
			LabelArtistaDeCancionActual.Content = cancionActual.Artista.Nombre;
			ProgressBarProgresoDeCancion.Maximum = cancionActual.Duracion;
			ProgressBarProgresoDeCancion.Value = 0;
			ConvertidorDeSegundosAMinutosYSegundos convertidorDeSegundosAMinutosYSegundos = new ConvertidorDeSegundosAMinutosYSegundos();
			LabelTiempoTotal.Content = convertidorDeSegundosAMinutosYSegundos.Convert((int)cancionActual.Duracion, typeof(string), null, null);
			AsignarImagenDeCancionActual(cancionActual.Id);
		}

		private async void AsignarImagenDeCancionActual(int idCancion)
		{
			byte[] imagen = await ServiciosDeIO.CargarCaratulaDeCancionPorId(idCancion, Token);
			ImageCaratulaDeAlbum.Source = CargarImagen(imagen);
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
	}
}
