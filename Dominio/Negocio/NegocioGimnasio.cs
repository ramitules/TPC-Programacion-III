using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDB;
using Dominio;

namespace Negocio
{
    public class NegocioGimnasio
    {
        public NegocioGimnasio() 
        { 
            

        }
       
        public List<Usuario> listarTodosLosUsuarios(string sp)
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP(sp);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Roles rol = (Roles)Convert.ToInt32(datos.Lector["IdRol"]);
                    Usuario us;
                    switch (rol)
                    {
                        case Roles.CLIENTE:
                            Cliente cl = new Cliente();
                            cl.PesoCorporal = (float)datos.Lector["PesoCorporalKG"];
                            us = cl;
                            break;
                        case Roles.ADMIN:
                            us = new Admin();
                            break;
                        case Roles.RECEPCIONISTA:
                            us = new Recepcionista();
                            break;
                        case Roles.ENTRENADOR:
                            us = new Entrenador();
                            break;
                        default:
                            throw new Exception("Rol no reconocido");
                    }
                    us.IdUsuario = (int)datos.Lector["IdUsuario"];
                    us.Nombre = (string)datos.Lector["Nombre"];
                    us.Apellido = (string)datos.Lector["Apellido"];
                    us.Email = (string)datos.Lector["Email"];
                    us.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    us.Rol.RolDescripcion = (string)datos.Lector["Nombre"];
                    us.FechaIngreso = (DateTime)datos.Lector["FechaIngreso"];
                    lista.Add(us);
                }
                return lista;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al listar los usuarios: (modulo Negocio listarTodosLosUsuarios)" + e.Message); //Esto hay que cambiarlo por un mensaje de error en la interfaz grafica al momento de presentar la app
                return lista;
            }
        }
    }
}
