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

        // Tarjeta de credito/debito (validacion de formato)

        private static string SoloDigitos(string valor)
        {
            return string.IsNullOrEmpty(valor)
                ? string.Empty
                : System.Text.RegularExpressions.Regex.Replace(valor, @"\D", "");
        }

        public static bool validarNumeroTarjeta(string numero)
        {
            return SoloDigitos(numero).Length == 16;
        }

        public static bool validarVencimientoTarjeta(string vencimiento)
        {
            if (string.IsNullOrWhiteSpace(vencimiento))
                return false;

            var match = System.Text.RegularExpressions.Regex.Match(vencimiento.Trim(), @"^(\d{2})/(\d{2})$");
            if (!match.Success)
                return false;

            int mes = int.Parse(match.Groups[1].Value);
            int anio = 2000 + int.Parse(match.Groups[2].Value);

            if (mes < 1 || mes > 12)
                return false;

            // No vencida: el primer dia del mes siguiente debe ser posterior a hoy.
            DateTime finDeVigencia = new DateTime(anio, mes, 1).AddMonths(1);
            return finDeVigencia > DateTime.Now;
        }

        public static bool validarCvv(string cvv)
        {
            int largo = SoloDigitos(cvv).Length;
            return largo == 3 || largo == 4;
        }

        public const string PrefijoLinkEjercicio = "https://www.simplyfitness.com/es/pages/";

        public static bool validarLinkEjercicio(string link)
        {
            return !string.IsNullOrWhiteSpace(link) && link.StartsWith(PrefijoLinkEjercicio);
        }

        /// <summary>
        /// El rango es valido si "Desde" y "Hasta" estan vacios, o "Desde" es menor o igual a "Hasta".
        /// Tambien valida que ninguna de las dos fechas sea superior al dia de hoy.
        /// </summary>
        public static bool rangoFechasValido(string Desde, string Hasta)
        {
            bool hayDesde = DateTime.TryParse(Desde, out DateTime desde);
            bool hayHasta = DateTime.TryParse(Hasta, out DateTime hasta);

            // Ninguna fecha puede ser posterior al dia de hoy.
            if (hayDesde && desde.Date > DateTime.Today)
                return false;
            if (hayHasta && hasta.Date > DateTime.Today)
                return false;

            // Si falta alguna fecha no hay rango que comparar: se considera valido.
            if (!hayDesde || !hayHasta)
                return true;

            return desde.Date <= hasta.Date;
        }
    }
}
