using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UVFYMetadatos.Enumeradores
{
	public enum EstadoDeCancion
	{
		Indefinido = -1,
		Publica = 0,
		PrivadaDeConsumidor = 1,
		PrivadaDeArtista = 2,
		NoDisponible = 3
	}
}
