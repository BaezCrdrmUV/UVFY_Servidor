using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UVFYMetadatos.Servicios
{
	public static class ServiciosDeValidacion
	{
        public const int TAMANO_MAXIMO_VARCHAR = 255;

        public static bool ValidarCadena(string cadena)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrEmpty(cadena) && cadena.Length <= TAMANO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }
    }
}
