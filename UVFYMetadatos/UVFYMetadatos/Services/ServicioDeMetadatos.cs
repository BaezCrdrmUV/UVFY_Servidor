using Grpc.Core;
using LogicaDeNegocios.Excepciones;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYMetadatos.DAO;
using UVFYMetadatos.Enumeradores;
using UVFYMetadatos.Exceptiones;
using UVFYMetadatos.Models;
using UVFYMetadatos.Servicios;

namespace UVFYMetadatos
{
	public class ServicioDeMetadatos : Metadata.MetadataBase
	{
		private readonly ILogger<ServicioDeMetadatos> _logger;

		public ServicioDeMetadatos(ILogger<ServicioDeMetadatos> logger)
		{
			_logger = logger;

		}

		#region Canciones
		public override Task<RespuestaDeCanciones> CargarCancionesTodas(Token request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				try
				{
					canciones = cancionDAO.CargarPublicas();
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Canciones cancion in canciones)
				{
					ArtistaDAO artistaDAO = new ArtistaDAO();
					AlbumDAO albumDAO = new AlbumDAO();
					Albumes album = new Albumes();
					UsuariosArtista artista = new UsuariosArtista();
					try
					{
						album = albumDAO.CargarPorIdCancion(cancion.Id);
						artista = artistaDAO.CargarPorIdCancion(cancion.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						Album = new Album()
						{
							Id = album.Id
						},
						Artista = new Artista()
						{
							Id = artista.Id
						}
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionPorId(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()

			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException e)
			{
				Console.WriteLine("Error accediendo a servicio: " + e.Message);
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			catch (RecursoNoExisteException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 404;
				return Task.FromResult(respuesta);
			}

			if (existeSesion)
			{
				respuesta.Respuesta.Exitosa = true;
				CancionDAO cancionDAO = new CancionDAO();
				Canciones cancion;
				ArtistaDAO artistaDAO = new ArtistaDAO();
				AlbumDAO albumDAO = new AlbumDAO();
				Albumes album;
				UsuariosArtista artista;
				try
				{

					cancion = cancionDAO.CargarPorId(request.IdPeticion);
					album = albumDAO.CargarPorIdCancion(cancion.Id);
					artista = artistaDAO.CargarPorIdCancion(cancion.Id);
				}
				catch (AccesoADatosException e)
				{
					Console.WriteLine("Error accediendo a base de datos: " + e.Message);
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				respuesta.Canciones.Add(new Cancion()
				{
					Id = cancion.Id,
					Nombre = cancion.Nombre,
					Duracion = cancion.Duracion,
					FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
					Album = new Album()
					{
						Id = album.Id
					},
					Artista = new Artista()
					{
						Id = artista.Id
					},
				});
			}

			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesPorIdArtista(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				try
				{
					canciones = cancionDAO.CargarPorIdArtista(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Canciones cancion in canciones)
				{
					AlbumDAO albumDAO = new AlbumDAO();
					Albumes album = new Albumes();
					try
					{
						album = albumDAO.CargarPorIdCancion(cancion.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						Album = new Album()
						{
							Id = album.Id
						},
						Artista = new Artista()
						{
							Id = request.IdPeticion
						}
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesPorIdAlbum(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				try
				{
					canciones = cancionDAO.CargarPorIdAlbum(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Canciones cancion in canciones)
				{
					ArtistaDAO artistaDAO = new ArtistaDAO();
					UsuariosArtista artista = new UsuariosArtista();
					try
					{
						artista = artistaDAO.CargarPorIdCancion(cancion.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						Album = new Album()
						{
							Id = request.IdPeticion
						},
						Artista = new Artista()
						{
							Id = artista.Id
						}
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesPorIdPlaylist(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				try
				{
					canciones = cancionDAO.CargarPorIdPlaylist(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Canciones cancion in canciones)
				{
					ArtistaDAO artistaDAO = new ArtistaDAO();
					AlbumDAO albumDAO = new AlbumDAO();
					Albumes album = new Albumes();
					UsuariosArtista artista = new UsuariosArtista();
					try
					{
						album = albumDAO.CargarPorIdCancion(cancion.Id);
						artista = artistaDAO.CargarPorIdCancion(cancion.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						Album = new Album()
						{
							Id = album.Id
						},
						Artista = new Artista()
						{
							Id = artista.Id
						}
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesPrivadasPorIdArtista(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				bool tokenTienePermisos = true;
				try
				{
					canciones = cancionDAO.CargarCancionesPrivadasPorIdArtista(request.IdPeticion);
					if (canciones.Count > 0)
					{
						tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, canciones[0].ArtistaId.GetValueOrDefault());
					}
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{


					foreach (Canciones cancion in canciones)
					{
						Cancion cancionAEnviar = new Cancion()
						{
							Id = cancion.Id,
							Nombre = cancion.Nombre,
							Duracion = cancion.Duracion,
							FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
							Artista = new Artista()
							{
								Id = request.IdPeticion
							}
						};
						respuesta.Canciones.Add(cancionAEnviar);
					}
				}
				else
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 403;
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> CargarCancionesPorIdGenero(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Canciones> canciones = new List<Canciones>();
				CancionDAO cancionDAO = new CancionDAO();
				try
				{
					canciones = cancionDAO.CargarPorIdGenero(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Canciones cancion in canciones)
				{
					ArtistaDAO artistaDAO = new ArtistaDAO();
					AlbumDAO albumDAO = new AlbumDAO();
					Albumes album = new Albumes();
					UsuariosArtista artista = new UsuariosArtista();
					try
					{
						album = albumDAO.CargarPorIdCancion(cancion.Id);
						artista = artistaDAO.CargarPorIdCancion(cancion.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Cancion cancionAEnviar = new Cancion()
					{
						Id = cancion.Id,
						Nombre = cancion.Nombre,
						Duracion = cancion.Duracion,
						FechaDeLanzamiento = cancion.FechaDeLanzamiento.ToString(),
						Album = new Album()
						{
							Id = album.Id
						},
						Artista = new Artista()
						{
							Id = artista.Id
						}
					};
					respuesta.Canciones.Add(cancionAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> RegistrarCancionDeConsumidor(SolicitudDeRegistrarCancion request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			int idUsuario;
			bool usuarioEsConsumidor;
			int idCancionGuardada;
			bool guardadoDeArchivosExitoso = false;
			UsuarioDAO consumidorDAO = new UsuarioDAO();
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			if (existeSesion)
			{
				idUsuario = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
				usuarioEsConsumidor = consumidorDAO.IdEsDeConsumidor(idUsuario);
				if (usuarioEsConsumidor)
				{
					CancionDAO cancionDAO = new CancionDAO();
					Canciones canciones = new Canciones()
					{
						Nombre = request.Nombre
					};
					try
					{
						idCancionGuardada = cancionDAO.RegistrarCancionDeConsumidor(canciones);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}

					GuardadoDeImagenes cargaDeArchivos = new GuardadoDeImagenes();

					try
					{
						guardadoDeArchivosExitoso = cargaDeArchivos.GuardarAudioDeCancionDeConsumidor(idCancionGuardada, request.Audio.ToByteArray());
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (AccesoAServicioException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (ResultadoDeServicioFallidoException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}

			if (idCancionGuardada > 0)
			{
				respuesta.Canciones.Add(new Cancion
				{
					Id = idCancionGuardada
				});
				if (guardadoDeArchivosExitoso)
				{
					respuesta.Respuesta.Exitosa = true;
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCanciones> RegistrarCancionDeArtista(SolicitudDeRegistrarCancion request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			int idUsuario;
			bool usuarioEsArtista;
			int idCancionGuardada;
			bool guardadoDeArchivosExitoso = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			if (existeSesion)
			{
				UsuarioDAO usuarioDAO = new UsuarioDAO();
				idUsuario = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
				usuarioEsArtista = usuarioDAO.IdEsDeArtista(idUsuario);
				if (usuarioEsArtista)
				{
					CancionDAO cancionDAO = new CancionDAO();
					Canciones cancionARegistrar = new Canciones()
					{
						Nombre = request.Nombre,
						ArtistaId = idUsuario,
						Duracion = request.Duracion.ToString()
					};
					try
					{
						idCancionGuardada = cancionDAO.RegistrarCancionDeArtista(cancionARegistrar, request.Generos.ToList());
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					GuardadoDeAudio guardadoDeAudio = new GuardadoDeAudio();
					GuardadoDeImagenes guardadoDeImagenes = new GuardadoDeImagenes();
					try
					{
						if (guardadoDeAudio.GuardarAudioDeCancionDeArtista(idCancionGuardada, request.Audio.ToByteArray()))
						{

							if(guardadoDeImagenes.GuardarCaratulaDeCancion(idCancionGuardada, request.Imagen.ToByteArray()))
							{
								guardadoDeArchivosExitoso = true;
							}
						}
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (AccesoAServicioException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (ResultadoDeServicioFallidoException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}

			if (idCancionGuardada > 0)
			{
				respuesta.Canciones.Add(new Cancion
				{
					Id = idCancionGuardada
				});
				if (guardadoDeArchivosExitoso)
				{
					respuesta.Respuesta.Exitosa = true;
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> LiberarCancion(SolicitudDeCambiarEstadoDeCancion request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();

			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Canciones cancionCargada = new Canciones();
				CancionDAO cancionDAO = new CancionDAO();
				bool tokenTienePermisos;
				try
				{
					cancionCargada = cancionDAO.CargarPorId(request.IdCancion);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, cancionCargada.ArtistaId.GetValueOrDefault());
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						respuesta.Exitosa = cancionDAO.CambiarEstadoDeCancion(cancionCargada.Id, EstadoDeCancion.Publica);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> PrivatizarCancion(SolicitudDeCambiarEstadoDeCancion request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();

			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Canciones cancionCargada = new Canciones();
				CancionDAO cancionDAO = new CancionDAO();
				bool tokenTienePermisos;
				try
				{
					cancionCargada = cancionDAO.CargarPorId(request.IdCancion);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, cancionCargada.ArtistaId.GetValueOrDefault());
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						respuesta.Exitosa = cancionDAO.CambiarEstadoDeCancion(cancionCargada.Id, EstadoDeCancion.PrivadaDeArtista);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> EliminarCancion(SolicitudDeEliminarCancion request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();

			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion = false;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Canciones cancionCargada = new Canciones();
				CancionDAO cancionDAO = new CancionDAO();
				bool tokenTienePermisos;
				try
				{
					cancionCargada = cancionDAO.CargarPorId(request.IdCancion);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, cancionCargada.ArtistaId.GetValueOrDefault());
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						respuesta.Exitosa = cancionDAO.CambiarEstadoDeCancion(cancionCargada.Id, EstadoDeCancion.NoDisponible);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
			}

			return Task.FromResult(respuesta);
		}

		#endregion Canciones

		#region Artista
		public override Task<RespuestaDeArtista> CargarArtistasTodos(Token request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeArtista respuesta = new RespuestaDeArtista()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<UsuariosArtista> Artistas;
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					Artistas = artistaDAO.CargarTodos();
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (UsuariosArtista artista in Artistas)
				{
					Artista artistaAEnviar = new Artista()
					{
						Id = artista.Id,
						Nombre = artista.Nombre,
						Descripcion = artista.Descripcion
					};
					respuesta.Artista.Add(artistaAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeArtista> CargarArtistaPorId(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeArtista respuesta = new RespuestaDeArtista()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				UsuariosArtista artistaCargado;
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					artistaCargado = artistaDAO.CargarPorId(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Artista artistaAEnviar = new Artista()
				{
					Id = artistaCargado.Id,
					Nombre = artistaCargado.Nombre,
					Descripcion = artistaCargado.Descripcion
				};
				respuesta.Artista.Add(artistaAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeArtista> CargarArtistaPorIdCancion(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeArtista respuesta = new RespuestaDeArtista()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				UsuariosArtista artistaCargado;
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					artistaCargado = artistaDAO.CargarPorIdCancion(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Artista artistaAEnviar = new Artista()
				{
					Id = artistaCargado.Id,
					Nombre = artistaCargado.Nombre,
					Descripcion = artistaCargado.Descripcion
				};
				respuesta.Artista.Add(artistaAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeArtista> CargarArtistaPorIdAlbum(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeArtista respuesta = new RespuestaDeArtista()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				UsuariosArtista artistaCargado;
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					artistaCargado = artistaDAO.CargarPorIdAlbum(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Artista artistaAEnviar = new Artista()
				{
					Id = artistaCargado.Id,
					Nombre = artistaCargado.Nombre,
					Descripcion = artistaCargado.Descripcion
				};
				respuesta.Artista.Add(artistaAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}
		#endregion Artistas

		#region Album
		public override Task<RespuestaDeAlbum> CargarAlbumPorId(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeAlbum respuesta = new RespuestaDeAlbum()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Albumes albumCargado;
				AlbumDAO albumDAO = new AlbumDAO();
				GeneroDAO generoDAO = new GeneroDAO();
				ArtistaDAO artistaDAO = new ArtistaDAO();
				UsuariosArtista artistaCargado;
				List<Generos> generosCargados;
				try
				{
					artistaCargado = artistaDAO.CargarPorIdAlbum(request.IdPeticion);
					generosCargados = generoDAO.CargarPorIdAlbum(request.IdPeticion);
					albumCargado = albumDAO.CargarPorId(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}

				Album albumAEnviar = new Album()
				{
					Id = albumCargado.Id,
					Nombre = albumCargado.Nombre,
					Descripcion = albumCargado.Descripcion,
					Artista = new Artista
					{
						Id = artistaCargado.Id
					}
				};

				foreach (Generos genero in generosCargados)
				{
					albumAEnviar.Genero.Add(new Genero
					{
						Id = genero.Id
					});
				}
				respuesta.Album.Add(albumAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeAlbum> CargarAlbumPorIdCancion(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeAlbum respuesta = new RespuestaDeAlbum()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Albumes albumCargado;
				AlbumDAO albumDAO = new AlbumDAO();
				GeneroDAO generoDAO = new GeneroDAO();
				ArtistaDAO artistaDAO = new ArtistaDAO();
				UsuariosArtista artistaCargado;
				List<Generos> generosCargados;
				try
				{
					albumCargado = albumDAO.CargarPorIdCancion(request.IdPeticion);
					artistaCargado = artistaDAO.CargarPorIdAlbum(albumCargado.Id);
					generosCargados = generoDAO.CargarPorIdAlbum(albumCargado.Id);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Album albumAEnviar = new Album()
				{
					Id = albumCargado.Id,
					Nombre = albumCargado.Nombre,
					Descripcion = albumCargado.Descripcion,
					Artista = new Artista
					{
						Id = artistaCargado.Id
					}
				};

				foreach (Generos genero in generosCargados)
				{
					albumAEnviar.Genero.Add(new Genero
					{
						Id = genero.Id
					});
				}
				respuesta.Album.Add(albumAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeAlbum> CargarAlbumesPorIdArtista(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeAlbum respuesta = new RespuestaDeAlbum()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Albumes> albumesCargados;
				AlbumDAO albumDAO = new AlbumDAO();
				GeneroDAO generoDAO = new GeneroDAO();
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					albumesCargados = albumDAO.CargarPorIdArtista(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}

				foreach (Albumes albumes in albumesCargados)
				{
					UsuariosArtista artistaCargado;
					List<Generos> generosCargados;
					try
					{
						artistaCargado = artistaDAO.CargarPorIdAlbum(albumes.Id);
						generosCargados = generoDAO.CargarPorIdAlbum(albumes.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Album albumAEnviar = new Album()
					{
						Id = albumes.Id,
						Nombre = albumes.Nombre,
						Descripcion = albumes.Descripcion,
						Artista = new Artista
						{
							Id = artistaCargado.Id
						}
					};

					foreach (Generos genero in generosCargados)
					{
						albumAEnviar.Genero.Add(new Genero
						{
							Id = genero.Id
						});
					}
					respuesta.Album.Add(albumAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeAlbum> CargarAlbumesPorIdGenero(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeAlbum respuesta = new RespuestaDeAlbum()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Albumes> albumesCargados;
				AlbumDAO albumDAO = new AlbumDAO();
				GeneroDAO generoDAO = new GeneroDAO();
				ArtistaDAO artistaDAO = new ArtistaDAO();
				try
				{
					albumesCargados = albumDAO.CargarPorIdGenero(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}

				foreach (Albumes albumes in albumesCargados)
				{
					UsuariosArtista artistaCargado;
					List<Generos> generosCargados;
					try
					{
						artistaCargado = artistaDAO.CargarPorIdAlbum(albumes.Id);
						generosCargados = generoDAO.CargarPorIdAlbum(albumes.Id);
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					Album albumAEnviar = new Album()
					{
						Id = albumes.Id,
						Nombre = albumes.Nombre,
						Descripcion = albumes.Descripcion,
						Artista = new Artista
						{
							Id = artistaCargado.Id
						}
					};

					foreach (Generos genero in generosCargados)
					{
						albumAEnviar.Genero.Add(new Genero
						{
							Id = genero.Id
						});
					}
					respuesta.Album.Add(albumAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeAlbum> RegistrarAlbum(SolicitudDeRegistrarAlbum request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeAlbum respuesta = new RespuestaDeAlbum()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			int idUsuario;
			bool usuarioEsArtista;
			int idAlbumGuardado;
			UsuarioDAO consumidorDAO = new UsuarioDAO();
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				try
				{
					idUsuario = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
					usuarioEsArtista = consumidorDAO.IdEsDeArtista(idUsuario);
				}
				catch (AccesoAServicioException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				if (usuarioEsArtista)
				{
					AlbumDAO albumDAO = new AlbumDAO();
					GuardadoDeImagenes guardadoDeImagenes = new GuardadoDeImagenes();
					Albumes album = new Albumes()
					{
						Nombre = request.Nombre,
						Descripcion = request.Descripcion,
						ArtistasId = idUsuario
					};
					try
					{
						idAlbumGuardado = albumDAO.RegistrarAlbum(album, request.Generos.ToList());
						guardadoDeImagenes.GuardarCaratulaDeAlbum(idAlbumGuardado, request.Imagen.ToByteArray());
					}
					catch (AccesoADatosException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (ResultadoDeServicioFallidoException)
					{
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}

			if (idAlbumGuardado > 0)
			{
				respuesta.Album.Add(new Album
				{
					Id = idAlbumGuardado
				});
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> AgregarCancionAAlbum(SolicitudDeAgregarCancionAPlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			int idUsuario;
			bool usuarioEsArtista;
			UsuarioDAO consumidorDAO = new UsuarioDAO();
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				try
				{
					idUsuario = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
					usuarioEsArtista = consumidorDAO.IdEsDeArtista(idUsuario);
				}
				catch (AccesoAServicioException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}

				if (usuarioEsArtista)
				{
					Albumes albumCargado;
					AlbumDAO albumDAO = new AlbumDAO();
					bool tokenTienePermisos;
					try
					{
						albumCargado = albumDAO.CargarPorId(request.IdPlaylist);
						tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, albumCargado.ArtistasId);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					if (tokenTienePermisos)
					{
						CancionDAO cancionDAO = new CancionDAO();
						try
						{
							albumDAO.AgregarCancionAAlbum(request.IdCancion, request.IdPlaylist);
							cancionDAO.CambiarEstadoDeCancion(request.IdCancion, EstadoDeCancion.Publica);
						}
						catch (AccesoADatosException)
						{
							respuesta.Exitosa = false;
							respuesta.Motivo = 500;
							return Task.FromResult(respuesta);
						}
						catch (RecursoNoExisteException)
						{
							respuesta.Exitosa = false;
							respuesta.Motivo = 404;
							return Task.FromResult(respuesta);
						}
					}
					else
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 403;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> EliminarCancionDeAlbum(SolicitudDeEliminarCancionDePlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			int idUsuario;
			bool usuarioEsArtista;
			UsuarioDAO consumidorDAO = new UsuarioDAO();
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				try
				{
					idUsuario = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
					usuarioEsArtista = consumidorDAO.IdEsDeArtista(idUsuario);
				}
				catch (AccesoAServicioException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}

				if (usuarioEsArtista)
				{
					Albumes albumCargado;
					AlbumDAO albumDAO = new AlbumDAO();
					bool tokenTienePermisos;
					try
					{
						albumCargado = albumDAO.CargarPorId(request.IdPlaylist);
						tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, albumCargado.ArtistasId);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
					if (tokenTienePermisos)
					{
						CancionDAO cancionDAO = new CancionDAO();
						try
						{
							albumDAO.EliminarCancionDeAlbum(request.IdCancion, request.IdPlaylist);
							cancionDAO.CambiarEstadoDeCancion(request.IdCancion, EstadoDeCancion.PrivadaDeArtista);
						}
						catch (AccesoADatosException)
						{
							respuesta.Exitosa = false;
							respuesta.Motivo = 500;
							return Task.FromResult(respuesta);
						}
						catch (RecursoNoExisteException)
						{
							respuesta.Exitosa = false;
							respuesta.Motivo = 404;
							return Task.FromResult(respuesta);
						}
					}
					else
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 403;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> EliminarAlbum(SolicitudDeEliminarAlbum request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			int idConsumidor;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
				idConsumidor = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Albumes albumCargado;
				AlbumDAO albumDAO = new AlbumDAO();
				bool tokenTienePermisos;
				bool resultadoDeEliminacion = false;
				try
				{
					albumCargado = albumDAO.CargarPorId(request.IdAlbum);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, albumCargado.ArtistasId);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						resultadoDeEliminacion = albumDAO.Eliminar(request.IdAlbum);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}

				if (resultadoDeEliminacion)
				{
					respuesta.Exitosa = true;
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}
		#endregion Album

		#region Playlist
		public override Task<RespuestaDePlaylist> CargarPlaylistPorId(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDePlaylist respuesta = new RespuestaDePlaylist()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Playlists playlistCargada;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				try
				{
					playlistCargada = playlistDAO.CargarPorId(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Playlist playlistAEnviar = new Playlist()
				{
					Id = playlistCargada.Id,
					Nombre = playlistCargada.Nombre
				};
				respuesta.Playlists.Add(playlistAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDePlaylist> CargarPlaylistsPorIdUsuario(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDePlaylist respuesta = new RespuestaDePlaylist()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Playlists> playlistsCargadas;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				try
				{
					playlistsCargadas = playlistDAO.CargarPorIdUsuario(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Playlists playlists in playlistsCargadas)
				{
					Playlist playlistAEnviar = new Playlist()
					{
						Id = playlists.Id,
						Nombre = playlists.Nombre
					};
					respuesta.Playlists.Add(playlistAEnviar);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDePlaylist> RegistrarPlaylist(SolicitudDeAgregarPlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDePlaylist respuesta = new RespuestaDePlaylist()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			int idConsumidor;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
				idConsumidor = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool resultadoDeAdicion;
				try
				{
					resultadoDeAdicion = playlistDAO.Registrar(request.Nombre, idConsumidor);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}

				if (resultadoDeAdicion)
				{
					respuesta.Respuesta.Exitosa = true;
				}
				else
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 400;
				}
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> AgregarCancionAPlaylist(SolicitudDeAgregarCancionAPlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Playlists playlistCargada;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool tokenTienePermisos;
				try
				{
					playlistCargada = playlistDAO.CargarPorId(request.IdPlaylist);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, playlistCargada.ConsumidorId);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						playlistDAO.AgregarCancion(request.IdPlaylist, request.IdCancion);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> EliminarCancionDePlaylist(SolicitudDeEliminarCancionDePlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Playlists playlistCargada;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool tokenTienePermisos;
				try
				{
					playlistCargada = playlistDAO.CargarPorId(request.IdPlaylist);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, playlistCargada.ConsumidorId);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						playlistDAO.EliminarCancion(request.IdPlaylist, request.IdCancion);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> RenombrarPlaylist(SolicitudDeRenombrarPlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Playlists playlistCargada;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool tokenTienePermisos;
				try
				{
					playlistCargada = playlistDAO.CargarPorId(request.IdPlaylist);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, playlistCargada.ConsumidorId);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						playlistDAO.Renombrar(request.IdPlaylist, request.Nombre);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
			}
			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> EliminarPlaylist(SolicitudDeEliminarPlaylist request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			Respuesta respuesta = new Respuesta
			{
				Exitosa = false
			};
			bool existeSesion;
			int idConsumidor;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
				idConsumidor = validacionDeSesiones.ObtenerIdUsuarioPorToken(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Playlists playlistCargada;
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool tokenTienePermisos;
				bool resultadoDeEliminacion = false;
				try
				{
					playlistCargada = playlistDAO.CargarPorId(request.IdPlaylist);
					tokenTienePermisos = validacionDeSesiones.TokenTienePermisos(request.Token.TokenDeAcceso, playlistCargada.ConsumidorId);
				}
				catch (AccesoADatosException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				if (tokenTienePermisos)
				{
					try
					{
						resultadoDeEliminacion = playlistDAO.Eliminar(request.IdPlaylist);
					}
					catch (AccesoADatosException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 500;
						return Task.FromResult(respuesta);
					}
					catch (RecursoNoExisteException)
					{
						respuesta.Exitosa = false;
						respuesta.Motivo = 404;
						return Task.FromResult(respuesta);
					}
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 403;
					return Task.FromResult(respuesta);
				}

				if (resultadoDeEliminacion)
				{
					respuesta.Exitosa = true;
				}
				else
				{
					respuesta.Exitosa = false;
					respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
			}
			else
			{
				respuesta.Exitosa = false;
				respuesta.Motivo = 403;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}
		#endregion Playlist	

		#region Genero
		public override Task<RespuestaDeGenero> CargarGenerosTodos(Token request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeGenero respuesta = new RespuestaDeGenero()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				List<Generos> generosCargados;
				GeneroDAO generoDAO = new GeneroDAO();
				try
				{
					generosCargados = generoDAO.CargarTodos();
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				foreach (Generos generos in generosCargados)
				{
					Genero genero = new Genero()
					{
						Id = generos.Id,
						Nombre = generos.Nombre,
						Descripcion = generos.Descripcion
					};
					respuesta.Generos.Add(genero);
				}
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeGenero> CargarGeneroPorid(PeticionId request, ServerCallContext context)
		{
			ValidacionDeSesiones validacionDeSesiones = new ValidacionDeSesiones();
			RespuestaDeGenero respuesta = new RespuestaDeGenero()
			{
				Respuesta = new Respuesta
				{
					Exitosa = false
				}
			};
			bool existeSesion;
			try
			{
				existeSesion = validacionDeSesiones.ValidarSesion(request.Token.TokenDeAcceso);
			}
			catch (AccesoAServicioException)
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			respuesta.Respuesta.Exitosa = existeSesion;
			if (existeSesion)
			{
				Generos generoCargado;
				GeneroDAO generoDAO = new GeneroDAO();
				try
				{
					generoCargado = generoDAO.CargarPorId(request.IdPeticion);
				}
				catch (AccesoADatosException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
					return Task.FromResult(respuesta);
				}
				catch (RecursoNoExisteException)
				{
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 404;
					return Task.FromResult(respuesta);
				}
				Genero generoAEnviar = new Genero()
				{
					Id = generoCargado.Id,
					Nombre = generoCargado.Nombre,
					Descripcion = generoCargado.Descripcion,
				};
				respuesta.Generos.Add(generoAEnviar);
			}
			else
			{
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 403;
			}
			return Task.FromResult(respuesta);
		}
		#endregion Genero
	}
}

