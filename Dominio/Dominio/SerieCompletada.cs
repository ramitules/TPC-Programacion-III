using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class SerieCompletada
    {
        public int IdSerieCompletada { get; set; }
        public SesionEntrenamiento Sesion { get; set; }
        public Ejercicio Ejercicio { get; set; }
        public float PesoLevantadoKG { get; set; }
        public int RepeticionesLogradas { get; set; }
        public int RIR { get; set; }
        public bool EsRecordPersonal { get; set; }
    }
}
