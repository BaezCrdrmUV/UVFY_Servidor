using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Excepciones
{
	public class ErrorInternoDeServicioException : Exception
    {
        public ErrorInternoDeServicioException()
        {
        }

        public ErrorInternoDeServicioException(string mensaje)
            : base(mensaje)
        {
        }

        public ErrorInternoDeServicioException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
