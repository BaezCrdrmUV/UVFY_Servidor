using Microsoft.Extensions.Localization.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYAuth.Models;
using UVFYAuth.LocalServices;
using UVFYAuth.DAOS;
using System.ComponentModel;

namespace UVFYAuth.LocalServices
{
	public static class AuthenticationServices
	{
		public static bool AuthenticateUserCredentials(string email, string password)
		{
			bool result = false;

			if(VerificationServices.VerifyEmail(email) && VerificationServices.VerifyPassword(password))
			{
				if (UsuarioDAO.ValidarExistenciaDeUsuarioCorreo(email))
				{
					if (UsuarioDAO.ValidarExistenciaDeCorreoYContraseña(email, ServiciosDeCifrado.EncriptarCadena(password)))
					{
						result = true;
					}
				}
			}

			return result;
		}
	}
}
