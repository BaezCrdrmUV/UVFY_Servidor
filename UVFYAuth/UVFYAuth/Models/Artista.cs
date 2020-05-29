using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class Artista
    {
        public Artista()
        {
            Albumes = new HashSet<Album>();
            Canciones = new HashSet<Cancion>();
        }

        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Contraseña { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Album> Albumes { get; set; }
        public virtual ICollection<Cancion> Canciones { get; set; }
    }
}
