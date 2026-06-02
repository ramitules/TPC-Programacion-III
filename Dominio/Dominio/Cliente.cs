using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Clase que representa a un cliente, que hereda de la clase Usuario y tiene 
    /// propiedades específicas relacionadas con su peso corporal, suscripción, 
    /// plan y rutinas.
    /// </summary>
    public class Cliente : Usuario
    {
        public Cliente()
        {
            Rol = Roles.CLIENTE;
        }
        public decimal PesoCorporal { get; set; }
        public Suscripcion SuscripcionCliente { get; set; }
        public Rutina RutinaCliente { get; set; }
        public List<Records> RecordsPersonales = new List<Records>();
    }
}
