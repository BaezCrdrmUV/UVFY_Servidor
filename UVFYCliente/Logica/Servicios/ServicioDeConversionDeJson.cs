using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Servicios
{
	public static class ServicioDeConversionDeJson
	{
		public static T ConvertJsonToClass<T>(string jsonData)
		{
			var result = default(T);

			if (!string.IsNullOrEmpty(jsonData))
			{
				result = JsonConvert.DeserializeObject<T>(jsonData);
			}
			return result;
		}
	}
}
