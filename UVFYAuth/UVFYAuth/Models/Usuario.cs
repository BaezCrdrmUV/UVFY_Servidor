using System;
using System.Collections.Generic;
using UVFYAuth.LocalServices;

namespace UVFYAuth.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contraseña { get; set; }

        public virtual UsuarioArtista UsuariosArtista { get; set; }
        public virtual UsuarioConsumidor UsuariosConsumidor { get; set; }

        public bool Validar()
        {
            bool resultado = false;
            if(VerificationServices.VerifyEmail(CorreoElectronico) && VerificationServices.VerifyString(NombreDeUsuario) && VerificationServices.VerifyPassword(Contraseña))
            {
                if(UsuariosArtista != null)
                {
                    if (VerificationServices.VerifyString(UsuariosArtista.Nombre) && VerificationServices.VerifyString(UsuariosArtista.Descripcion))
                    {
                        resultado = true;
                    }
                }
                else if(UsuariosConsumidor != null)
                {
                    resultado = true;
                }
            }
            return resultado;
        }
    }
}
