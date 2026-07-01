using AccesoDB;
using Dominio;
using Integraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ClienteNegocio
    {
        /// <summary>
        /// Obtener un cliente especifico utilizando su ID.
        /// full = true: devuelve el usuario con su suscripcion activa actual, rutinas y records personales.
        /// </summary>
        public Cliente Get(string id = "", bool full = false)
        {
            string Excepcion = "Ocurrio un error al obtener el cliente (ClienteNegocio.Get())\n";

            AccesoADatos datos = new AccesoADatos();
            Cliente cliente = new Cliente();
            try
            {
                datos.SetearConsulta("SELECT * FROM Usuarios WHERE IdUsuarios = @ID");
                datos.setearParametro("@ID", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    cliente.IdUsuario = Convert.ToInt32(datos.Lector["IdUsuarios"]);
                    cliente.Nombre = datos.Lector["Nombre"].ToString();
                    cliente.Apellido = datos.Lector["Apellido"].ToString();
                    cliente.Email = datos.Lector["Email"].ToString();
                    cliente.FechaNacimiento = Convert.ToDateTime(datos.Lector["FechaNacimiento"]);
                    cliente.FechaIngreso = Convert.ToDateTime(datos.Lector["FechaIngreso"]);
                    cliente.Activo = Convert.ToBoolean(datos.Lector["Activo"]);
                    cliente.PesoCorporal = Convert.ToSingle(datos.Lector["PesoCorporalKG"]);

                    if (full)
                    {
                        cliente.SuscripcionCliente = new SuscripcionNegocio().GetSuscripcionCliente(id, EstadoSuscripcion.ACTIVA);
                        cliente.RecordsPersonales = new RecordsNegocio().GetRecordsUsuario(id);
                    }

                    return cliente;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
            return cliente;
        }

        /// <summary>
        /// Obtener todos los clientes. Opcionalmente solo los activos
        /// </summary>
        public List<Cliente> ListarClientes(bool activos = true)
        {
            string Excepcion = "Ocurrio un error al obtener una lista de clientes (ClienteNegocio.ListarClientes())\n";
            string query = "SELECT * FROM Usuarios WHERE IdRol = @Rol" + (activos ? " AND Activo = 1" : "");

            AccesoADatos datos = new AccesoADatos();
            List<Cliente> lista = new List<Cliente>();
            try
            {
                datos.SetearConsulta(query);
                datos.setearParametro("@Rol", Roles.CLIENTE);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        IdUsuario = Convert.ToInt32(datos.Lector["IdUsuarios"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(datos.Lector["FechaNacimiento"]),
                        FechaIngreso = Convert.ToDateTime(datos.Lector["FechaIngreso"]),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        PesoCorporal = Convert.ToSingle(datos.Lector["PesoCorporalKG"])
                    };

                    lista.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }
        private void AltaOModificacion(Cliente cliente, AccesoADatos datos, string pass)
        {
            // Creacion
            if (cliente.IdUsuario == 0)
            {
                datos.SetearConsultaSP("sp_CrearUsuario");
                datos.setearParametro("@PesoCorporalKG", cliente.PesoCorporal);
            }
            // Modificacion
            else
            {
                datos.SetearConsultaSP("sp_ModificarUsuario");
                datos.setearParametro("@IdUsuario", cliente.IdUsuario);
                datos.setearParametro("@PesoCorporal", cliente.PesoCorporal);
                datos.setearParametro("@Activo", cliente.Activo);
            }

            datos.setearParametro("@Nombre", cliente.Nombre);
            datos.setearParametro("@Apellido", cliente.Apellido);
            datos.setearParametro("@Email", cliente.Email);
            datos.setearParametro("@FechaNacimiento", cliente.FechaNacimiento);
            datos.setearParametro("@IdRol", cliente.Rol.IdRol);
            datos.setearParametro("@FechaIngreso", cliente.FechaIngreso);
            // Solo se hashea cuando viene una contrasenia nueva. En la modificacion
            // de perfil se pasa "" y el SP deja la contrasenia existente intacta.
            datos.setearParametro("@Pass", string.IsNullOrEmpty(pass) ? pass : HashContrasenia.Hashear(pass));

            datos.ejecutarAccion();
        }
        public void Agregar(Cliente nuevo, string pass)
        {
            string Excepcion = "Ocurrio un error al agregar un cliente (ClienteNegocio.Agregar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(nuevo, datos, pass);
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Cliente existente)
        {
            string Excepcion = "Ocurrio un error al modificar un cliente (ClienteNegocio.Modificar())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                AltaOModificacion(existente, datos, "");
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void DarBaja(Cliente cliente)
        {
            string Excepcion = "Ocurrio un error al dar de baja al cliente (ClienteNegocio.DarBaja())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_EliminarCliente");
                datos.setearParametro("@IdUsuario", cliente.IdUsuario);
                datos.ejecutarAccion();

                cliente.Activo = false;
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool ExisteEmail(string email, int idExcluir = 0)
        {
            string Excepcion = "Ocurrio un error al chequear email de cliente (ClienteNegocio.ExisteEmail())\n";

            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM Usuarios WHERE Email = @email AND IdUsuarios <> @idExcluir");
                datos.setearParametro("@email", email);
                datos.setearParametro("@idExcluir", idExcluir);
                int cantidad = datos.EjecutarScalar();

                if (cantidad > 0) return true;
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion, ex);
            }
            finally { datos.cerrarConexion(); }
            
            return false;
        }
    }
}
