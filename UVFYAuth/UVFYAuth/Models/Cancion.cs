using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class Cancion
    {
        public Cancion()
        {
            CancionGenero = new HashSet<CancionGenero>();
            CancionPlaylist = new HashSet<CancionPlaylist>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Duracion { get; set; }
        public DateTime FechaDeLanzamiento { get; set; }
        public int ArtistaId { get; set; }
        public int AlbumsId { get; set; }

        public virtual Album Albums { get; set; }
        public virtual UsuarioArtista Artista { get; set; }
        public virtual ICollection<CancionGenero> CancionGenero { get; set; }
        public virtual ICollection<CancionPlaylist> CancionPlaylist { get; set; }
    }
}
