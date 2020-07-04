using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UVFYMetadatos.Models
{
    public partial class Usuarios
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        [Column("Contraseña")]
        public string Contrasena { get; set; }

        public virtual UsuariosArtista UsuariosArtista { get; set; }
        public virtual UsuariosConsumidor UsuariosConsumidor { get; set; }
    }
}
