using System;
using System.Collections.Generic;

namespace UVFYMetadatos.Models
{
    public partial class Playlists
    {
        public Playlists()
        {
            CancionPlaylist = new HashSet<CancionPlaylist>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int ConsumidorId { get; set; }

        public virtual UsuariosConsumidor Consumidor { get; set; }
        public virtual ICollection<CancionPlaylist> CancionPlaylist { get; set; }
    }
}
