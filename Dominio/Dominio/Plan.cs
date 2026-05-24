using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa un plan de suscripción que un usuario puede elegir, con detalles como el nombre del plan, su precio y la duración en días.
    public class Plan
    {
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; }
        public decimal PrecioPlan { get; set; }
        public int DuracionDiasPlan { get; set; }
    }
}
