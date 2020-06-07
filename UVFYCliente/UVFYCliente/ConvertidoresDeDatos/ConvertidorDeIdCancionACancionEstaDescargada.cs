using Logica.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UVFYCliente.ConvertidoresDeDatos
{
	public class ConvertidorDeIdCancionACancionEstaDescargada : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool resultado;
			resultado = ServiciosDeIO.CancionEstaGuardada((int)value);
			return resultado;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
