using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDB;
using Dominio;

namespace Negocio
{
    public class AdminNegocio
    {
        //Este bloque es para traer una lista de cada tipo de rol
        public List<Cliente> ObtenerClientes()
        {
            // Lógica para obtener la lista de clientes desde la base de datos
            return new List<Cliente>();
        }
        public List<Entrenador> ObtenerEntrenadores()
        {
            // Lógica para obtener la lista de entrenadores desde la base de datos
            return new List<Entrenador>();
        }
        public List<Recepcionista> ObtenerRecepcionistas()
        {
            // Lógica para obtener la lista de recepcionistas desde la base de datos
            return new List<Recepcionista>();
        }
        //Admins
        public List<Usuario> ObtenerUsuarioPorRol(string sp_tipoRol)
        {
            // Lógica para obtener la lista de adminis desde la base de datos
            AccesoADatos datos = new AccesoADatos();
            List<Usuario> listaUsuario = new List<Usuario>();
            try
            {
                datos.SetearConsultaSP(sp_tipoRol);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Usuario user;
                    {
                        switch ((string)datos.Lector["Rol"])
                        {
                            case "Admin":
                                user = new Admin();
                                break;
                            case "Entrenador":
                                user = new Entrenador();
                                break;
                            case "Administrativo":
                                user = new Recepcionista();
                                break;
                            case "Cliente":
                                Cliente cliente = new Cliente();
                                cliente.SuscripcionCliente = new Suscripcion();
                                cliente.SuscripcionCliente.IdSuscripcion = Convert.ToInt32(datos.Lector["IdEstado"]);
                                cliente.SuscripcionCliente.Estado = (EstadoSuscripcion)(Convert.ToInt32(datos.Lector["IdEstado"]));
                                cliente.PesoCorporal = float.Parse(datos.Lector["PesoCorporalKG"].ToString());
                                user = cliente;
                                break;
                            default:
                                throw new Exception("Un problema al captar tipo de rol - AdminNegocio/ObtenerAdmins " + (string)datos.Lector["Rol"]);
                                
                        }
                        user.IdUsuario = (int)datos.Lector["IdUsuarios"];
                        user.Nombre = (string)datos.Lector["Nombre"];
                        user.Apellido = (string)datos.Lector["Apellido"];
                        user.Email = (string)datos.Lector["Email"];
                        user.FechaNacimiento = (DateTime)datos.Lector["Fecha de Nacimiento"];
                        user.Rol.RolDescripcion = (string)datos.Lector["Rol"];
                        user.FechaIngreso = (DateTime)datos.Lector["Fecha de Ingreso"];
                        user.Activo = (bool)datos.Lector["Activo"];
                    };
                    listaUsuario.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw;
                //throw new Exception("Ocurrio un problema al traer los admins de base de datos - AdminNegocio/ObtenerAdmins");
            }
            return listaUsuario;
        }  // Funcion que ya esta ok. Trae todo sim problemas
        public void crearAdmin(Admin admin, string pass) 
        { 
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_CrearUsuario");
                datos.setearParametro("@Nombre", admin.Nombre);
                datos.setearParametro("@Apellido", admin.Apellido);
                datos.setearParametro("@Email", admin.Email);
                datos.setearParametro("@FechaNacimiento", admin.FechaNacimiento);
                datos.setearParametro("@PesoCorporalKG", 0);
                datos.setearParametro("@IdRol", admin.Rol.IdRol);
                datos.setearParametro("@FechaIngreso", DateTime.Now);
                datos.setearParametro("@Pass", pass);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un problema al crear el admin en la base de datos - AdminNegocio/createAdmin");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificarAdmin(Admin admin, string pass) 
        { 
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_ModificarUsuario");
                datos.setearParametro("@Nombre", admin.Nombre);
                datos.setearParametro("@Apellido", admin.Apellido);
                datos.setearParametro("@Email", admin.Email);
                datos.setearParametro("@FechaNacimiento", admin.FechaNacimiento);
                datos.setearParametro("@PesoCorporal", 0);
                datos.setearParametro("@IdRol", admin.Rol.IdRol);
                datos.setearParametro("@FechaIngreso", admin.FechaIngreso);
                datos.setearParametro("@Activo", admin.Activo);
                datos.setearParametro("@Pass", pass);
                datos.setearParametro("@IdUsuario", admin.IdUsuario);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        //Este bloque es para gestionar cada tipo de Rol
        //Métodos para administrar clientes
        public void activarCancelarSuscripcionCliente(Cliente cliente) { }//funcion compartida con recepcionista (30 dias de suscripcion en forzado)
        public void cancelarSuscripcionCliente(Cliente cliente) { }
    }



}
