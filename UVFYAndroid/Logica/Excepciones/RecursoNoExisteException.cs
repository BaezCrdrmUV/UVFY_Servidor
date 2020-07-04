using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Excepciones
{
    public class RecursoNoExisteException : Exception
    {

        public RecursoNoExisteException()
        {
        }

        public RecursoNoExisteException(string mensaje)
            : base(mensaje)
        {
        }

        public RecursoNoExisteException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
