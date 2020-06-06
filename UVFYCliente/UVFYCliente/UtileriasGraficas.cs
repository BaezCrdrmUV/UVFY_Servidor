using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static Logica.Servicios.ServiciosDeValidacion;

namespace UVFYCliente
{
	public static class UtileriasGráficas
	{
		private static void MostrarToolTip(Control controlGrafico, string mensaje)
		{
			if (controlGrafico.ToolTip == null)
			{
				controlGrafico.ToolTip = new ToolTip()
				{
					Content = mensaje,
					Placement = System.Windows.Controls.Primitives.PlacementMode.Right,
				};
			}

			((ToolTip)controlGrafico.ToolTip).IsEnabled = true;
			ToolTipService.SetPlacementTarget((ToolTip)controlGrafico.ToolTip, controlGrafico);
			((ToolTip)controlGrafico.ToolTip).IsOpen = true;
		}

		private static void OcultarToolTip(Control controlGrafico)
		{
			if (controlGrafico.ToolTip != null)
			{
				((ToolTip)controlGrafico.ToolTip).IsOpen = false;
				((ToolTip)controlGrafico.ToolTip).IsEnabled = false;
				controlGrafico.ToolTip = null;
			}
		}

		public static void MostrarEstadoDeValidacionCadena(TextBox textBoxCadena)
		{
			if (ValidarCadena(textBoxCadena.Text))
			{
				textBoxCadena.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxCadena);
			}
			else
			{
				textBoxCadena.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxCadena, "Debe tener un largo de 0 a 255 caractéres y no puede estar vacio");
			}
		}

		public static void MostrarEstadoDeValidacionCadenaVacioPermitido(TextBox textBoxCadena)
		{
			if (ValidarCadenaVacioPermitido(textBoxCadena.Text))
			{
				textBoxCadena.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxCadena);
			}
			else
			{
				textBoxCadena.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxCadena, "Debe tener un largo de 0 a 255 caractéres");
			}
		}

		public static void MostrarEstadoDeValidacionContraseña(PasswordBox textBoxContraseña)
		{
			if (ValidarContraseña(textBoxContraseña.Password))
			{
				textBoxContraseña.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxContraseña);
			}
			else
			{
				textBoxContraseña.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxContraseña, "Debe tener un largo de 6 a 255 caractéres");
			}
		}


		public static void MostrarEstadoDeValidacionCorreoElectronico(TextBox textBoxCorreoElectronico)
		{
			if (ValidarCorreoElectronico(textBoxCorreoElectronico.Text))
			{
				textBoxCorreoElectronico.BorderBrush = Brushes.Green;
				OcultarToolTip(textBoxCorreoElectronico);
			}
			else
			{
				textBoxCorreoElectronico.BorderBrush = Brushes.Red;
				MostrarToolTip(textBoxCorreoElectronico, "El correo ingresado es invalido");
			}
		}

		public static string MostrarVentanaDeSeleccionDeArchivos()
		{

			string direccionDeArchivoSeleccionado = string.Empty;

			SaveFileDialog ventanaDeSeleccionDeArchivo = new SaveFileDialog
			{
				Filter = "DocumentosPDF (*.PDF)|*.PDF",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			};

			if (ventanaDeSeleccionDeArchivo.ShowDialog() == true)
			{
				direccionDeArchivoSeleccionado = ventanaDeSeleccionDeArchivo.FileName;
			}

			return direccionDeArchivoSeleccionado;
		}
	}
}
