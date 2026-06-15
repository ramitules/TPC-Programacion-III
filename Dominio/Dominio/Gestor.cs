using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Gestor: Usuario
    {
        public List<Cliente> ObtenerClientes()
        {
            // Lógica para obtener la lista de clientes desde la base de datos
            return new List<Cliente>();
        }
    }
}
