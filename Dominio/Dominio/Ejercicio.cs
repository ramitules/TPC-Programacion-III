using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa un ejercicio específico, que tiene un identificador, un nombre y un grupo muscular al que pertenece.
    public class Ejercicio
    {
        public int IdEjercicio { get; set; }
        public string NombreEjercicio { get; set; }
        public GrupoMuscular GrupoMuscular { get; set; }    
    }
}
