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
						if (ServiciosDeIO.VerificarEstructuraDeArchivosCancion(request.IdPeticion.ToString()))
						{
							ServiciosDeIO.GuardarArchivoDeCancion(request.IdPeticion.ToString(), request.Datos.ToByteArray(), TipoDeArchivo.png);
						}
						else
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
			catch (System.Net.Http.HttpRequestException)
			{
				//Error conectandose a otro servicio
				respuesta.Exitosa = false;
				respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
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
					try
					{
						respuesta.Caratula = Google.Protobuf.ByteString.CopyFrom(ServiciosDeIO.CargarCaratulaDeCancion(request.IdPeticion.ToString()));
						respuesta.Respuesta.Exitosa = true;
					}
					catch (IOException)
					{
						//No se encontro el archivo
						respuesta.Respuesta.Exitosa = false;
						respuesta.Respuesta.Motivo = 500;
					}
				}
				else
				{
					//No se pudo realizar la lectura, error de io
					respuesta.Respuesta.Exitosa = false;
					respuesta.Respuesta.Motivo = 500;
				}
			}
			catch (System.Net.Http.HttpRequestException)
			{
				//Error conectandose a otro servicio
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}
			catch (AccesoADatosException)
			{
				//Error conectandose a la base de datos
				respuesta.Respuesta.Exitosa = false;
				respuesta.Respuesta.Motivo = 500;
				return Task.FromResult(respuesta);
			}

			return Task.FromResult(respuesta);
		}
	}
}
