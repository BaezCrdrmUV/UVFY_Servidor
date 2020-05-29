using System;
using System.Collections.Generic;

namespace UVFYAuth.Models
{
    public partial class UsuarioArtista
    {
        public UsuarioArtista()
        {
            Albumes = new HashSet<Album>();
            Canciones = new HashSet<Cancion>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Id { get; set; }

        public virtual Usuario IdNavigation { get; set; }
        public virtual ICollection<Album> Albumes { get; set; }
        public virtual ICollection<Cancion> Canciones { get; set; }
    }
}
