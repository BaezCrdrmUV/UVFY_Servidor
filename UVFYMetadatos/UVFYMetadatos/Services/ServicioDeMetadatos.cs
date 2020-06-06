using Grpc.Core;
using LogicaDeNegocios.Excepciones;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UVFYMetadatos.DAO;
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
					canciones = cancionDAO.CargarTodas();
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

			return base.CargarCancionesPorIdArtista(request, context);
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
					canciones = cancionDAO.CargarPodIdAlbum(request.IdPeticion);
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
				PlaylistDAO playlistDAO = new PlaylistDAO();
				bool resultadoDeAdicion;
				try
				{
					resultadoDeAdicion = playlistDAO.Registrar(request.Nombre, request.IdConsumidor);
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
						playlistDAO.AgregarCancion(request.IdPlaylist, request.IdCacnione);
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
						playlistDAO.EliminarCancion(request.IdPlaylist, request.IdCacnione);
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

