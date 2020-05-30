using System;
using System.Collections.Generic;

namespace UVFYMetadatos.Models
{
    public partial class CancionPlaylist
    {
        public int CancionId { get; set; }
        public int PlaylistsId { get; set; }

        public virtual Canciones Cancion { get; set; }
        public virtual Playlists Playlists { get; set; }
    }
}
