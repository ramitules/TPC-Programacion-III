using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Administrativo : Usuario
    {
        public Administrativo(bool esAdmin)
        {
            Rol = esAdmin ? Roles.ADMIN : Roles.ADMINISTRATIVO;
        }
    }
}
