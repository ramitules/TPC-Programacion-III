
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using AccesoDB;

//Mejorar la consulta

namespace Negocio
{
    public class EjerciciosNegocio
    {
        public List<Ejercicio> ListarEjercicios()
        {
            AccesoADatos datos = new AccesoADatos();
            List<Ejercicio> listaEjercicios = new List<Ejercicio>();

            string excepcion = "Ocurrio un error al listar los ejercicios contra la base de datos\n\n(EjerciciosNegocio.ListarEjercicios()): ";

            try
            {
                datos.SetearConsulta("SELECT E.IdEjercicios, E.Nombre AS NombreEjercicio, E.IdGrupoMuscular, GM.Nombre AS NombreGM FROM Ejercicios E LEFT JOIN GruposMusculares GM ON E.IdGrupoMuscular = GM.IdGruposMusculares");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Ejercicio ejercicio = new Ejercicio();

                    ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicios"].ToString());
                    ejercicio.NombreEjercicio = datos.Lector["NombreEjercicio"].ToString();
                    ejercicio.GrupoMuscular.IdGrupoMuscular = int.Parse(datos.Lector["IdGrupoMuscular"].ToString());
                    ejercicio.GrupoMuscular.NombreGrupoMuscular = datos.Lector["NombreGM"].ToString();

                    listaEjercicios.Add(ejercicio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
            return listaEjercicios;
        }
    }
}