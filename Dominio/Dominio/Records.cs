using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum Unidad
    {
        KILOS = 1,
        REPETICIONES = 2
    }
    public class Records
    {
        public Ejercicio Ejercicio { get; set; }
        public float Record { get; set; }
        public Unidad Unidad { get; set; }
    }
}
