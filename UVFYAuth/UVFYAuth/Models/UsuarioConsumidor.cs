using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class UsuarioConsumidor
    {
        public UsuarioConsumidor()
        {
            Playlists = new HashSet<Playlist>();
        }

        public short EstadoDeSuscripcion { get; set; }
        public DateTime FechaDeFinalDeSuscripcion { get; set; }
        public int Id { get; set; }

        public virtual Usuario IdNavigation { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
