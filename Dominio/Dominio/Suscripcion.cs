using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Suscripcion
    {
        public Suscripcion()
        {
            IdSuscripcion = 0;
            Activa = false;
        }
        public int IdSuscripcion { get; set; }
        public Plan Plan { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa { get; set; }
    }
}
