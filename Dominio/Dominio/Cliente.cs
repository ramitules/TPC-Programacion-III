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
            this.Rol = new RolUsuario();
            Rol.IdRol = (int)Roles.CLIENTE;
            SuscripcionCliente = null;
            RecordsPersonales = new List<Records>();
        }
        public float PesoCorporal { get; set; }
        public Suscripcion SuscripcionCliente { get; set; }
        public List<Records> RecordsPersonales { get; set; }

        public List<Plan> elegirPlan() { return new List<Plan>(); }
        //Ver perfil mas que una funcion, seria una ventana donde se muetren los datos del cliente
        public void modificarPerfil() { }
        public List<Rutina> seleccionarRutinaGeneral() { return new List<Rutina>(); }
        public List<Rutina> seleccionarRutinasAsignadas() { return new List<Rutina>(); }
        public void iniciarRutinaAsignada() { }
        public void registrarRutinaRealizada() { }
        //Listar sesiones realizadas => falta crear clase
        public List<Records> verRecordsPersonales() { return new List<Records>(); }
    }
}
