using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    /// <summary>
    /// Representa un grupo muscular específico, que puede ser utilizado
    /// para categorizar los ejercicios en la rutina de un usuario.
    /// </summary>
    public class GrupoMuscular
    {
        public GrupoMuscular() { IdGrupoMuscular = 0; }
        public int IdGrupoMuscular { get; set; }
        public string NombreGrupoMuscular { get; set; }
        public bool Activo { get; set; }
    }
}
