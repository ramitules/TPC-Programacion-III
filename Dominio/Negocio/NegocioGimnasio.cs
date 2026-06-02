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
                    int rol = Convert.ToInt32(datos.Lector["IdRol"]);
                    Usuario us;
                    if (rol == 3)
                    {
                        Cliente cl = new Cliente();
                        cl.PesoCorporal = (decimal)datos.Lector["PesoCorporalKG"];
                        us = cl;
                    }
                    else if (rol == 1)
                    {
                        us = new Admin();
                    }
                    else if (rol == 2)
                    {
                        us = new Profesor();
                    }
                    else
                    {
                        us = new Administrativo();
                    }
                    us.IdUsuario = (int)datos.Lector["IdUsuario"];
                    us.Nombre = (string)datos.Lector["Nombre"];
                    us.Apellido = (string)datos.Lector["Apellido"];
                    us.Email = (string)datos.Lector["Email"];
                    us.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    us.Rol = Convert.ToInt32(datos.Lector["IdRol"]);
                    us.FechaIngreso = (DateTime)datos.Lector["FechaIngreso"];
                    lista.Add(us);
                }
                return lista;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al listar los usuarios: (modulo Negocio listarTodosLosUsuarios)" + e.Message);
                return lista;
            }
        }
    }
}
