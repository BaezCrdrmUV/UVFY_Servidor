﻿using Logica;
using Logica.Clases;
using Logica.DAO;
using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static UVFYCliente.UtileriasGráficas;

namespace UVFYCliente.Paginas.PaginasDeArtista
{
	/// <summary>
	/// Interaction logic for RegistroDeCancion.xaml
	/// </summary>
	public partial class RegistroDeCancion : Window
	{
		private const string ARCHIVO_SELECCIONADO = "Archivo no seleccionado";

		private string DireccionDeArchivoDeAudio { get; set; }
		private string DireccionDeArchivoDeCaratula { get; set; }
		private List<int> GenerosSeleccionados { get; set; } = new List<int>();
		private List<Genero> GenerosCargados { get; set; } = new List<Genero>();
		private Usuario UsuarioActual { get; set; } = new Usuario();
		private TipoDeUsuario TipoDeUsuario { get; set; }
		private ControladorDeReproduccion ControladorDeVistaPrevia { get; set; } = new ControladorDeReproduccion();

		public RegistroDeCancion(Usuario usuario, TipoDeUsuario tipoDeUsuario)
		{
			InitializeComponent();
			ButtonVistaPreviaDeAudio.Visibility = Visibility.Hidden;
			ImagenVistaPreviaDeCaratula.Visibility = Visibility.Hidden;
			if(tipoDeUsuario == TipoDeUsuario.Consumidor)
			{
				DataGridGeneros.Visibility = Visibility.Collapsed;
				LabelGeneros.Visibility = Visibility.Collapsed; 
			}
			UsuarioActual = usuario;
			TipoDeUsuario = tipoDeUsuario;
			CargarGeneros();
		}

		private async void CargarGeneros()
		{
			GeneroDAO generoDAO = new GeneroDAO(UsuarioActual.Token);
			GenerosCargados = await generoDAO.CargarTodos();
			DataGridGeneros.ItemsSource = GenerosCargados;
		}

		private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonElegirArchivoDeAudio_Click(object sender, RoutedEventArgs e)
		{
			DireccionDeArchivoDeAudio = MostrarVentanaDeSeleccionDeArchivosDeAudio();

			if (!string.IsNullOrEmpty(DireccionDeArchivoDeAudio))
			{
				LabelDireccionDeArchivoDeAudio.Content = DireccionDeArchivoDeAudio;
				ButtonVistaPreviaDeAudio.Visibility = Visibility.Visible;
			}
			else
			{
				LabelDireccionDeArchivoDeAudio.Content = ARCHIVO_SELECCIONADO;
				ButtonVistaPreviaDeAudio.Visibility = Visibility.Hidden;
			}
		}

		private void ButtonVistaPreviaDeAudio_Click(object sender, RoutedEventArgs e)
		{
			if (ServiciosDeIO.ExisteArchivo(DireccionDeArchivoDeAudio))
			{
				ControladorDeVistaPrevia.ReproducirVistaPrevia(DireccionDeArchivoDeAudio);
			}
		}

		private void ButtonElegirArchivoDeCaratula_Click(object sender, RoutedEventArgs e)
		{
			DireccionDeArchivoDeCaratula = MostrarVentanaDeSeleccionDeArchivosDeCaratula();

			if (!string.IsNullOrEmpty(DireccionDeArchivoDeCaratula))
			{
				LabelDireccionDeArchivoDeCaratula.Content = DireccionDeArchivoDeCaratula;
				ImagenVistaPreviaDeCaratula.Source = new BitmapImage(new Uri(DireccionDeArchivoDeCaratula));
				ImagenVistaPreviaDeCaratula.Visibility = Visibility.Visible;
			}
			else
			{
				LabelDireccionDeArchivoDeCaratula.Content = ARCHIVO_SELECCIONADO;
				ImagenVistaPreviaDeCaratula.Visibility = Visibility.Hidden;
			}
		}

		private async void ButtonGuardar_Click(object sender, RoutedEventArgs e)
		{
			if (ValidarCampos())
			{

				byte[] datosDeAudio = ServiciosDeIO.CargarBytesDeArchivo(DireccionDeArchivoDeAudio);
				byte[] datosDeCaratula = ServiciosDeIO.CargarBytesDeArchivo(DireccionDeArchivoDeCaratula);
				int duracionDeAudio = ServiciosDeIO.ObtenerDuracionDeCancion(DireccionDeArchivoDeAudio);
				foreach (Genero item in DataGridGeneros.ItemsSource)
				{
					if (((CheckBox)CheckBoxColumn.GetCellContent(item)).IsChecked == true)
					{
						GenerosSeleccionados.Add(item.Id);
					}
				}
				CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
				try
				{
					if (TipoDeUsuario == TipoDeUsuario.Artista)
					{


						bool resultado = await cancionDAO.RegistrarCancionDeArtista(TextBoxNombreDeCancion.Text, GenerosSeleccionados, datosDeAudio, datosDeCaratula, duracionDeAudio);
					}
					else if (TipoDeUsuario == TipoDeUsuario.Consumidor)
					{
						bool resultado = await cancionDAO.RegistrarCancionDeConsumidor(TextBoxNombreDeCancion.Text, datosDeAudio, datosDeCaratula, duracionDeAudio);
					}
				}
				catch (Exception ex)
				{
					MensajeDeErrorParaMessageBox mensaje = EncadenadorDeExcepciones.ManejarExcepcion(ex);
					MessageBox.Show(mensaje.Mensaje, mensaje.Titulo);
				}

			}
			else
			{
				MessageBox.Show("Campos invalidos", "Error");
			}
		}

		private bool ValidarCampos()
		{
			bool respuesta = false;
			if (ServiciosDeIO.ExisteArchivo(DireccionDeArchivoDeAudio) && ServiciosDeIO.ExisteArchivo(DireccionDeArchivoDeCaratula))
			{
				if (ServiciosDeValidacion.ValidarCadena(TextBoxNombreDeCancion.Text))
				{
					respuesta = true;
				}
			}

			return respuesta;
		}
	}
}
