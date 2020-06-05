using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicaDeNegocios.Excepciones
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