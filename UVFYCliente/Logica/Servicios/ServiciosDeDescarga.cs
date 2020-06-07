﻿using Logica.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Servicios
{
	public class ServiciosDeDescarga
	{
		public async void DescargarAudioDeCancion(int idCancion, string token)
		{
			ArchivosDAO archivosDAO = new ArchivosDAO(token);
			byte[] audio = await archivosDAO.CargarAudioDeCancionPorId(idCancion);
			ServiciosDeIO.GuardarCancion(audio, idCancion);
		}

		public async void DescargarCaratulaDeCancion(int idCancion, string token)
		{
			ArchivosDAO archivosDAO = new ArchivosDAO(token);
			byte[] imagen = await archivosDAO.CargarCaratulaDeCancionPorId(idCancion);
			ServiciosDeIO.GuardarCaratula(imagen, idCancion);
		}
	}
}