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
            SuscripcionCliente = null;
            RutinasCliente = new List<Rutina>();
            RecordsPersonales = new List<Records>();
        }
        public float PesoCorporal { get; set; }
        public Suscripcion SuscripcionCliente { get; set; }
        public List<Rutina> RutinasCliente { get; set; }
        public List<Records> RecordsPersonales { get; set; }
    }
}
