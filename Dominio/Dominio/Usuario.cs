using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public enum Roles
    {
        ADMIN = 1,
        RECEPCIONISTA = 2,
        CLIENTE = 3,
        ENTRENADOR = 4
    }
    /// <summary>
    /// Clase abstracta con propiedades base del Usuario
    /// </summary>
    public abstract class Usuario
    {
        public Usuario()
        {
            IdUsuario = 0;
            Activo = true;
        }
        public int IdUsuario { get; set; }
        public RolUsuario Rol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Activo { get; set; }
    }
}
