namespace UVFYGateway.Peticiones
{
	public class SolicitudDeEliminarCancionDePlaylist
	{
		public Token token { get; set; }
		public int idCancion { get; set; }
		public int idPlaylist { get; set; }
	}
}