using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Gestor: Usuario
    {
        public Cliente buscarCliente() { return new Cliente(); }
        public List<Cliente> clienteSuscripcionPorVencer() { return new List<Cliente>(); }
        public void agregarSuscripcion() { }
        public List<Plan> verPlanes() { return new List<Plan>(); }
    }
}
