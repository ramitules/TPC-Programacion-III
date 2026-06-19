using AccesoDB;
using Dominio;
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
                    cliente.IdUsuario = (int)datos.Lector["IdUsuarios"];
                    cliente.Rol = Roles.CLIENTE;
                    cliente.Nombre = datos.Lector["Nombre"].ToString();
                    cliente.Apellido = datos.Lector["Apellido"].ToString();
                    cliente.Email = datos.Lector["Email"].ToString();
                    cliente.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    cliente.FechaIngreso = (DateTime)datos.Lector["FechaIngreso"];
                    cliente.Activo = (bool)datos.Lector["Activo"];
                    cliente.PesoCorporal = float.Parse(datos.Lector["PesoCorporalKG"].ToString());

                    if (full)
                    {
                        cliente.SuscripcionCliente = new SuscripcionNegocio().GetSuscripcionCliente(id, EstadoSuscripcion.ACTIVA);
                        cliente.RutinasCliente = new RutinasNegocio().GetRutinasUsuario(id);
                        cliente.RecordsPersonales = new RecordsNegocio().GetRecordsUsuario(id);
                    }

                    return cliente;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion + ex.ToString());
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
                        IdUsuario = (int)datos.Lector["IdUsuarios"],
                        Rol = Roles.CLIENTE,
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"],
                        FechaIngreso = (DateTime)datos.Lector["FechaIngreso"],
                        Activo = (bool)datos.Lector["Activo"],
                        PesoCorporal = (float)datos.Lector["PesoCorporalKG"]
                    };

                    lista.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion + ex.ToString());
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
            datos.setearParametro("@Pass", pass);

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
                throw new Exception(Excepcion + ex.ToString());
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
                throw new Exception(Excepcion + ex.ToString());
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
                cliente.Activo = false;
                AltaOModificacion(cliente, datos, "");
            }
            catch (Exception ex)
            {
                throw new Exception(Excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void SetRecordPersonal(Ejercicio ejercicio, Cliente cliente)
        {
            Records record = new Records();
            record.Ejercicio = ejercicio;
            
            if (cliente.RecordsPersonales is null)
                cliente.RecordsPersonales = new List<Records>();

            // PENDIENTE
        }
        /// <summary>
        /// Chequea si se esta estableciendo un nuevo record en el ejercicio que el
        /// cliente esta realizando
        /// </summary>
        /// <returns></returns>
        public bool EsRecordPersonal(Cliente cliente)
        { // PENDIENTE
            if (cliente.RecordsPersonales is null)
            {  // No habia records registrados (nuevo usuario tal vez)
                cliente.RecordsPersonales = new List<Records>();
                return true;
            }

            return false;
        }
    }
}
