using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Recepcionista : Gestor
    {
        public Recepcionista()
        {
            this.Rol = new RolUsuario();
            Rol.IdRol = (int)Roles.RECEPCIONISTA;
        }
    }
}
