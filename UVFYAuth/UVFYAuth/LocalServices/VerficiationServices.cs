using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UVFYAuth.LocalServices
{
    public static class VerificationServices
    {
        /// <summary>
        /// Expresión regular que valida que la cadena tenga al menos una letra seguida
        /// de un arroba, al menos otras dos letras, un punto y al menos otras dos letras.
        /// </summary>
        private static readonly Regex EmailRegex = new Regex(@"^(\D)+(\w)*((\.(\w)+)?)+@(\D)+(\w)*((\.(\D)+(\w)*)+)?(\.)[a-z]{2,}$");
        /// <summary>
        /// Expresión regular que valida que la cadena sean solo letras y letras modificadas.
        /// </summary>
        private static readonly Regex NameRegex = new Regex(@"^[a-zA-Z àáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð,.'-]+$");
        /// <summary>
        /// Expresión regular que valida que la cadena no tenga espacios en blanco y sea de 6 a 255 de longitud.
        /// </summary>
        private static readonly Regex PasswordRegex = new Regex(@"^\S{6,255}$");
        public const int TAMAÑO_MAXIMO_VARCHAR = 255;
        public const int VALOR_ENTERO_MINIMO_PERMITIDO = 0;
        private const int VALOR_ENTERO_MAXIMO_PERMITIDO = 255;

        /// <summary>
        /// Valida la estructura de la cadena del correo del usuario.
        /// </summary>
        /// <param name="email">Correo del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool VerifyEmail(string email)
        {
            bool resultadoDeValidacion = false;

            if (email.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                if (EmailRegex.IsMatch(email))
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena del nombre del usuario.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
		public static bool VerifyName(string name)
        {
            bool resultadoDeValidacion = false;

            if (name.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                if (NameRegex.IsMatch(name))
                {
                    resultadoDeValidacion = true;
                }
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida la estructura de la cadena de la contraseña del usuario.
        /// </summary>
        /// <param name="password">Contraseña del usuario.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool VerifyPassword(string password)
        {
            bool resultadoDeValidacion = false;

            if (PasswordRegex.IsMatch(password))
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida una cadena para la entrada a la base de datos.
        /// </summary>
        /// <param name="string">Cadena de carácteres.</param>
        /// <returns>Si la cadena cumple con la validación.</returns>
        public static bool VerifyString(string @string)
        {
            bool resultadoDeValidacion = false;

            if (!string.IsNullOrEmpty(@string) && @string.Length <= TAMAÑO_MAXIMO_VARCHAR)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }

        /// <summary>
        /// Valida si el <paramref name="email"/> esta disponible en la base de datos.
        /// </summary>
        /// <param name="email">Correo del usuario.</param>
        /// <returns>Si el correo esta disponible.</returns>
        public static bool VerifyPasswordAvailability(string email)
        {
            ////ServiciosDeValidacionDAO serviciosDeValidacionDAO = new ServiciosDeValidacionDAO();
            //bool resultadoDeValidacion = false;

            //if (serviciosDeValidacionDAO.ContarOcurrenciasDeCorreo(email) == 0)
            //{
            //    resultadoDeValidacion = true;
            //}

            //return resultadoDeValidacion;
            return true;
        }

        /// <summary>
        /// Valida si la cadena es convertible a entero y tiene la estructura para insertar a la base de datos.
        /// </summary>
        /// <param name="integer">Cadena con un numero.</param>
        /// <returns>Si la cadena es convertiblea entero.</returns>
        public static bool VerifyInteger(string integer)
        {
            bool resultadoDeValidacion = false;

            if (int.TryParse(integer, out int numeroConvertido) && numeroConvertido > VALOR_ENTERO_MINIMO_PERMITIDO && numeroConvertido <= VALOR_ENTERO_MAXIMO_PERMITIDO)
            {
                resultadoDeValidacion = true;
            }

            return resultadoDeValidacion;
        }
    }
}
