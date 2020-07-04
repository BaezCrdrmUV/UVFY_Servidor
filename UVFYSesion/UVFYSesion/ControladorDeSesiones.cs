using System;
using System.Collections.Generic;
using System.Linq;

namespace UVFYSesion
{
	public sealed class ControladorDeSesiones
	{
		public static ControladorDeSesiones instancia = null;
		private static readonly object Candado = new object();
		List<Sesion> SesionesConectadas = new List<Sesion>();


		ControladorDeSesiones()
		{
		}

		public static ControladorDeSesiones Instancia
		{
			get
			{
				lock (Candado)
				{
					if (instancia == null)
					{
						instancia = new ControladorDeSesiones();
					}
					return instancia;
				}
			}
		}

		public string Agregar(int IdUsuario)
		{
			Sesion sesion;
			if (!UsuarioYaTieneSesion(IdUsuario))
			{
				sesion = new Sesion
				{
					Id = Guid.NewGuid().ToString(),
					IdUsuario = IdUsuario,
					HoraDeDestruccion = DateTime.Now.AddDays(2)
				};
				SesionesConectadas.Add(sesion);
			}
			else
			{
				sesion = ObtenerSesionPorIdUsuario(IdUsuario);
			}
			return sesion.Id;
		}

		public bool SesionExiste(string IdSesion)
		{
			bool resultado = false;

			if (SesionesConectadas.Any(s => s.Id == IdSesion))
			{
				if (SesionEsActual(IdSesion))
				{
					resultado = true;
				}
				else
				{
					EliminarSesion(IdSesion);
				}
			}

			return resultado;
		}

		private bool SesionEsActual(string IdSesion)
		{
			bool resultado = false;

			if (SesionesConectadas.FirstOrDefault(s => s.Id == IdSesion).HoraDeDestruccion > DateTime.Now)
			{
				resultado = true;
			}

			return resultado;
		}

		private void EliminarSesion(string IdSesion)
		{
			SesionesConectadas.Remove(SesionesConectadas.FirstOrDefault(s => s.Id == IdSesion));
		}

		public int ObtenerIdUsuarioPorIdSesion(string IdSesion)
		{
			Sesion sesionEncontrada;
			sesionEncontrada = SesionesConectadas.First(s => s.Id == IdSesion);
			int IdUsuarioEncontrado = 0;
			if (sesionEncontrada != null)
			{
				IdUsuarioEncontrado = sesionEncontrada.IdUsuario;
			}
			return IdUsuarioEncontrado;
		}

		public Sesion ObtenerSesionPorIdUsuario(int IdUsuario)
		{
			Sesion sesionEncontrada;
			sesionEncontrada = SesionesConectadas.FirstOrDefault(s => s.IdUsuario == IdUsuario);
			return sesionEncontrada;
		}

		public bool UsuarioYaTieneSesion(int IdUsuario)
		{
			bool resultado = false;

			if (SesionesConectadas.Any(s => s.IdUsuario == IdUsuario))
			{
				resultado = true;
			}

			return resultado;
		}
	}
}
