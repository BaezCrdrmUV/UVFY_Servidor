using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UVFYAuth.Dominio;
using UVFYAuth.Models;

namespace UVFYAuth.DAOS
{
	public static class UsuarioDAO
	{
        public static bool ValidarExistenciaDeUsuarioPorID(int id)
        {
            bool resultadoDeExistencia = false;
            Usuario usuarioLocalizado;
            using (UVFYContext context = new UVFYContext())
            {
                usuarioLocalizado = context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
            }
            if (usuarioLocalizado != null)
            {
                resultadoDeExistencia = true;
            }

            return resultadoDeExistencia;
        }

        public static bool ValidarExistenciaDeCorreoYContraseña(string correo, string contraseña)
        {
            bool resultadoDeExistencia = false;
            Usuario usuarioLocalizado;
            using (UVFYContext context = new UVFYContext())
            {
                usuarioLocalizado = context.Usuarios.FirstOrDefault(usuario => usuario.CorreoElectronico == correo && usuario.Contraseña == contraseña);
            }
            if (usuarioLocalizado != null)
            {
                resultadoDeExistencia = true;
            }

            return resultadoDeExistencia;
        }

        public static Usuario CargarUsuarioPorCorreo(string correo)
        {
            Usuario usuario = new Usuario();
            if (ValidarExistenciaDeUsuarioCorreo(correo))
            {
                Usuario usuarioBD;

                using (UVFYContext context = new UVFYContext())
                {
                    usuarioBD = context.Usuarios.FirstOrDefault(usuarioBusqueda => usuarioBusqueda.CorreoElectronico == correo);
                }

                if (usuarioBD != null)
                {
                    usuario = usuarioBD;
                }
            }
            else
            {
                usuario.Id = 0;
            }
            return usuario;
        }

        public static Usuario CargarUsuarioPorId(int id)
        {
            Usuario usuario = new Usuario();
            if (ValidarExistenciaDeUsuarioPorID(id))
            {
                Usuario usuarioBD;

                using (UVFYContext context = new UVFYContext())
                {
                    usuarioBD = context.Usuarios.FirstOrDefault(usuarioBusqueda => usuarioBusqueda.Id == id);
                }

                if (usuarioBD != null)
                {
                    usuario = usuarioBD;
                }
            }
            else
            {
                usuario.Id = 0;
            }
            return usuario;
        }

        public static bool ValidarExistenciaDeUsuarioCorreo(string correo)
        {
            List<Usuario> usuariosContext;
            using (UVFYContext context = new UVFYContext())
            {
                usuariosContext = context.Usuarios.ToList();
            }
            bool resultadoDeExistencia = usuariosContext.Exists(usuario => usuario.CorreoElectronico == correo);

            return resultadoDeExistencia;
        }

        public static TipoDeUsuario ObtenerTipoDeUsuarioPorID(int id)
        {
            TipoDeUsuario tipoDeUsuario = TipoDeUsuario.Indefinido;

            if (ValidarExistenciaDeUsuarioPorID(id))
            {
               
                using (UVFYContext context = new UVFYContext())
                {
                    UsuarioConsumidor consumidor;
                    consumidor = context.UsuariosConsumidor.FirstOrDefault(usuarioBusqueda => usuarioBusqueda.Id == id);
                    if (consumidor != null && consumidor.Id > 0)
                    {
                        tipoDeUsuario = TipoDeUsuario.Consumidor;
                    }
                    else
                    {
                        UsuarioArtista artista;
                        artista = context.UsuariosArtista.FirstOrDefault(usuarioBusqueda => usuarioBusqueda.Id == id);
                        if (artista != null && artista.Id > 0)
                        {
                            tipoDeUsuario = TipoDeUsuario.Artista;
                        }
                    }
                }
                
            }

            return tipoDeUsuario;
        }

        public static void GuardarUsuario(Usuario usuario)
        {
            using (UVFYContext context = new UVFYContext())
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
            }
        }

    }
}
