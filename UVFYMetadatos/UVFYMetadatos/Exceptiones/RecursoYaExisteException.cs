using System;
using System.Runtime.Serialization;

namespace UVFYMetadatos.DAO
{
	[Serializable]
	internal class RecursoYaExisteException : Exception
	{
		public RecursoYaExisteException()
		{
		}

		public RecursoYaExisteException(string message) : base(message)
		{
		}

		public RecursoYaExisteException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected RecursoYaExisteException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}