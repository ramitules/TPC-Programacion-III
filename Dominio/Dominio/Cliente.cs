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
        public Cliente() { Rol = "Cliente"; }
        public decimal PesoCorporal { get; set; }
        public Suscripcion SuscripcionCliente { get; set; }
        public Rutinas RutinasCliente { get; set; } // ¿Esto seria la rutina activa a realizar...?
        public int MyProperty { get; set; } // ¿Esto que funcion cumple...?

    }
}
