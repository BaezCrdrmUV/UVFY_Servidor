using System;
using System.Runtime.Serialization;

namespace UVFYMetadatos.DAO
{
	[Serializable]
	internal class ValidacionFallidaException : Exception
	{
		public ValidacionFallidaException()
		{
		}

		public ValidacionFallidaException(string message) : base(message)
		{
		}

		public ValidacionFallidaException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ValidacionFallidaException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}