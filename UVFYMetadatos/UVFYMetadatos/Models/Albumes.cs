using System;
using System.Collections.Generic;

namespace UVFYMetadatos.Models
{
    public partial class Albumes
    {
        public Albumes()
        {
            AlbumGenero = new HashSet<AlbumGenero>();
            Canciones = new HashSet<Canciones>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int ArtistasId { get; set; }

        public virtual UsuariosArtista Artistas { get; set; }
        public virtual ICollection<AlbumGenero> AlbumGenero { get; set; }
        public virtual ICollection<Canciones> Canciones { get; set; }
    }
}
