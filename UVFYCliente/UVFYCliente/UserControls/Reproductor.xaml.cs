﻿using Logica;
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
	public partial class Reproductor : UserControl, IReproductor
	{

		private ControladorDeReproduccion ControladorDeReproduccion { get; set; }
		private string Token { get; set; }
		private DispatcherTimer Contador { get; set; }
		private TimeSpan TiempoContado { get; set; }
		private bool CancionCambiada { get; set; }
		private bool ModoConectado { get; set; } = true;
		public Reproductor()
		{
			CancionCambiada = true;
			InitializeComponent();
			Contador = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
			{
				AumentarTiempo();
				if(SliderProgresoDeCancion.Value >= SliderProgresoDeCancion.Maximum)
				{
					ControladorDeReproduccion.Siguiente();
					Contador.Stop();
				}
			}, this.Dispatcher);
		}

		private void AumentarTiempo()
		{
			if (ControladorDeReproduccion.EstaReproduciendo())
			{
				TiempoContado = TiempoContado.Add(TimeSpan.FromSeconds(1));
				SliderProgresoDeCancion.Value++;
				ActualizarTiempo();
			}
		}

		private void ActualizarTiempo()
		{
			if (TiempoContado.Seconds < 10)
			{
				LabelTiempoTranscurrido.Content = TiempoContado.Minutes + ":0" + TiempoContado.Seconds;
			}
			else
			{
				LabelTiempoTranscurrido.Content = TiempoContado.Minutes + ":" + TiempoContado.Seconds;
			}
		}

		public void AsignarControlador(ControladorDeReproduccion controlador)
		{
			ControladorDeReproduccion = controlador;
		}

		public void AsignarToken (string token)
		{
			Token = token;
		}

		public void AsignarModoConectado(bool modoConectado)
		{
			ModoConectado = modoConectado;
		}

		private void ButtonReproducir_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.PausarOReproducir();
			if (CancionCambiada)
			{
				CargarDatosDeCancionActual();
				CancionCambiada = false;
			}
		}

		private void ButtonAnterior_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.Anterior();
			TiempoContado = TimeSpan.FromSeconds(0);
			LabelTiempoTranscurrido.Content = "0:00";
		}

		private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
		{
			ControladorDeReproduccion.Siguiente();
			TiempoContado = TimeSpan.FromSeconds(0);
			LabelTiempoTranscurrido.Content = "0:00";
		}

		public void CargarDatosDeCancionActual()
		{
			if (ControladorDeReproduccion.CancionesEnCola.Count > 0)
			{
				Cancion cancionActual = ControladorDeReproduccion.CancionesEnCola[ControladorDeReproduccion.CancionActual];
				LabelNombreDaCancionActual.Content = cancionActual.Nombre;
				if (cancionActual.Artista != null)
				{
					LabelArtistaDeCancionActual.Content = cancionActual.Artista.Nombre;
					LabelArtistaDeCancionActual.Visibility = Visibility.Visible;
				}
				else
				{
					LabelArtistaDeCancionActual.Visibility = Visibility.Hidden;
				}
				SliderProgresoDeCancion.Maximum = cancionActual.Duracion;
				SliderProgresoDeCancion.Value = 0;
				ConvertidorDeSegundosAMinutosYSegundos convertidorDeSegundosAMinutosYSegundos = new ConvertidorDeSegundosAMinutosYSegundos();
				LabelTiempoTotal.Content = convertidorDeSegundosAMinutosYSegundos.Convert((int)cancionActual.Duracion, typeof(string), null, null);
				if (ModoConectado)
				{
					AsignarImagenDeCancionActual(cancionActual.Id);
				}
				else
				{
					AsignarImagenLocalDeCancionActual(cancionActual.Id);
				}
			}
		}

		private void AsignarImagenLocalDeCancionActual(int idCancion)
		{
			byte[] imagen;
			imagen = ServiciosDeIO.CargarCaratulaDeCancionPorId(idCancion);
			ImageCaratulaDeAlbum.Source = CargarImagen(imagen);
		}

		private async void AsignarImagenDeCancionActual(int idCancion)
		{
			byte[] imagen;
			try
			{
				imagen = await ServiciosDeIO.CargarCaratulaDeCancionPorId(idCancion, Token);
			}
			catch (Exception ex)
			{
				MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
				MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				return;
			}
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

		private void SliderProgresoDeCancion_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
		{
			Slider sliderDeProgreso = sender as Slider;
			ControladorDeReproduccion.Buscar((int)sliderDeProgreso.Value);
			TiempoContado = TimeSpan.FromSeconds(sliderDeProgreso.Value);
			ActualizarTiempo();
		}

		private void ProgressBarVolumen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Slider sliderDeProgreso = sender as Slider;
			ControladorDeReproduccion.CambiarVolumen((float)sliderDeProgreso.Value);
		}

		void IReproductor.CargarDatosDeCancionActual()
		{
			CargarDatosDeCancionActual();
		}

		private void ButtonColaDeReproduccion_Click(object sender, RoutedEventArgs e)
		{
			Paginas.PaginasDeConsumidor.ListaDeReproduccion listaDeReproduccion = new Paginas.PaginasDeConsumidor.ListaDeReproduccion(ControladorDeReproduccion, Token, ModoConectado);
			listaDeReproduccion.Show();
		}

		public void Bloquear()
		{
			ButtonAnterior.IsEnabled = false;
			ButtonSiguiente.IsEnabled = false;
			ButtonReproducir.IsEnabled = false;
			SliderProgresoDeCancion.IsEnabled = false;
		}

		public void Desbloquear()
		{
			ButtonAnterior.IsEnabled = true;
			ButtonSiguiente.IsEnabled = true;
			ButtonReproducir.IsEnabled = true;
			SliderProgresoDeCancion.IsEnabled = true;
			TiempoContado = TimeSpan.FromSeconds(0);
			LabelTiempoTranscurrido.Content = "0:00";
			Contador.Start();
		}
	}
}
