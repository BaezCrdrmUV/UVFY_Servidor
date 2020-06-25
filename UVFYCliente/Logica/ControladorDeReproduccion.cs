using Logica.Clases;
using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace Logica
{
	public class ControladorDeReproduccion
	{
		public List<Cancion> CancionesEnCola { get; set; } = new List<Cancion>();
		public int CancionActual { get; set; } = 0;
		public WaveOutEvent Reproductor { get; set; } = new WaveOutEvent();
		public Mp3FileReader Lector { get; set; }


		public void Reproducir()
		{
			if (Reproductor.PlaybackState == PlaybackState.Paused)
			{
				Reproductor.Play();
			}
			else if (Reproductor.PlaybackState == PlaybackState.Stopped)
			{
				InicializarReproduccion();
			}
		}

		public void ReiniciarVistaPrevia()
		{
			Reproductor.Stop();
		}

		public void ReproducirVistaPrevia(string direccionDeCancion)
		{
			if (Reproductor.PlaybackState == PlaybackState.Stopped)
			{
				Lector = new Mp3FileReader(direccionDeCancion);
				Reproductor.DeviceNumber = 0;
				Reproductor.Init(Lector);
				Reproductor.Play();
				Reproductor.Volume = 0.1f;
			}
			else if (Reproductor.PlaybackState == PlaybackState.Playing)
			{
				Reproductor.Pause();
			}
			else if (Reproductor.PlaybackState == PlaybackState.Paused)
			{
				Reproductor.Play();
			}
		}


		private void InicializarReproduccion()
		{
			if (CancionesEnCola.Count > CancionActual)
			{
				if (CancionesEnCola[CancionActual].CancionEstaDescargada())
				{
					CancionesEnCola[CancionActual].CargarDireccionDeCancion();
					Lector = new Mp3FileReader(CancionesEnCola[CancionActual].DireccionDeCancion);
					Reproductor.DeviceNumber = 0;
					Reproductor.Init(Lector);
					Reproductor.Play();
					Reproductor.Volume = 0.1f;
				}
			}
		}

		public bool EstaReproduciendo()
		{
			bool resultado = false;

			if (Reproductor.PlaybackState == PlaybackState.Playing)
			{
				resultado = true;
			}

			return resultado;
		}

		public void AsignarCanciones(List<Cancion> canciones)
		{
			Reproductor.Stop();
			CancionesEnCola = canciones;
			CancionActual = 0;
			InicializarReproduccion();
		}

		public void PausarOReproducir()
		{
			if (Reproductor.PlaybackState == PlaybackState.Playing)
			{
				Pausar();
			}
			else
			{
				Reproducir();
			}
		}

		public void Pausar()
		{
			if (Reproductor.PlaybackState == PlaybackState.Playing)
			{
				Reproductor.Pause();
			}
		}

		public void Siguiente()
		{
			if (Reproductor.PlaybackState != PlaybackState.Stopped)
			{
				CancionActual++;
				if (CancionActual > CancionesEnCola.Count - 1)
				{
					CancionActual = 0;
				}
				Reproductor.Stop();
				InicializarReproduccion();
			}
		}

		public void Anterior()
		{
			if (Reproductor.PlaybackState != PlaybackState.Stopped)
			{
				CancionActual--;
				if (CancionActual < 0)
				{
					CancionActual = CancionesEnCola.Count - 1;
				}
				Reproductor.Stop();
				InicializarReproduccion();
			}
		}

		public TimeSpan ObtenerTiempoActual()
		{
			return Reproductor.GetPositionTimeSpan();
		}

		public void Buscar(int posicion)
		{
			Lector.Seek(Lector.WaveFormat.AverageBytesPerSecond * posicion, System.IO.SeekOrigin.Begin);
		}

		public void CambiarVolumen(float volumen)
		{
			Reproductor.Volume = volumen;
		}

		public void AgregarCancionAlFinal(Cancion cancion)
		{
			CancionesEnCola.Add(cancion);
		}

		public void AgregarCancionAlPrincipio(Cancion cancion)
		{
			CancionesEnCola.Insert(CancionActual, cancion);
		}

	}
}
