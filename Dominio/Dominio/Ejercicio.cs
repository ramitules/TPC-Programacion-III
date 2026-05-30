using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Representa un ejercicio específico, que tiene un identificador, 
    /// un nombre y un grupo muscular al que pertenece.
    /// </summary>
    public class Ejercicio
    {
        public Ejercicio()
        {
            IdEjercicio = 0;
        }
        public int IdEjercicio { get; set; }
        public string NombreEjercicio { get; set; }
        public GrupoMuscular GrupoMuscular { get; set; }    
    }
}
