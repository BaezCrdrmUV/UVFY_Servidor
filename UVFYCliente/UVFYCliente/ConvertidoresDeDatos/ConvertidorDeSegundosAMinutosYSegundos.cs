using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UVFYCliente.ConvertidoresDeDatos
{
	class ConvertidorDeSegundosAMinutosYSegundos : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string resultado = string.Empty;
			int segundos = (int)value;
			TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
			if (tiempo.Seconds < 10)
			{
				resultado = tiempo.Minutes + ":0" + tiempo.Seconds;
			}
			else
			{
				resultado = tiempo.Minutes + ":" + tiempo.Seconds;
			}
			return resultado;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
