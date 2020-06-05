using System;
using System.Collections.Generic;

namespace UVFYArchivos.Models
{
    public partial class AlbumGenero
    {
        public int AlbumesId { get; set; }
        public int GenerosId { get; set; }

        public virtual Albumes Albumes { get; set; }
        public virtual Generos Generos { get; set; }
    }
}
