using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class SesionEntrenamiento
    {
        public int IdSesion { get; set; } = 0;
        public Cliente Cliente { get; set; }
        public Rutina Rutina { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public int CantidadSeries { get; set; }
    }
}
