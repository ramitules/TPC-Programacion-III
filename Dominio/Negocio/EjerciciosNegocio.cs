
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using AccesoDB;
using Integraciones;

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
                datos.SetearConsulta("SELECT E.IdEjercicios, E.Nombre AS NombreEjercicio, E.IdGrupoMuscular, E.LinkExplicacion, GM.Nombre AS NombreGM FROM Ejercicios E LEFT JOIN GruposMusculares GM ON E.IdGrupoMuscular = GM.IdGruposMusculares");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Ejercicio ejercicio = new Ejercicio();

                    ejercicio.IdEjercicio = int.Parse(datos.Lector["IdEjercicios"].ToString());
                    ejercicio.NombreEjercicio = datos.Lector["NombreEjercicio"].ToString();
                    ejercicio.GrupoMuscular.IdGrupoMuscular = int.Parse(datos.Lector["IdGrupoMuscular"].ToString());
                    ejercicio.GrupoMuscular.NombreGrupoMuscular = datos.Lector["NombreGM"].ToString();
                    ejercicio.LinkExplicacion = datos.Lector["LinkExplicacion"].ToString();

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

        /// <summary>
        /// Agrega un nuevo ejercicio. Valida que el link de explicacion comience con el
        /// prefijo permitido antes de insertar.
        /// </summary>
        public void Agregar(Ejercicio ejercicio)
        {
            AccesoADatos datos = new AccesoADatos();

            string excepcion = "Ocurrio un error al agregar el ejercicio\n\n(EjerciciosNegocio.Agregar()): ";

            if (!Validaciones.validarLinkEjercicio(ejercicio.LinkExplicacion))
                throw new Exception($"El link del ejercicio debe comenzar con '{Validaciones.PrefijoLinkEjercicio}'.");

            try
            {
                datos.SetearConsulta("INSERT INTO Ejercicios (Nombre, IdGrupoMuscular, LinkExplicacion) VALUES (@Nombre, @IdGrupoMuscular, @LinkExplicacion)");
                datos.setearParametro("@Nombre", ejercicio.NombreEjercicio);
                datos.setearParametro("@IdGrupoMuscular", ejercicio.GrupoMuscular.IdGrupoMuscular);
                datos.setearParametro("@LinkExplicacion", ejercicio.LinkExplicacion);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception(excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        /// <summary>
        /// Modifica un ejercicio existente. Valida que el link de explicacion comience con el
        /// prefijo permitido antes de actualizar.
        /// </summary>
        public void Modificar(Ejercicio ejercicio)
        {
            AccesoADatos datos = new AccesoADatos();

            string excepcion = "Ocurrio un error al modificar el ejercicio\n\n(EjerciciosNegocio.Modificar()): ";

            if (!Validaciones.validarLinkEjercicio(ejercicio.LinkExplicacion))
                throw new Exception($"El link del ejercicio debe comenzar con '{Validaciones.PrefijoLinkEjercicio}'.");

            try
            {
                datos.SetearConsulta("UPDATE Ejercicios SET Nombre = @Nombre, IdGrupoMuscular = @IdGrupoMuscular, LinkExplicacion = @LinkExplicacion WHERE IdEjercicios = @IdEjercicio");
                datos.setearParametro("@Nombre", ejercicio.NombreEjercicio);
                datos.setearParametro("@IdGrupoMuscular", ejercicio.GrupoMuscular.IdGrupoMuscular);
                datos.setearParametro("@LinkExplicacion", ejercicio.LinkExplicacion);
                datos.setearParametro("@IdEjercicio", ejercicio.IdEjercicio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception(excepcion + ex.ToString());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
