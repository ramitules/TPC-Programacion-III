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
        }
        public int IdRutina { get; set; }
        public List<DiaRutina> listaDeRutinas = new List<DiaRutina>();
        public bool Activo { get; set; }
    }
}
