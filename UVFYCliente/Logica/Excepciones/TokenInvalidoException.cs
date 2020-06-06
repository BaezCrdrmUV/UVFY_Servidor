using System;

namespace Logica.Excepciones
{
	public class TokenInvalidoException : Exception
    {
        public TokenInvalidoException()
        {
        }

        public TokenInvalidoException(string mensaje)
            : base(mensaje)
        {
        }

        public TokenInvalidoException(string mensaje, Exception excepcionInterna)
            : base(mensaje, excepcionInterna)
        {
        }
    }
}
