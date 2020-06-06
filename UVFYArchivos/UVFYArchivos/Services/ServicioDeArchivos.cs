using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using UVFYArchivos.DAO;
using UVFYArchivos.Excepciones;
using UVFYArchivos.Models;
using UVFYArchivos.ServiciosInternos;

namespace UVFYArchivos
{
	public class ServicioDeArchivos : Archivos.ArchivosBase
	{
		private readonly ILogger<ServicioDeArchivos> _logger;
		public ServicioDeArchivos(ILogger<ServicioDeArchivos> logger)
		{
			_logger = logger;
		}

		public override Task<Respuesta> GuardarCaratulaDeCancionPorId(PeticionGuardadoId request, ServerCallContext context)
		{
			Respuesta respuesta = new Respuesta();
			try
			{
				using (UVFYContext contexto = new UVFYContext())
				{
					CancionDAO cancionDAO = new CancionDAO();
					if (cancionDAO.CargarPorId(request.IdPeticion) != null)
					{
						try
						{
							if (ServiciosDeIO.VerificarEstructuraDeArchivosCancion(request.IdPeticion.ToString()))
							{
								ServiciosDeIO.GuardarArchivoDeCancion(request.IdPeticion.ToString(), request.Datos.ToByteArray(), TipoDeArchivo.png);
								respuesta.Exitosa = true;
							}
							else
							{
								//No se pudo realizar el guardado, error de io
								respuesta.Exitosa = false;
								respuesta.Motivo = 500;
							}
						}
						catch (IOException)
						{
							//No se pudo realizar el guardado, error de io
							respuesta.Exitosa = false;
							respuesta.Motivo = 500;
						}
					}
					else
					{
						//No se encontro la cancion con el id dado (Deberia ser imposible porque se valida)
						respuesta.Exitosa = false;
						respuesta.Motivo = 400;
					}
				}
			}
			catch (AccesoADatosException)
			{
				//Error conectandose a la base de datos
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCaratula> CargarCaratulaDeCancionPorId(PeticionId request, ServerCallContext context)
		{
			RespuestaDeCaratula respuesta = new RespuestaDeCaratula()
			{
				Respuesta = new Respuesta()
			};
			try
			{
				if (ServiciosDeIO.VerificarEstructuraDeArchivosCancion(request.IdPeticion.ToString()))
				{
					respuesta.Caratula = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarCaratulaDeCancion(request.IdPeticion.ToString()));
					respuesta.Respuesta.Exitosa = true;
				}
				else
				{
					//No se pudo realizar la lectura, error de io
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
				}
			}
			catch (IOException)
			{
				//No se encontro el archivo
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCaratula> CargarCaratulaDeAlbumPorId(PeticionId request, ServerCallContext context)
		{
			RespuestaDeCaratula respuesta = new RespuestaDeCaratula()
			{
				Respuesta = new Respuesta()
			};
			try
			{
				if (ServiciosDeIO.VerificarEstructuraDeArchivosAlbum())
				{
					respuesta.Caratula = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarCaratulaDeAlbum(request.IdPeticion.ToString()));
					respuesta.Respuesta.Exitosa = true;
				}
				else
				{
					//No se pudo realizar la lectura, error de io
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
				}
			}
			catch (IOException)
			{
				//No se encontro el archivo
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
			}

			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> GuardarCaratulaDeAlbumPorId(PeticionGuardadoId request, ServerCallContext context)
		{
			Respuesta respuesta = new Respuesta();
			try
			{
				using (UVFYContext contexto = new UVFYContext())
				{
					AlbumDAO albumDAO = new AlbumDAO();
					if (albumDAO.CargarPorId(request.IdPeticion) != null)
					{
						try
						{
							if (ServiciosDeIO.VerificarEstructuraDeArchivosAlbum())
							{
								ServiciosDeIO.GuardarCaratulaDeAlbum(request.IdPeticion.ToString(), request.Datos.ToByteArray());
								respuesta.Exitosa = true;
							}
							else
							{
								//No se pudo realizar el guardado, error de io
								respuesta.Exitosa = false;
								respuesta.Motivo = 500;
							}
						}
						catch (IOException)
						{
							//No se pudo realizar el guardado, error de io
							respuesta.Exitosa = false;
							respuesta.Motivo = 500;
						}
					}
					else
					{
						//No se encontro la cancion con el id dado (Deberia ser imposible porque se valida)
						respuesta.Exitosa = false;
						respuesta.Motivo = 400;
					}
				}
			}
			catch (AccesoADatosException)
			{
				//Error conectandose a la base de datos
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}



			return Task.FromResult(respuesta);
		}

		public override Task<Respuesta> GuardarAudioDeCancionPorIdYCalidad(PeticionGuardadoIdYCalidad request, ServerCallContext context)
		{
			Respuesta respuesta = new Respuesta();
			try
			{
				using (UVFYContext contexto = new UVFYContext())
				{
					CancionDAO cancionDAO = new CancionDAO();
					if (cancionDAO.CargarPorId(request.IdPeticion) != null)
					{
						try
						{
							if (ServiciosDeIO.VerificarEstructuraDeArchivosCancion(request.IdPeticion.ToString()))
							{
								switch (request.Calidad)
								{
									case calidad.Alta:
										ServiciosDeIO.GuardarArchivoDeCancion(request.IdPeticion.ToString(), request.Datos.ToByteArray(), TipoDeArchivo.mp3_320);
										break;
									case calidad.Media:
										ServiciosDeIO.GuardarArchivoDeCancion(request.IdPeticion.ToString(), request.Datos.ToByteArray(), TipoDeArchivo.mp3_256);
										break;
									case calidad.Baja:
										ServiciosDeIO.GuardarArchivoDeCancion(request.IdPeticion.ToString(), request.Datos.ToByteArray(), TipoDeArchivo.mp3_128);
										break;
								}
								respuesta.Exitosa = true;
							}
							else
							{
								//No se pudo realizar el guardado, error de io
								respuesta.Exitosa = false;
								respuesta.Motivo = 500;
							}
						}
						catch (IOException)
						{
							//No se pudo realizar el guardado, error de io
							respuesta.Exitosa = false;
							respuesta.Motivo = 500;
						}
					}
					else
					{
						//No se encontro la cancion con el id dado (Deberia ser imposible porque se valida)
						respuesta.Exitosa = false;
						respuesta.Motivo = 400;
					}
				}
			}
			catch (AccesoADatosException)
			{
				//Error conectandose a la base de datos
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			return Task.FromResult(respuesta);
		}

		public override Task<RespuestaDeCancion> CargarAudioDeCancionPorIdYCalidad(PeticionIdYCalidad request, ServerCallContext context)
		{
			RespuestaDeCancion respuesta = new RespuestaDeCancion()
			{
				Respuesta = new Respuesta()
			};
			try
			{
				if (ServiciosDeIO.VerificarEstructuraDeArchivosCancion(request.IdPeticion.ToString()))
				{
					switch (request.Calidad)
					{
						case calidad.Alta:
							respuesta.Autio = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarAudioDeCancionPorCalidad(request.IdPeticion.ToString(), TipoDeArchivo.mp3_320));
							break;
						case calidad.Media:
							respuesta.Autio = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarAudioDeCancionPorCalidad(request.IdPeticion.ToString(), TipoDeArchivo.mp3_256));
							break;
						case calidad.Baja:
							respuesta.Autio = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarAudioDeCancionPorCalidad(request.IdPeticion.ToString(), TipoDeArchivo.mp3_128));
							break;
					}
					respuesta.Respuesta.Exitosa = true;
				}
				else
				{
					//No se pudo realizar la lectura, error de io
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
				}
			}
			catch (IOException)
			{
				//No se encontro el archivo
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
			}

			return Task.FromResult(respuesta);
		}
	}
}
