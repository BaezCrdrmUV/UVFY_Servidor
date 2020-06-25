using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVFYCliente
{
	public class MensajeDeErrorParaMessageBox
	{
		public string Titulo { get; }
		public string Mensaje { get; }

		public MensajeDeErrorParaMessageBox()
		{
			Titulo = "Error desconocido";
			Mensaje = "Ocurrion un error insesperado.";
		}

		public MensajeDeErrorParaMessageBox(string mensaje, string titulo)
		{
			Mensaje = mensaje;
			Titulo = titulo;
		}
	}
}
