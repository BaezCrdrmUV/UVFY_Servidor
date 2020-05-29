using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class Consumidor
    {
        public Consumidor()
        {
            Canciones = new HashSet<Cancion>();
            Playlists = new HashSet<Playlist>();
        }

        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public short EstadoDeSuscripcion { get; set; }
        public DateTime FechaDeFinalDeSuscripcion { get; set; }

        public virtual ICollection<Cancion> Canciones { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
