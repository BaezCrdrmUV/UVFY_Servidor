using Microsoft.AspNetCore.Identity;

namespace UVFYGateway.Peticiones
{
	public class SolicitudDeAgregarCancionAPlaylist
	{
		public Token token { get; set; }
		public int idCancion{ get; set; }
		public int idPlaylist { get; set; }
	}
}