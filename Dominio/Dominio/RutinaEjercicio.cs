using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa la relación entre una rutina y un ejercicio específico, incluyendo detalles como el número de series, repeticiones y el orden del ejercicio dentro de la rutina.
    public class RutinaEjercicio
    {
        public int IdRutinaEjercicio { get; set; }
        public int IdEjercicio { get; set; }
        public int? ObjetivoSeries { get; set; }
        public int? ObjetivoRepeticiones { get; set; }
        public int? OrdenEjercicio { get; set; }

        //
    }
}
