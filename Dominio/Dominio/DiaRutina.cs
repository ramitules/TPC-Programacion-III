using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Representa un día específico de la rutina, que contiene una lista de rutinas
    /// </summary>
    public class DiaRutina  
    {
        public string DiaDeRutina { get; set; }
        public List<Rutina> Rutinas { get; set; }
        public bool TieneRutinas { get { return Rutinas != null && Rutinas.Count > 0; } }
    }
}
