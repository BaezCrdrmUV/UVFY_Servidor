using System;
using System.Collections.Generic;

namespace UVFYArchivos.Models
{
    public partial class UsuariosArtista
    {
        public UsuariosArtista()
        {
            Albumes = new HashSet<Albumes>();
            Canciones = new HashSet<Canciones>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Id { get; set; }

        public virtual Usuarios IdNavigation { get; set; }
        public virtual ICollection<Albumes> Albumes { get; set; }
        public virtual ICollection<Canciones> Canciones { get; set; }
    }
}
