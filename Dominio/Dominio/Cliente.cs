using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //clase que representa a un cliente, que hereda de la clase Usuario y tiene propiedades específicas relacionadas con su peso corporal, suscripción, plan y rutinas.
    public class Cliente : Usuario
    {
        public decimal PesoCorporal { get; set; }
        public Suscripcion SuscripcionCliente { get; set; }
        public Plan PlanCliente { get; set; }
        public Rutinas RutinasCliente { get; set; }

        public int MyProperty { get; set; }

    }
}
