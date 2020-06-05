using System;
using System.Collections.Generic;

namespace UVFYArchivos.Models
{
    public partial class CancionGenero
    {
        public int CancionesId { get; set; }
        public int GenerosId { get; set; }

        public virtual Canciones Canciones { get; set; }
        public virtual Generos Generos { get; set; }
    }
}
