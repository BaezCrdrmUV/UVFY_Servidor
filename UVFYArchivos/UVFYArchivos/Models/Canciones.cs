using System;
using System.Collections.Generic;

namespace UVFYArchivos.Models
{
    public partial class Canciones
    {
        public Canciones()
        {
            CancionGenero = new HashSet<CancionGenero>();
            CancionPlaylist = new HashSet<CancionPlaylist>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Duracion { get; set; }
        public DateTime FechaDeLanzamiento { get; set; }
        public short Estado { get; set; }
        public int? ArtistaId { get; set; }
        public int? AlbumsId { get; set; }
        public int? ConsumidorId { get; set; }

        public virtual Albumes Albums { get; set; }
        public virtual UsuariosArtista Artista { get; set; }
        public virtual UsuariosConsumidor Consumidor { get; set; }
        public virtual ICollection<CancionGenero> CancionGenero { get; set; }
        public virtual ICollection<CancionPlaylist> CancionPlaylist { get; set; }
    }
}
