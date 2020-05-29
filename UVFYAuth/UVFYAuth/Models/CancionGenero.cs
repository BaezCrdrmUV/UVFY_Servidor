using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class CancionGenero
    {
        public int CancionesId { get; set; }
        public int GenerosId { get; set; }

        public virtual Cancion Canciones { get; set; }
        public virtual Genero Generos { get; set; }
    }
}
