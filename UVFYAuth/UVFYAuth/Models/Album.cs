using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class Album
    {
        public Album()
        {
            AlbumGenero = new HashSet<AlbumGenero>();
            Canciones = new HashSet<Cancion>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int ArtistasId { get; set; }

        public virtual UsuarioArtista Artistas { get; set; }
        public virtual ICollection<AlbumGenero> AlbumGenero { get; set; }
        public virtual ICollection<Cancion> Canciones { get; set; }
    }
}
