using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa un día específico de la rutina, que contiene una lista de ejercicios asignados para ese día.
    public class DiaRutina  
    {
        public Dia DiaDeRutina { get; set; }
        public List<Ejercicio> rutina = new List<Ejercicio>();
    }
}
