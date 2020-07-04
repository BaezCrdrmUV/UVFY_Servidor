using System.Text.RegularExpressions;

namespace Logica.Servicios
{
    public static class ServiciosDeValidacion
    {
        /// <summary>
        /// Expresión regular que valida que la cadena tenga al menos una letra seguida
        /// de un arroba, al menos otras dos letras, un punto y al menos otras dos letras.
        /// </summary>
        private static readonly Regex RegexCorreoElectronico = new Regex(@"^(\D)+(\w)*((\.(\w)+)?)+@(\D)+(\w)*((\.(\D)+(\w)*)+)?(\.)[a-z]{2,}$");
        /// <summary>
        /// Expresión regular que valida que la cadena sean solo letras y letras modificadas.
        /// </summary>
        private static readonly Regex RegexNombre = new Regex(@"^[a-zA-Z àáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$");
        /// <summary>
        /// Expresión regular que valida que la cadena no tenga espacios en blanco y sea de 6 a 255 de longitud.
        /// </summary>
        private static readonly Regex RegexContraseña = new Regex(@"^\S{6,255}$");

        private static readonly Regex RegexNumeroEntero = new Regex(@"^\d+$");

        public const int TAMAÑO_MAXIMO_VARCHAR = 255;
        public const int VALOR_ENTERO_MINIMO_PERMITIDO = 0;
        private const int VALOR_ENTERO_MAXIMO_PERMITIDO = 255;

        /// <summary>
        /// Valida la estructura de la cadena del correo del usuario.
        /// </summary>
        /// <param name="correoElectronico">Correo del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool ValidarCorreoElectronico(string correoElectronico)
        {
            bool resultadoDeValidacion = false;

            if (correoElectronico.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                if (RegexCorreoElectronico.IsMatch(correoElectronico))
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarNumeroEntero(string numero)
        {
            bool resultadoDeValidacion = false;

            if (RegexNumeroEntero.IsMatch(numero))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena de la contraseña del usuario.
        /// </summary>
        /// <param name="contraseña">Contraseña del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool ValidarContraseña(string contraseña)
        {
            bool resultadoDeValidacion = false;

            if (RegexContraseña.IsMatch(contraseña))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        public static bool ValidarCadenaVacioPermitido(string cadena)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrWhiteSpace(cadena) && cadena.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida una cadena para la entrada a la base de datos.
        /// </summary>
        /// <param name="cadena">Cadena de carácteres.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool ValidarCadena(string cadena)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrEmpty(cadena) && cadena.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }
    }
}
