using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integraciones
{
    public static class Validaciones
    {
        public static bool validarEmail(string email)
        {
            return !(string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email));
        }

        public static bool validarContrasenias(string pass1, string pass2)
        {
            return pass1 == pass2;
        }

        public static bool validarFechaNacimiento(string fecha, int minimoAnios = 12)
        {
            DateTime ahora = DateTime.Today;

            if (!DateTime.TryParse(fecha, out DateTime fechaNacimiento))
                return false;

            int edad = ahora.Year - fechaNacimiento.Year;

            if (fechaNacimiento.Date > ahora.AddYears(-edad))
                edad--;

            return edad >= minimoAnios;
        }
        
        public static bool validarNombre(string nombre)
        {
            return !string.IsNullOrWhiteSpace(nombre);
        }

        public const string PrefijoLinkEjercicio = "https://www.simplyfitness.com/es/pages/";

        public static bool validarLinkEjercicio(string link)
        {
            return !string.IsNullOrWhiteSpace(link) && link.StartsWith(PrefijoLinkEjercicio);
        }
    }
}
