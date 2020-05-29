using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            CancionPlaylist = new HashSet<CancionPlaylist>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ConsumidorId { get; set; }

        public virtual UsuarioConsumidor Consumidor { get; set; }
        public virtual ICollection<CancionPlaylist> CancionPlaylist { get; set; }
    }
}
