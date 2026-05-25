using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using AccesoDB;

namespace Negocio
{
    public abstract class UsuarioNegocio
    {
        /// <summary>
        /// Obtener un usuario especifico utilizando su ID
        /// </summary>
        public Usuario Get(string id = "")
        {
            AccesoADatos datos = new AccesoADatos();

            try
            {
                datos.SetearConsultaSP("sp_ObtenerUsuario");
                datos.setearParametro("@ID", id);
                datos.ejecutarLectura();
                datos.Lector.Read();

                return new Usuario()
                {
                    IdUsuario = (int)datos.Lector["ID"],
                    IdRol = (int)datos.Lector["IdRol"],
                    Nombre = datos.Lector["Nombre"].ToString(),
                    Apellido = datos.Lector["Apellido"].ToString(),
                    Email = datos.Lector["Email"].ToString(),
                    FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"],
                    FechaIngreso = (DateTime)datos.Lector["FechaIngreso"]
                };
            }
            catch (Exception ex)
            {
                // PENDIENTE
                throw;
            }
        }

        /// <summary>
        /// Obtener todos los usuarios en una lista. Acepta filtro por rol
        /// </summary>
        public List<Usuario> Listar(string rol = "TODOS")
        {
            AccesoADatos datos = new AccesoADatos();
            List<Usuario> lista = new List<Usuario>();
            try
            {
                datos.SetearConsultaSP("sp_ObtenerUsuarios");
                datos.setearParametro("@Rol", rol);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Usuario()
                    {
                        IdUsuario = (int)datos.Lector["ID"],
                        IdRol = (int)datos.Lector["IdRol"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"],
                        FechaIngreso = (DateTime)datos.Lector["FechaIngreso"]
                    });
                }

                return lista;
            }
            catch (Exception ex)
            {
                // PENDIENTE
                throw;
            }
        }
        private void AltaOModificacion(Usuario usuario, bool alta)
        {
            AccesoADatos datos = new AccesoADatos();
            string SP;

            if (alta)
            {
                SP = "sp_AltaUsuario";
            }
            else
            {
                SP = "sp_ModificarUsuario";  // PENDIENTE crear en SPs
            }

            datos.SetearConsultaSP(SP);

            datos.setearParametro("@Nombre", usuario.Nombre);
            datos.setearParametro("@Apellido", usuario.Apellido);
            datos.setearParametro("@Email", usuario.Email);
            datos.setearParametro("@FechaNacimiento", usuario.FechaNacimiento);
            // datos.setearParametro("@PesoCorporalKG", usuario.PesoCorporalKG);  FALTA PROPIEDAD
            datos.setearParametro("@IdRol", usuario.IdRol);
            datos.setearParametro("@FechaIngreso", usuario.FechaIngreso);

            datos.ejecutarAccion();
        }
        public void Agregar(Usuario nuevo)
        {
            try
            {
                AltaOModificacion(nuevo, true);
            }
            catch (Exception)
            {
                // PENDIENTE
                throw;
            }
        }

        public void Modificar(Usuario existente)
        {
            try
            {
                AltaOModificacion(existente, false);
            }
            catch (Exception)
            {
                // PENDIENTE
                throw;
            }
        }
    }
}
