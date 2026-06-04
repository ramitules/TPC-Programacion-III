using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum EstadoSuscripcion
    {
        ACTIVA = 1,
        VENCIDA = 2,
        CANCELADA = 3
    }
    public class Suscripcion
    {
        public Suscripcion()
        {
            IdSuscripcion = 0;
        }
        public int IdSuscripcion { get; set; }
        public Plan Plan { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public EstadoSuscripcion Estado { get; set; }
    }
}
