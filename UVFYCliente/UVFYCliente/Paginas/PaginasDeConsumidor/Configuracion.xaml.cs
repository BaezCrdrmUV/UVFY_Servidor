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
using System.Windows.Shapes;

namespace UVFYCliente.Paginas.PaginasDeConsumidor
{
	/// <summary>
	/// Interaction logic for Configuracion.xaml
	/// </summary>
	public partial class Configuracion : Window
	{
		public Configuracion()
		{
			InitializeComponent();
			
			ComboBoxCalidadDeDescarga.Items.Add("Baja");
			ComboBoxCalidadDeDescarga.Items.Add("Media");
			ComboBoxCalidadDeDescarga.Items.Add("Alta");
			int calidad;
			if (int.TryParse(ConfigurationManager.AppSettings["calidadDescarga"], out calidad))
			{
				ComboBoxCalidadDeDescarga.SelectedIndex = calidad;	
			}
			
			
		}

		private void ButtonAplicar_Click(object sender, RoutedEventArgs e)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["CalidadDescarga"].Value = ComboBoxCalidadDeDescarga.SelectedIndex.ToString();
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			MessageBox.Show("Configuracion aplicada", "¡Exito!");
			Close();
		}

		private void ButtonCancelar_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
