using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDB;
using Dominio;
using Integraciones;

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
                            case "Recepcionista":
                                user = new Recepcionista();
                                break;
                            case "Cliente":
                                Cliente cliente = new Cliente();
                                cliente.SuscripcionCliente = new Suscripcion();
                                if (datos.Lector["IdEstado"] != DBNull.Value)
                                {
                                    cliente.SuscripcionCliente.IdSuscripcion = Convert.ToInt32(datos.Lector["IdEstado"]);
                                    cliente.SuscripcionCliente.Estado = (EstadoSuscripcion)(Convert.ToInt32(datos.Lector["IdEstado"]));
                                }
                                else
                                {
                                    cliente.SuscripcionCliente.IdSuscripcion = 4;
                                    cliente.SuscripcionCliente.Estado = EstadoSuscripcion.VIGENTE_PENDIENTE;
                                }

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
                throw ex;
                //throw new Exception("Ocurrio un problema al traer los admins de base de datos - AdminNegocio/ObtenerAdmins");
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaUsuario;
        }  // Funcion que ya esta ok. Trae todo sim problemas
        public void crearUsuario(Usuario usuario, string pass) 
        { 
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_CrearUsuario");
                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@FechaNacimiento", usuario.FechaNacimiento);
                datos.setearParametro("@PesoCorporalKG", 0);
                datos.setearParametro("@IdRol", usuario.Rol.IdRol);
                datos.setearParametro("@FechaIngreso", DateTime.Now);
                // datos.setearParametro("@Pass", pass);
                datos.setearParametro("@Pass", string.IsNullOrEmpty(pass) ? pass : HashContrasenia.Hashear(pass));
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un problema al crear el usuario en la base de datos - AdminNegocio/crearUsuario");
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificarUsuario(Usuario user, string pass) 
        { 
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_ModificarUsuario");
                datos.setearParametro("@Nombre", user.Nombre);
                datos.setearParametro("@Apellido", user.Apellido);
                datos.setearParametro("@Email", user.Email);
                datos.setearParametro("@FechaNacimiento", user.FechaNacimiento);
                datos.setearParametro("@PesoCorporal", 0);
                datos.setearParametro("@IdRol", user.Rol.IdRol);
                datos.setearParametro("@FechaIngreso", user.FechaIngreso);
                datos.setearParametro("@Activo", user.Activo);
                // datos.setearParametro("@Pass", pass);
                datos.setearParametro("@Pass", string.IsNullOrEmpty(pass) ? pass : HashContrasenia.Hashear(pass));
                datos.setearParametro("@IdUsuario", user.IdUsuario);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void activarInactivarUsuario(int id) 
        { 
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Activar_Inactivar_Usuario");
                datos.setearParametro("@IdUsuario", id);
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




        //Obtener suscripciones
        public List<Suscripcion> listaSuscripciones() 
        {
            List<Suscripcion> listaSuscripciones = new List<Suscripcion>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Traer_Estados_De_Suscripciones");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Suscripcion suscripcion = new Suscripcion();
                    suscripcion.IdSuscripcion = Convert.ToInt32(datos.Lector["IdSuscripcionesEstados"]);
                    suscripcion.Estado = (EstadoSuscripcion)(Convert.ToInt32(datos.Lector["IdSuscripcionesEstados"]));
                    listaSuscripciones.Add(suscripcion);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaSuscripciones; 
        }
        //grupo listar
        public List<GrupoMuscular> listarGruposMusculares() 
        {
            List<GrupoMuscular> listaGrupos = new List<GrupoMuscular>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Traer_Grupos_Musculares");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    GrupoMuscular grupo = new GrupoMuscular();
                    grupo.IdGrupoMuscular = Convert.ToInt32(datos.Lector["IdGruposMusculares"]);
                    grupo.NombreGrupoMuscular = (string)datos.Lector["Nombre"];
                    listaGrupos.Add(grupo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaGrupos;
        }
        public List<Ejercicio> listarEjercicios() 
        {
            List<Ejercicio> listaEjercicios = new List<Ejercicio>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Traer_Ejercicios");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Ejercicio ejercicio = new Ejercicio();
                    ejercicio.IdEjercicio = Convert.ToInt32(datos.Lector["IdEjercicios"]);
                    ejercicio.NombreEjercicio = (string)datos.Lector["Nombre"];
                    ejercicio.GrupoMuscular = new GrupoMuscular();
                    ejercicio.GrupoMuscular.IdGrupoMuscular = Convert.ToInt32(datos.Lector["IdGrupoMuscular"]);
                    ejercicio.GrupoMuscular.NombreGrupoMuscular = (string)datos.Lector["GrupoMuscular"];
                    ejercicio.LinkExplicacion = datos.Lector["LinkExplicacion"] != DBNull.Value? (string)datos.Lector["LinkExplicacion"] : "";
                    listaEjercicios.Add(ejercicio);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaEjercicios;
        }
        public List<Plan> listarPlanes() 
        {
            List<Plan> listaPlanes = new List<Plan>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Traer_Planes");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Plan plan = new Plan();
                    plan.IdPlan = Convert.ToInt32(datos.Lector["IdPlanes"]);
                    plan.NombrePlan = (string)datos.Lector["Nombre"];
                    plan.PrecioPlan = Convert.ToSingle(datos.Lector["PrecioMensual"]);
                    plan.DuracionDiasPlan = Convert.ToInt32(datos.Lector["DuracionDias"]);
                    listaPlanes.Add(plan);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaPlanes;
        }
        //
        //grupo eliminar
        public void eliminarGrupoMuscular(GrupoMuscular grupo) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Eliminar_Grupo_Muscular");
                datos.setearParametro("@IdGrupoMuscular", grupo.IdGrupoMuscular);
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
        public void eliminarEjercicio(Ejercicio ejercicio) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Eliminar_Ejercicio");
                datos.setearParametro("@IdEjercicio", ejercicio.IdEjercicio);
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
        public void eliminarPlan(Plan plan) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Eliminar_Plan");
                datos.setearParametro("@IdPlan", plan.IdPlan);
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
        //
        //grupo modificar
        public void modificarGrupoMuscular(GrupoMuscular grupo) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Modificar_Grupo_Muscular");
                datos.setearParametro("@IdGrupoMuscular", grupo.IdGrupoMuscular);
                datos.setearParametro("@Nombre", grupo.NombreGrupoMuscular);
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
        public void modificarEjercicio(Ejercicio ejercicio) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Modificar_Ejercicio");
                datos.setearParametro("@IdEjercicio", ejercicio.IdEjercicio);
                datos.setearParametro("@Nombre", ejercicio.NombreEjercicio);
                datos.setearParametro("@IdGrupoMuscular", ejercicio.GrupoMuscular.IdGrupoMuscular);
                datos.setearParametro("@LinkExplicacion", ejercicio.LinkExplicacion);
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
        public void modificarPlan(Plan plan) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Modificar_Plan");
                datos.setearParametro("@IdPlan", plan.IdPlan);
                datos.setearParametro("@Nombre", plan.NombrePlan);
                datos.setearParametro("@PrecioMensual", plan.PrecioPlan);
                datos.setearParametro("@DuracionDias", plan.DuracionDiasPlan);
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
        //
        //grupo crear
        public void crearGrupoMuscular(GrupoMuscular grupo) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Crear_Grupo_Muscular");
                datos.setearParametro("@Nombre", grupo.NombreGrupoMuscular);
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
        public void crearEjercicio(Ejercicio ejercicio) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Crear_Ejercicio");
                datos.setearParametro("@Nombre", ejercicio.NombreEjercicio);
                datos.setearParametro("@IdGrupoMuscular", ejercicio.GrupoMuscular.IdGrupoMuscular);
                datos.setearParametro("@LinkExplicacion", ejercicio.LinkExplicacion);
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
        public void crearPlan(Plan plan) 
        {
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP("sp_Crear_Plan");
                datos.setearParametro("@Nombre", plan.NombrePlan);
                datos.setearParametro("@PrecioMensual", plan.PrecioPlan);
                datos.setearParametro("@DuracionDias", plan.DuracionDiasPlan);
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
    }
}