using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public abstract class Credenciales
    {
        public Usuario Usuario { get; set; }
        public string Contrasenia { get; set; }
        public DateTime UltimaModificacion { get; set; }
    }
}
