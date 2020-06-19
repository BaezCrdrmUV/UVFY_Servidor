using System;
using System.Runtime.Serialization;

namespace UVFYGuardadoDeArchivos
{
	[Serializable]
	internal class AccesoAServicioException : Exception
	{
		public AccesoAServicioException()
		{
		}

		public AccesoAServicioException(string message) : base(message)
		{
		}

		public AccesoAServicioException(string message, Exception innerException) : base(message, innerException)
		{
		}


		protected AccesoAServicioException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}