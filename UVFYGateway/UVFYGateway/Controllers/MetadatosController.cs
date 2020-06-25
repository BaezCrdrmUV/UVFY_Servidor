using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVFYMetadatos;

namespace UVFYGateway.Controllers
{
	[Route("Metadatos")]
	[ApiController]
	public class MetadatosController : ControllerBase
	{
		private GrpcChannel ServicioDeMetadatos;


		private readonly ILogger<MetadatosController> _logger;

		public MetadatosController(ILogger<MetadatosController> logger)
		{
			_logger = logger;
			GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions();
			grpcChannelOptions.Credentials = ChannelCredentials.Insecure;
			grpcChannelOptions.MaxReceiveMessageSize = 200000000;
			AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
			ServicioDeMetadatos = GrpcChannel.ForAddress("http://172.17.0.7:80", grpcChannelOptions);

		}
		#region Cancion
		[HttpGet]
		[Route("Canciones/Todas")]
		public IActionResult CancionesTodas([FromQuery] string tokenDeAcceso)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = tokenDeAcceso
			};
			RespuestaDeCanciones respuesta = new RespuestaDeCanciones();
			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesTodas(token);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorID")]
		public IActionResult CargarCancionPorId([FromQuery]string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorArtista")]
		public IActionResult CargarCancionesPorIdArtista([FromQuery]string tokenDeAcceso, int idArtista)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idArtista
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdArtista(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorAlbum")]
		public IActionResult CargarCancionesPorIdAlbum([FromQuery]string tokenDeAcceso, int idAlbum)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idAlbum
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdAlbum(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorPlaylist")]
		public IActionResult CargarCancionesPorIdPlaylist([FromQuery]string tokenDeAcceso, int idPlaylist)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idPlaylist
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PrivadasPorArtista")]
		public IActionResult CargarCancionesPrivadasPorIdArtista([FromQuery]string tokenDeAcceso, int idArtista)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idArtista
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPrivadasPorIdArtista(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Canciones/PorGenero")]
		public IActionResult CargarCancionesPorIdGenero([FromQuery]string tokenDeAcceso, int idGenero)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idGenero
			};

			RespuestaDeCanciones respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarCancionesPorIdGenero(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Canciones/RegistrarDeArtista")]
		public IActionResult RegistrarCancionDeArtista([FromBody]Peticiones.SolicitudDeRegistrarCancion peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			RespuestaDeCanciones respuesta;
			SolicitudDeRegistrarCancion solicitudDeRegistrarCancion = new SolicitudDeRegistrarCancion()
			{
				Token = new Token
				{
					TokenDeAcceso = peticion.token.tokenDeAcceso
				},
				Nombre = peticion.nombre,
				Audio = Google.Protobuf.ByteString.CopyFrom(peticion.audio),
				Imagen = Google.Protobuf.ByteString.CopyFrom(peticion.imagen),
				Duracion = peticion.duracion
			};

			foreach (int genero in peticion.generos)
			{
				solicitudDeRegistrarCancion.Generos.Add(genero);
			}

			try
			{
				respuesta = clienteDeMetadatos.RegistrarCancionDeArtista(solicitudDeRegistrarCancion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Canciones/RegistrarDeConsumidor")]
		public IActionResult RegistrarCancionDeConsumidor([FromBody]Peticiones.SolicitudDeRegistrarCancion peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			RespuestaDeCanciones respuesta;
			SolicitudDeRegistrarCancion solicitudDeRegistrarCancion = new SolicitudDeRegistrarCancion()
			{
				Token = new Token
				{
					TokenDeAcceso = peticion.token.tokenDeAcceso
				},
				Nombre = peticion.nombre,
				Audio = Google.Protobuf.ByteString.CopyFrom(peticion.audio),
				Imagen = Google.Protobuf.ByteString.CopyFrom(peticion.imagen)
			};

			foreach (int genero in peticion.generos)
			{
				solicitudDeRegistrarCancion.Generos.Add(genero);
			}

			try
			{
				respuesta = clienteDeMetadatos.RegistrarCancionDeConsumidor(solicitudDeRegistrarCancion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Cancion> canciones = respuesta.Canciones.ToList();
				actionResult = Ok(canciones);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpDelete]
		[Route("Canciones/Eliminar")]
		public IActionResult EliminarCancion([FromQuery] string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			SolicitudDeEliminarCancion peticion = new SolicitudDeEliminarCancion
			{
				Token = new Token()
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdCancion = idCancion
			};
			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.EliminarCancion(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok();
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		#endregion Cancion 

		#region Artista
		[HttpGet]
		[Route("Artistas/Todos")]
		public IActionResult ArtistasTodos([FromQuery] string tokenDeAcceso)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = tokenDeAcceso
			};
			RespuestaDeArtista respuesta = new RespuestaDeArtista();
			try
			{
				respuesta = clienteDeMetadatos.CargarArtistasTodos(token);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Artista> artistas = respuesta.Artista.ToList();
				actionResult = Ok(artistas);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Artistas/PorID")]
		public IActionResult CargarArtistaPorId([FromQuery]string tokenDeAcceso, int idArtista)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idArtista
			};

			RespuestaDeArtista respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarArtistaPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Artista> artistas = respuesta.Artista.ToList();
				actionResult = Ok(artistas);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Artistas/PorCancion")]
		public IActionResult CargarArtistaPorIdCancion([FromQuery]string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion
			};

			RespuestaDeArtista respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarArtistaPorIdCancion(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Artista> artistas = respuesta.Artista.ToList();
				actionResult = Ok(artistas);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Artistas/PorAlbum")]
		public IActionResult CargarArtistaPorIdAlbum([FromQuery]string tokenDeAcceso, int idAlbum)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idAlbum
			};

			RespuestaDeArtista respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarArtistaPorIdAlbum(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Artista> artistas = respuesta.Artista.ToList();
				actionResult = Ok(artistas);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}
		#endregion Artista

		#region Album 
		[HttpGet]
		[Route("Albumes/PorID")]
		public IActionResult CargarAlbumPorId([FromQuery]string tokenDeAcceso, int idAlbum)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idAlbum
			};

			RespuestaDeAlbum respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarAlbumPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Album> albumes = respuesta.Album.ToList();
				actionResult = Ok(albumes);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Albumes/PorCancion")]
		public IActionResult CargarAlbumPorIdCancion([FromQuery]string tokenDeAcceso, int idCancion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idCancion
			};

			RespuestaDeAlbum respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarAlbumPorIdCancion(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Album> albumes = respuesta.Album.ToList();
				actionResult = Ok(albumes);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Albumes/PorArtista")]
		public IActionResult CargarAlbumPorIdArtista([FromQuery]string tokenDeAcceso, int idArtista)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idArtista
			};

			RespuestaDeAlbum respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarAlbumesPorIdArtista(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Album> albumes = respuesta.Album.ToList();
				actionResult = Ok(albumes);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Albumes/PorGenero")]
		public IActionResult CargarAlbumPorIdGenero([FromQuery]string tokenDeAcceso, int idGenero)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idGenero
			};

			RespuestaDeAlbum respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarAlbumesPorIdGenero(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Album> albumes = respuesta.Album.ToList();
				actionResult = Ok(albumes);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Albumes/Registrar")]
		public IActionResult RegistrarAlbum([FromBody] Peticiones.SolicitudDeRegistrarAlbum peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			RespuestaDeAlbum respuesta;

			SolicitudDeRegistrarAlbum solicitudDeRegistrarAlbum = new SolicitudDeRegistrarAlbum()
			{
				Token = new Token
				{
					TokenDeAcceso = peticion.token.tokenDeAcceso
				},
				Nombre = peticion.nombre,
				Descripcion = peticion.descripcion,
				Imagen = Google.Protobuf.ByteString.CopyFrom(peticion.imagen)
			};

			foreach(int genero in peticion.generos)
			{
				solicitudDeRegistrarAlbum.Generos.Add(genero);
			}

			try
			{
				respuesta = clienteDeMetadatos.RegistrarAlbum(solicitudDeRegistrarAlbum);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Album> albumes = respuesta.Album.ToList();
				actionResult = Ok(albumes);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Albumes/AgregarCancion")] 
		public IActionResult AgregarCancionAAlbum([FromBody] Peticiones.SolicitudDeAgregarCancionAPlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Respuesta respuesta;

			SolicitudDeAgregarCancionAPlaylist solicitudDeAgregarCancionAPlaylist = new SolicitudDeAgregarCancionAPlaylist()
			{
				Token = new Token
				{
					TokenDeAcceso = peticion.token.tokenDeAcceso
				},
				IdCancion = peticion.idCancion,
				IdPlaylist = peticion.idPlaylist
			};

			try
			{
				respuesta = clienteDeMetadatos.AgregarCancionAAlbum(solicitudDeAgregarCancionAPlaylist);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok();
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Albumes/EliminarCancion")]
		public IActionResult EliminarCancionDeAlbum([FromBody] Peticiones.SolicitudDeEliminarCancionDePlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Respuesta respuesta;

			SolicitudDeEliminarCancionDePlaylist solicitudDeEliminarCancionAPlaylist = new SolicitudDeEliminarCancionDePlaylist()
			{
				Token = new Token
				{
					TokenDeAcceso = peticion.token.tokenDeAcceso
				},
				IdCancion = peticion.idCancion,
				IdPlaylist = peticion.idPlaylist
			};

			try
			{
				respuesta = clienteDeMetadatos.EliminarCancionDeAlbum(solicitudDeEliminarCancionAPlaylist);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok();
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpDelete]
		[Route("Albumes/Eliminar")]
		public IActionResult EliminarAlbum([FromQuery]string tokenDeAcceso, int idAlbum )
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			SolicitudDeEliminarAlbum peticion = new SolicitudDeEliminarAlbum()
			{
				Token = new Token()
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdAlbum = idAlbum
			};
			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.EliminarAlbum(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok();
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}
		#endregion Album

		#region Playlist 
		[HttpGet]
		[Route("Playlists/PorID")]
		public IActionResult CargarPlaylistPorId([FromQuery]string tokenDeAcceso, int idPlaylist)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idPlaylist
			};

			RespuestaDePlaylist respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarPlaylistPorId(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Playlist> playlist = respuesta.Playlists.ToList();
				actionResult = Ok(playlist);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Playlists/PorConsumidor")]
		public IActionResult CargarPlaylistsPorIdConsumidor([FromQuery]string tokenDeAcceso, int idConsumidor)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idConsumidor
			};

			RespuestaDePlaylist respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarPlaylistsPorIdUsuario(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Playlist> playlist = respuesta.Playlists.ToList();
				actionResult = Ok(playlist);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Playlists/Crear")]
		public IActionResult CrearPlaylist([FromBody]SolicitudDeAgregarPlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			RespuestaDePlaylist respuesta;

			try
			{
				respuesta = clienteDeMetadatos.RegistrarPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Playlist> playlists = respuesta.Playlists.ToList();
				actionResult = Ok(playlists);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Playlist/AgregarCancion")]
		public IActionResult AgregarCancionAPlaylist([FromBody]SolicitudDeAgregarCancionAPlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.AgregarCancionAPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok(respuesta);
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Playlist/EliminarCancion")]
		public IActionResult EliminarCancionDePlaylist([FromBody]SolicitudDeEliminarCancionDePlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.EliminarCancionDePlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok(respuesta);
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpPost]
		[Route("Playlist/RenombrarPlaylist")]
		public IActionResult RenombrarPlaylist([FromBody]SolicitudDeRenombrarPlaylist peticion)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.RenombrarPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok(respuesta);
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpDelete]
		[Route("Playlist/Eliminar")]
		public IActionResult EliminarPlaylist([FromQuery]string tokenDeAcceso, int idPlaylist)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			SolicitudDeEliminarPlaylist peticion = new SolicitudDeEliminarPlaylist()
			{
				Token = new Token()
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPlaylist = idPlaylist
			};
			Respuesta respuesta;

			try
			{
				respuesta = clienteDeMetadatos.EliminarPlaylist(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Exitosa)
			{
				actionResult = Ok(respuesta);
			}
			else
			{
				actionResult = StatusCode(respuesta.Motivo);
			}

			return actionResult;
		}
		#endregion Playlist

		#region Genero

		[HttpGet]
		[Route("Generos/Todos")]
		public IActionResult CargarGenerosTodos([FromQuery] string tokenDeAcceso)
		{
			IActionResult actionResult = BadRequest();
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);

			Token token = new Token
			{
				TokenDeAcceso = tokenDeAcceso
			};
			RespuestaDeGenero respuesta = new RespuestaDeGenero();
			try
			{
				respuesta = clienteDeMetadatos.CargarGenerosTodos(token);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Genero> generos = respuesta.Generos.ToList();
				actionResult = Ok(generos);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}

		[HttpGet]
		[Route("Generos/PorID")]
		public IActionResult CargarGenerosPorId([FromQuery]string tokenDeAcceso, int idGenero)
		{
			IActionResult actionResult;
			var clienteDeMetadatos = new UVFYMetadatos.Metadata.MetadataClient(ServicioDeMetadatos);
			PeticionId peticion = new PeticionId()
			{
				Token = new Token
				{
					TokenDeAcceso = tokenDeAcceso
				},
				IdPeticion = idGenero
			};

			RespuestaDeGenero respuesta;

			try
			{
				respuesta = clienteDeMetadatos.CargarGeneroPorid(peticion);
			}
			catch (System.Net.Http.HttpRequestException)
			{
				actionResult = StatusCode(500);
				return actionResult;
			}

			if (respuesta.Respuesta.Exitosa)
			{
				List<Genero> generos = respuesta.Generos.ToList();
				actionResult = Ok(generos);
			}
			else
			{
				actionResult = StatusCode(respuesta.Respuesta.Motivo);
			}

			return actionResult;
		}
		#endregion Genero
	}
}