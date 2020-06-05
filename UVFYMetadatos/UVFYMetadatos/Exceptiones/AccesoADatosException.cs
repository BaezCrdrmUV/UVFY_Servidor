using System;

namespace LogicaDeNegocios.Excepciones
{
    public class AccesoADatosException : Exception
    {

        public AccesoADatosException()
        {
        }

        public AccesoADatosException(string mensaje)
            : base(mensaje)
        {
        }

        public AccesoADatosException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}