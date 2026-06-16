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
            Ejercicios = new List<RutinaEjercicio>();
        }

        public int IdRutina { get; set; }
        public string Nombre { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Dia { get; set; }
        public bool Activo { get; set; }
        public List<RutinaEjercicio> Ejercicios { get; set; }
    }
}
