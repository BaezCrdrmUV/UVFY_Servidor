using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UVFYMetadatos.Exceptiones
{
    public class ResultadoDeServicioFallidoException : Exception
    {

        public ResultadoDeServicioFallidoException()
        {
        }

        public ResultadoDeServicioFallidoException(string mensaje)
            : base(mensaje)
        {
        }

        public ResultadoDeServicioFallidoException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
