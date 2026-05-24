using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    //Esta clase representa el tipo de suscripción que un usuario puede tener, con un identificador y una descripción de la suscripción.
    public class Suscripcion
    {
        public int IdSuscripcion{ get; set; } // 1 o 0
        public string DescripcionSuscripcion{ get; set; }
    }
}
