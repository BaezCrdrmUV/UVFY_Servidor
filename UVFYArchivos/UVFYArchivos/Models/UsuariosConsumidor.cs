using System;
using System.Collections.Generic;

namespace UVFYArchivos.Models
{
    public partial class UsuariosConsumidor
    {
        public UsuariosConsumidor()
        {
            Canciones = new HashSet<Canciones>();
            Playlists = new HashSet<Playlists>();
        }

        public short EstadoDeSuscripcion { get; set; }
        public DateTime FechaDeFinalDeSuscripcion { get; set; }
        public int Id { get; set; }

        public virtual Usuarios IdNavigation { get; set; }
        public virtual ICollection<Canciones> Canciones { get; set; }
        public virtual ICollection<Playlists> Playlists { get; set; }
    }
}
