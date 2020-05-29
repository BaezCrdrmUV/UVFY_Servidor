using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class CancionPlaylist
    {
        public int CancionId { get; set; }
        public int PlaylistsId { get; set; }

        public virtual Cancion Cancion { get; set; }
        public virtual Playlist Playlists { get; set; }
    }
}
