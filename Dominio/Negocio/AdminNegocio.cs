using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDB;
using Dominio;

namespace Negocio
{
    public class AdminNegocio
    {
        //Este bloque es para traer una lista de cada tipo de rol
        public List<Cliente> ObtenerClientes()
        {
            // Lógica para obtener la lista de clientes desde la base de datos
            return new List<Cliente>();
        }
        public List<Entrenador> ObtenerEntrenadores()
        {
            // Lógica para obtener la lista de entrenadores desde la base de datos
            return new List<Entrenador>();
        }
        public List<Recepcionista> ObtenerRecepcionistas()
        {
            // Lógica para obtener la lista de recepcionistas desde la base de datos
            return new List<Recepcionista>();
        }
        //Este bloque es para gestionar cada tipo de Rol
        //Métodos para administrar clientes
        public void activarCancelarSuscripcionCliente(Cliente cliente) { }//funcion compartida con recepcionista (30 dias de suscripcion en forzado)
        public void cancelarSuscripcionCliente(Cliente cliente) { }
    }



}
