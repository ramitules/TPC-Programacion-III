using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Admin : Gestor
    {
        public Admin()
        {
            this.Rol = new RolUsuario();
            Rol.IdRol = (int)Roles.ADMIN;
        }
        //Ejercicios
        public void crearEjercicio() { }
        public void eliminarEjercicio() { }
        //Grupos musculares
        public void crearGrupoMuscular() { }
        public void eliminarGrupoMuscular() { }
        //Rutinas
        public void crearRutina() { }
        public void modificarRutina() { }
        public void eliminarRutina() { }
        //Planes
        public void crearPlan() { }
        public void modificarPlan() { }
        public void eliminarPlan() { }
        //Recepcionistas
        public void crearRecepcionista() { }
        public void modificarRecepcionista() { }
        public void eliminarRecepcionista() { }
        //
        public void crearEntrenador() { }
        public void modificarEntrenador() { }
        public void eliminarEntrenador() { }
        public void cancelarSuscripcionCliente() { }
    }
}
