using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Representa la relación entre una rutina y un ejercicio específico, 
    /// incluyendo detalles como el número de series, repeticiones y 
    /// el orden del ejercicio dentro de la rutina (1 = primer ejercicio, etc.)
    /// </summary>
    public class RutinaEjercicio
    {
        public RutinaEjercicio() { IdRutinaEjercicio = 0; }
        public int IdRutinaEjercicio { get; set; }
        public Ejercicio Ejercicio { get; set; }
        public int ObjetivoSeries { get; set; }
        public int ObjetivoRepeticiones { get; set; }
        public int OrdenEjercicio { get; set; }
    }
}
