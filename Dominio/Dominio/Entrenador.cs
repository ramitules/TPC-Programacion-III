using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Entrenador : Usuario
    {
        public Entrenador()
        {
            this.Rol = new RolUsuario();
            Rol.IdRol = (int)Roles.ENTRENADOR;
        }

        public void crearRutinaEstandar() { }
        public void crearRutinaParaCliente() { }
        public void modificarRutina() { }
        public void eliminarRutina() { }
        public void asignarRutina() { }
        public List<Ejercicio> listarEjercicioConFiltro(string filtro) { return new List<Ejercicio>();}
        public List<Rutina> listarRutinas() { return new List<Rutina>();}
        public Cliente buscarCliente() { return new Cliente(); }//Aca entran la de ver perfil una vez que encuentra el cliente
        public List<Rutina> buscarRutinasRealizadas() { return new List<Rutina>();}
    }
}
