﻿using Logica.Clases;
using Logica.ClasesDeComunicacion;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Logica
{
	public class ControladorDeReproduccion
	{
		public List<Cancion> CancionesEnCola { get; set; } = new List<Cancion>();
		public int CancionActual { get; set; } = 0;
		//public WaveOutEvent Reproductor { get; set; } = new WaveOutEvent();
		//public Mp3FileReader Lector { get; set; }
		private IReproductor IReproductor { get; set; }
		private float Volumen { get; set; } = 0.1f;
		private string Token { get; set; }

		public void AsignarToken(string token)
		{
			Token = token;
		}

		public void Reproducir()
		{
			//if (Reproductor.PlaybackState == PlaybackState.Paused)
			//{
			//	Reproductor.Play();
			//}
			//else if (Reproductor.PlaybackState == PlaybackState.Stopped)
			//{
			//	InicializarReproduccion();
			//}
		}

		public void SaltarA(int indiceDeCancion)
		{
			if (indiceDeCancion <= CancionesEnCola.Count - 1)
			{
				CancionActual = indiceDeCancion;
				//Reproductor.Stop();
				InicializarReproduccion();
			}
		}

		public void AsignarInterfaz(IReproductor reproductor)
		{
			IReproductor = reproductor;
		}

		public void ReiniciarVistaPrevia()
		{
			//Reproductor.Stop();
		}

		public void ReproducirVistaPrevia(string direccionDeCancion)
		{
			//if (Reproductor.PlaybackState == PlaybackState.Stopped)
			//{
			//	Lector = new Mp3FileReader(direccionDeCancion);
			//	Reproductor.DeviceNumber = 0;
			//	Reproductor.Init(Lector);
			//	Reproductor.Play();
			//	Reproductor.Volume = Volumen;
			//}
			//else if (Reproductor.PlaybackState == PlaybackState.Playing)
			//{
			//	Reproductor.Pause();
			//}
			//else if (Reproductor.PlaybackState == PlaybackState.Paused)
			//{
			//	Reproductor.Play();

			//}
		}


		private void InicializarReproduccion()
		{
			Servicios.ServiciosDeDescarga serviciosDeDescarga = new Servicios.ServiciosDeDescarga();
			serviciosDeDescarga.EliminarArchivosTemporales();
			if (CancionesEnCola.Count > CancionActual)
			{
				CancionesEnCola[CancionActual].CargarDireccionDeCancion();
				if (!CancionesEnCola[CancionActual].CancionEstaDescargada())
				{
					serviciosDeDescarga.DescargarAudioTemporalDeCancion(CancionesEnCola[CancionActual].Id, Token);
					
				}
				//Lector = new Mp3FileReader(CancionesEnCola[CancionActual].DireccionDeCancion);
				//Reproductor.DeviceNumber = 0;
				//Reproductor.Init(Lector);
				//Reproductor.Play();
				//Reproductor.Volume = Volumen;
				//IReproductor.CargarDatosDeCancionActual();
			}
		}

		public bool EstaReproduciendo()
		{
			bool resultado = false;

			//if (Reproductor.PlaybackState == PlaybackState.Playing)
			//{
			//	resultado = true;
			//}

			return resultado;
		}

		public void AsignarCanciones(List<Cancion> canciones)
		{
			//Reproductor.Stop();
			CancionesEnCola = canciones;
			CancionActual = 0;
			InicializarReproduccion();
		}

		public void PausarOReproducir()
		{
			//if (Reproductor.PlaybackState == PlaybackState.Playing)
			//{
			//	Pausar();
			//}
			//else
			//{
			//	Reproducir();
			//}
		}

		public void Pausar()
		{
			//if (Reproductor.PlaybackState == PlaybackState.Playing)
			//{
			//	Reproductor.Pause();
			//}
		}

		public void Siguiente()
		{
			CancionActual++;
			if (CancionActual > CancionesEnCola.Count - 1)
			{
				CancionActual = 0;
			}
			//Reproductor.Stop();
			InicializarReproduccion();
		}

		public void Anterior()
		{

			CancionActual--;
			if (CancionActual < 0)
			{
				CancionActual = CancionesEnCola.Count - 1;
			}
			//Reproductor.Stop();
			InicializarReproduccion();

		}

		public TimeSpan ObtenerTiempoActual()
		{
			//return Reproductor.GetPositionTimeSpan();
			return new TimeSpan();
		}

		public void Buscar(int posicion)
		{
			//Lector.Seek(Lector.WaveFormat.AverageBytesPerSecond * posicion, System.IO.SeekOrigin.Begin);
		}

		public void CambiarVolumen(float volumen)
		{
			Volumen = volumen;
			//Reproductor.Volume = Volumen;
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
