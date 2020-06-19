using System;
using System.Runtime.Serialization;

namespace UVFYGuardadoDeArchivos
{
	[Serializable]
	internal class ResultadoDeServicioFallidoException : Exception
	{
		public ResultadoDeServicioFallidoException()
		{
		}

		public ResultadoDeServicioFallidoException(string message) : base(message)
		{
		}

		public ResultadoDeServicioFallidoException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ResultadoDeServicioFallidoException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}