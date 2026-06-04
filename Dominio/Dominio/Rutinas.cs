using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa la rutina completa de un usuario, que contiene una lista de días de rutina, cada uno con sus ejercicios asignados.
    public class Rutina
    {
        public Rutina() 
        {
            IdRutina = 0;
            Activo = true;
        }
        public Rutina(int idRutina, string nombre, int idUsuario, DateTime fechaCreacion, string dia, bool activo)
        {
            IdRutina = idRutina;
            Nombre = nombre;
            IdUsuario = idUsuario;
            FechaCreacion = fechaCreacion;
            Dia = dia;
            Activo = activo;
        }

        public int IdRutina { get; set; }
        public string Nombre { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Dia { get; set; }
        public bool Activo { get; set; }
    }
}
