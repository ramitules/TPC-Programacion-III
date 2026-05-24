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

        public List<Cliente> listarTodosLosUsuarios(string sp)
        {
            List<Cliente> lista = new List<Cliente>();
            AccesoADatos datos = new AccesoADatos();
            try
            {
                datos.SetearConsultaSP(sp);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Cliente cl = new Cliente();
                    cl.IdUsuario = (int)datos.Lector["ID"];
                    cl.Nombre = (string)datos.Lector["NombreCompleto"];
                    cl.Email = (string)datos.Lector["Email"];
                    cl.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    cl.PesoCorporal = (decimal)datos.Lector["PesoCorporalKG"];
                    cl.IdRol = Convert.ToInt32(datos.Lector["IdRol"]);
                    cl.FechaIngreso = (DateTime)datos.Lector["FechaIngreso"];
                    lista.Add(cl);
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
