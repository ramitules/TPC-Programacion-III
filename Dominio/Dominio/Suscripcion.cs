using System;

namespace Dominio
{
    public class Suscripcion
    {
        public int IdSuscripcion { get; set; }
        public Cliente Cliente { get; set; }
        public Plan Plan { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa { get; set; } // Indica si la suscripción está activa o no.
    }
}
