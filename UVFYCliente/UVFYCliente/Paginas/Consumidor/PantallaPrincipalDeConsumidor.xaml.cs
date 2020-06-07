using Logica;
using Logica.Clases;
using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace UVFYCliente.Paginas.Consumidor
{
	/// <summary>
	/// Interaction logic for PantallaPrincipalDeConsumidor.xaml
	/// </summary>
	public partial class PantallaPrincipalDeConsumidor : Page
	{
		private Usuario UsuarioActual { get; set; }
		private ControladorDeReproduccion ControladorDeReproduccion { get; set; } = new ControladorDeReproduccion();
		public PantallaPrincipalDeConsumidor(Usuario usuario)
		{
			InitializeComponent();
			UsuarioActual = usuario;
			CargarCanciones();
		}

		private async void CargarCanciones()
		{
			CancionDAO cancionDAO = new CancionDAO(UsuarioActual.Token);
			var respuesta = await cancionDAO.CargarTodas();
			ListaDeCanciones.AsignarCanciones(respuesta);
			ListaDeCanciones.AsignarToken(UsuarioActual.Token);
			ListaDeCanciones.AsignarControladorDeReproduccion(ControladorDeReproduccion);
			Reproductor.AsignarControlador(ControladorDeReproduccion);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			ListaDeCanciones.Buscar((sender as TextBox).Text);
		}
	}
}
