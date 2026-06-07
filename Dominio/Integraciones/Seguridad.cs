using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integraciones
{
    public abstract class Seguridad
    {
        public static bool SessionActiva(object user)
        {
            if (user is null) return false;

            try
            {
                if (((Usuario)user).IdUsuario != 0) 
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
