using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Representa un plan de suscripción que un usuario puede elegir,
    /// con detalles como el nombre del plan, su precio y la duración en días.
    /// </summary>
    public class Plan
    {
        public Plan() { IdPlan = 0; }
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; }
        public float PrecioPlan { get; set; }
        public int DuracionDiasPlan { get; set; }
        public bool Activo { get; set; }
    }
}
