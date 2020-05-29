using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class AlbumGenero
    {
        public int AlbumesId { get; set; }
        public int GenerosId { get; set; }

        public virtual Album Albumes { get; set; }
        public virtual Genero Generos { get; set; }
    }
}
