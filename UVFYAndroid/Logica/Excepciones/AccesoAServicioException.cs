using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Excepciones
{
    public class AccesoAServicioException : Exception
    {

        public AccesoAServicioException()
        {
        }

        public AccesoAServicioException(string mensaje)
            : base(mensaje)
        {
        }

        public AccesoAServicioException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
