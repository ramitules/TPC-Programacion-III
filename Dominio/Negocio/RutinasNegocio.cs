using AccesoDB;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class RutinasNegocio
    {
        /// <summary>
        /// Obtiene las rutinas asignadas a un solo usuario especifico
        /// </summary>
        /// <param name="id"> Id de Usuario</param>
        public List<Rutina> GetRutinasUsuario(string id)
        {
            string Excepcion = "Ocurrio un error al obtener rutinas (RutinasNegocio.GetRutinasCliente())\n";

            AccesoADatos datos = new AccesoADatos();
            List<Rutina> rutinas = new List<Rutina>();

            try
            {
                datos.SetearConsulta("SELECT * FROM Rutinas WHERE IdUsuario = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    if (!bool.Parse(datos.Lector["Activo"].ToString())) continue;

                    rutinas.Add(new Rutina(
                        idRutina: int.Parse(datos.Lector["IdRutinas"].ToString()),
                        nombre: datos.Lector["Nombre"].ToString(),
                        idUsuario: int.Parse(id),
                        fechaCreacion: DateTime.Parse(datos.Lector["FechaCreacion"].ToString()),
                        dia: datos.Lector["Dia"].ToString(),
                        activo: true
                    ));
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

            return rutinas;
        }
    }
}
