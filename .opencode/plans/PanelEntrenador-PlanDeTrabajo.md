# Panel de Entrenador - Plan de Trabajo

## Proyecto: GimnasioApp

**Stack:** ASP.NET Web Forms (.NET Framework 4.8), C#, SQL Server, arquitectura N-Layer  
**Ubicación:** `C:\Home\Dev\csharp\programacion III\GimnasioApp`

---

## Estado Actual del Panel de Entrenador

### Funcionalidades Completadas:

| # | Funcionalidad | Estado | Archivos |
|---|---------------|--------|----------|
| 1 | **Ver lista de socios** (solo activos, datos restringidos) | ✅ Completado | `PanelEntrenador.aspx`, `PanelEntrenador.aspx.cs` |
| 2 | **Asignar rutinas a socios** (crear rutina nueva y asignarla) | ✅ Completado | `PanelEntrenadorAsignarRutina.aspx`, `PanelEntrenadorAsignarRutina.aspx.cs`, `RutinasNegocio.cs` |
| 3 | **Ver detalle del cliente** (datos + rutinas asignadas) | ✅ Completado | `PanelEntrenadorClienteDetalle.aspx`, `PanelEntrenadorClienteDetalle.aspx.cs` |
| 4 | **Ver sesiones/progreso** de los socios | ✅ Completado | `PanelEntrenadorProgresoCliente.aspx`, `PanelEntrenadorProgresoCliente.aspx.cs`, `PanelEntrenadorProgresoSesionDetalle.aspx`, `PanelEntrenadorProgresoSesionDetalle.aspx.cs` |
| 5 | **Ver rutinas asignadas** a clientes | ✅ Completado | `PanelEntrenadorRutinasAsignadas.aspx`, `PanelEntrenadorRutinasAsignadas.aspx.cs`, `RutinasNegocio.cs` |
| 6 | **Ver detalle de rutina** (lectura + usar como plantilla) | 🔄 En Progreso | `PanelEntrenadorDetalleRutina.aspx`, `PanelEntrenadorDetalleRutina.aspx.cs` |

### Funcionalidades Pendientes:

| # | Funcionalidad | Prioridad | Estado |
|---|---------------|-----------|--------|
| 7 | **Ver su propio perfil** | Baja | Pendiente |

### Tareas Pendientes Menores:

| Tarea | Descripción |
|-------|-------------|
| Implementar `btnQuitar_Click` | En `PanelEntrenadorAsignarRutina.aspx.cs` (tiene comentario PENDIENTE) |

---

## Archivos Creados/Modificados en Esta Sesión

### Archivos NUEVOS:
1. **`PanelEntrenadorAsignarRutina.aspx`** — Vista para crear y asignar rutina a un socio
2. **`PanelEntrenadorAsignarRutina.aspx.cs`** — Code-behind con Page_Load, btnAgregar_Click, btnQuitar_Click (PENDIENTE), btnGuardar_Click
3. **`PanelEntrenadorClienteDetalle.aspx`** — Vista con datos del cliente y rutinas asignadas
4. **`PanelEntrenadorClienteDetalle.aspx.cs`** — Code-behind con Page_Load, CalcularEdad, btnCrearRutina_Click
5. **`PanelEntrenadorProgresoCliente.aspx`** — Vista con GridView de sesiones del cliente (fecha, rutina, duración, series)
6. **`PanelEntrenadorProgresoCliente.aspx.cs`** — Code-behind con Page_Load (busca cliente en Session, carga sesiones de BD), gvSesiones_RowDataBound, CalcularDuracion, btnVolver_Click
7. **`PanelEntrenadorProgresoSesionDetalle.aspx`** — Vista con GridView de series de una sesión (ejercicio, peso, reps, RIR, record)
8. **`PanelEntrenadorProgresoSesionDetalle.aspx.cs`** — Code-behind con Page_Load (carga series agrupadas), btnVolver_Click
9. **`PanelEntrenadorRutinasAsignadas.aspx`** — Vista con GridView de rutinas asignadas a clientes (nombre, cliente, día, fecha)
10. **`PanelEntrenadorRutinasAsignadas.aspx.cs`** — Code-behind con Page_Load simple (carga de BD)
11. **`PanelEntrenadorDetalleRutina.aspx`** — Vista de detalle de rutina (solo lectura) con ejercicios, botones "Usar como plantilla" y "Asignar a cliente"
12. **`PanelEntrenadorDetalleRutina.aspx.cs`** — Code-behind con Page_Load (carga rutina y ejercicios), btnVolver_Click, btnUsarPlantilla_Click, btnAsignarCliente_Click

### Archivos MODIFICADOS:
1. **`PanelEntrenador.aspx`** — Agregado GridView con columnas (Nombre, Apellido, Edad, Peso, Fecha Ingreso, Acciones), link "Ver Detalle" apunta a PanelEntrenadorClienteDetalle.aspx
2. **`PanelEntrenador.aspx.cs`** — Agregado Page_Load, CalcularEdad, aplicarFiltros, btnBuscar_Click con try-catch. **Modificado para usar patrón Session** (verifica si Session tiene datos antes de cargar de BD)
3. **`RutinasNegocio.cs`** — Agregado método `CrearRutinaParaCliente(int idCliente, string nombre, List<RutinaEjercicio> ejercicios)` y `GetRutinasAsignadas()`
4. **`PanelEntrenadorClienteDetalle.aspx.cs`** — **Modificado para buscar cliente en Session** en vez de ir a BD. Agregado btnVerProgreso_Click
5. **`PanelEntrenadorClienteDetalle.aspx`** — Agregado botón "Ver Progreso" entre "Crear Rutina" y "Volver". Agregada columna "Ver" en GridView de rutinas
6. **`Login.aspx.cs`** — Agregado `case (int)Roles.ENTRENADOR` en el switch del login (EN-02)
7. **`Site.Master`** — Agregada sección ENTRENADOR en menú lateral + link a Login cuando no hay sesión
8. **8 páginas del panel** — Agregada validación de sesión/rol al inicio de Page_Load (EN-01): `PanelEntrenador`, `PanelEntrenadorCrearRutina`, `PanelEntrenadorRutinas`, `PanelEntrenadorAsignarRutina`, `PanelEntrenadorClienteDetalle`, `PanelEntrenadorProgresoCliente`, `PanelEntrenadorProgresoSesionDetalle`, `PanelEntrenadorRutinasAsignadas`
9. **`PanelEntrenadorRutinas.aspx`** — Agregada columna "Ver" con link a PanelEntrenadorDetalleRutina.aspx
10. **`PanelEntrenadorRutinasAsignadas.aspx`** — Agregada columna "Ver" con link a PanelEntrenadorDetalleRutina.aspx
11. **`PanelEntrenadorAsignarRutina.aspx`** — Agregado DropDownList para seleccionar cliente (cuando no viene idCliente por URL)
12. **`PanelEntrenadorAsignarRutina.aspx.cs`** — Modificado Page_Load para aceptar `idRutinaBase` por URL y precargar ejercicios. Modificado btnGuardar_Click para manejar cliente desde dropdown

---

## Flujo Actual de Navegación

```
PanelEntrenador.aspx (lista socios activos)
  ├─→ "Ver Detalle" → PanelEntrenadorClienteDetalle.aspx?idCliente=X
  │                     ├─→ Datos del cliente (nombre, apellido, edad, peso, fecha ingreso)
  │                     ├─→ Lista de rutinas asignadas (nombre, fecha creación, día)
  │                     │     └─→ "Ver" → PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorClienteDetalle&idCliente=X
  │                     │                   ├─→ GridView de ejercicios (solo lectura)
  │                     │                   ├─→ "Usar como plantilla" → PanelEntrenadorAsignarRutina.aspx?idCliente=X&idRutinaBase=X
  │                     │                   └─→ "Asignar a cliente" → PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X
  │                     ├─→ "Crear Rutina" → PanelEntrenadorAsignarRutina.aspx?idCliente=X
  │                     ├─→ "Ver Progreso" → PanelEntrenadorProgresoCliente.aspx?idCliente=X
  │                     │                     ├─→ GridView de sesiones (rutina, fecha, duración, series)
  │                     │                     ├─→ "Ver Detalle" → PanelEntrenadorProgresoSesionDetalle.aspx?idSesion=Z&idCliente=X
  │                     │                     │                     └─→ GridView de series (ejercicio, grupo muscular, peso, reps, RIR, record)
  │                     │                     └─→ "Volver" → PanelEntrenadorClienteDetalle.aspx?idCliente=X
  │                     └─→ "Volver" → PanelEntrenador.aspx
  │
  ├─→ "Crear Rutina" → PanelEntrenadorCrearRutina.aspx (crea rutina general, ya existía)
  ├─→ "Panel de Rutinas" → PanelEntrenadorRutinas.aspx (ver/eliminar generales)
  │                          └─→ "Ver" → PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorRutinas
  │                                        ├─→ GridView de ejercicios (solo lectura)
  │                                        ├─→ "Usar como plantilla" → PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X
  │                                        └─→ "Asignar a cliente" → PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X
  └─→ "Rutinas Asignadas" → PanelEntrenadorRutinasAsignadas.aspx (ver rutinas asignadas a clientes)
                              └─→ "Ver" → PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorRutinasAsignadas
                                            ├─→ GridView de ejercicios (solo lectura)
                                            ├─→ "Usar como plantilla" → PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X
                                            └─→ "Asignar a cliente" → PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X
```

---

## Base de Datos Local (Configurada)

La base de datos local ya está configurada en SQL Server Express:
- **Servidor:** `.\SQLEXPRESS` (DESKTOP-21C03Q4\SQLEXPRESS)
- **Base de datos:** `GestionGimnasio`
- **Connection string:** `Server=.\SQLEXPRESS;Database=GestionGimnasio;Integrated Security=True;TrustServerCertificate=True;`
- **Scripts ejecutados:** ScriptDB.sql, Vistas.sql, ProcedimientosAlmacenados.sql, triggers.sql, Inserts.sql
- **Datos de prueba:** 30 usuarios (23 clientes activos), ejercicios, rutinas, sesiones y series cargados

---

## Patrón Session (Consistente con Clientes/Admins)

PanelEntrenador usa el mismo patrón de Session que las secciones de Clientes y Admins del proyecto:

### Cómo funciona:

| Paso | Qué hace | Cómo |
|------|----------|------|
| **Primera carga** | Carga lista de BD → guarda en Session | `Session.Add("listaClientesEntrenador", clientes)` |
| **Cargas siguientes** | Usa datos de Session (no va a BD) | `clientes = (List<Cliente>)Session["listaClientesEntrenador"]` |
| **Navegación a detalle** | Pasa ID por QueryString | `?idCliente=8` |
| **Búsqueda en detalle** | Busca en Session por ID | `clientes.Find(c => c.IdUsuario == int.Parse(idCliente))` |
| **Volver al listado** | Usa Session (no recarga BD) | Link estático o Response.Redirect |

### Claves de Session usadas:

| Clave | Qué guarda | Dónde se usa |
|-------|------------|--------------|
| `listaClientesEntrenador` | Lista completa de clientes activos | PanelEntrenador, PanelEntrenadorClienteDetalle, PanelEntrenadorProgresoCliente |
| `idClienteDetalle` | ID del cliente en detalle | PanelEntrenadorClienteDetalle |
| `idClienteProgreso` | ID del cliente en progreso | PanelEntrenadorProgresoCliente |
| `idClienteProgresoDetalle` | ID del cliente en detalle de sesión | PanelEntrenadorProgresoSesionDetalle |
| `idSesionDetalle` | ID de la sesión en detalle | PanelEntrenadorProgresoSesionDetalle |

### Nota importante:
Las sesiones de entrenamiento y las series se cargan siempre de BD (no se cachean en Session) porque son datos que cambian frecuentemente.

---

## Línea de Trabajo: Seguridad (Completada)

### Hallazgos de Auditoría Resueltos:

| ID | Severidad | Problema | Estado | Archivos |
|----|-----------|----------|--------|----------|
| **EN-01** | 🔴 Crítico | Ninguna página del panel valida sesión/rol | ✅ Resuelto | 8 archivos .aspx.cs del panel |
| **EN-02** | 🟠 Alto | Login no maneja rol ENTRENADOR | ✅ Resuelto | `Login.aspx.cs` |

### EN-01: Validación de sesión/rol en páginas del panel

**Solución implementada:** Agregar al inicio de cada `Page_Load`:
```csharp
if (!Seguridad.SessionActiva(Session["usuario"]) ||
    !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
{
    Response.Redirect("Default", false);
    return;
}
```

**Patrón usado:** Consistente con páginas de Cliente (Rutinas.aspx.cs, DetalleRutina.aspx.cs, perfil.aspx.cs).

**Archivos modificados:**
1. PanelEntrenador.aspx.cs
2. PanelEntrenadorCrearRutina.aspx.cs
3. PanelEntrenadorRutinas.aspx.cs
4. PanelEntrenadorAsignarRutina.aspx.cs
5. PanelEntrenadorClienteDetalle.aspx.cs
6. PanelEntrenadorProgresoCliente.aspx.cs
7. PanelEntrenadorProgresoSesionDetalle.aspx.cs
8. PanelEntrenadorRutinasAsignadas.aspx.cs

### EN-02: Login para entrenador

**Solución implementada:** Agregar case en Login.aspx.cs:
```csharp
case (int)Roles.ENTRENADOR:
    Session.Add("usuario", usuario);
    Response.Redirect("~/PanelEntrenador.aspx", false);
    break;
```

**Patrón usado:** Consistente con case ADMIN (simple, sin carga extra de datos).

### Site.Master: Menú lateral para entrenador

**Solución implementada:** Agregar sección ENTRENADOR en el menú lateral:
- Panel de Entrenador
- Rutinas Generales
- Rutinas Asignadas

**También:** Link "Iniciar sesión" cuando no hay usuario logueado.

---

## Funcionalidad en Progreso: Ver Detalle de Rutina

### Objetivo:
Que el entrenador pueda ver los ejercicios de cualquier rutina (generales o asignadas) y usarla como plantilla para crear/asignar nuevas rutinas.

### Archivos creados:
- **`PanelEntrenadorDetalleRutina.aspx`** — Vista de solo lectura con GridView de ejercicios + botones "Usar como plantilla" y "Asignar a cliente"
- **`PanelEntrenadorDetalleRutina.aspx.cs`** — Page_Load (carga rutina y ejercicios), btnVolver_Click, btnUsarPlantilla_Click, btnAsignarCliente_Click

### Archivos modificados:
- **`PanelEntrenadorClienteDetalle.aspx`** — Agregada columna "Ver" en GridView de rutinas → link a `PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorClienteDetalle&idCliente=X`
- **`PanelEntrenadorRutinas.aspx`** — Agregada columna "Ver" → link a `PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorRutinas`
- **`PanelEntrenadorRutinasAsignadas.aspx`** — Agregada columna "Ver" → link a `PanelEntrenadorDetalleRutina.aspx?id=X&origen=PanelEntrenadorRutinasAsignadas`
- **`PanelEntrenadorAsignarRutina.aspx`** — Agregado DropDownList `ddlClientes` para seleccionar cliente cuando no viene por URL
- **`PanelEntrenadorAsignarRutina.aspx.cs`** — Modificado Page_Load para aceptar `idRutinaBase` por URL y precargar ejercicios + nombre. Modificado btnGuardar_Click para manejar cliente desde dropdown

### Parámetros de URL:
- `id` → ID de la rutina a ver
- `origen` → Página de origen para el botón Volver (PanelEntrenadorClienteDetalle, PanelEntrenadorRutinas, PanelEntrenadorRutinasAsignadas)
- `idCliente` → Opcional, si viene desde el detalle de un cliente específico

### Flujo de "Usar como plantilla":
1. Entrenador ve detalle de una rutina
2. Clic en "Usar como plantilla"
3. Redirige a `PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X` (y `idCliente=X` si viene del detalle de un cliente)
4. La página precarga: nombre de rutina + ejercicios de la rutina base
5. Entrenador puede modificar y guardar

### Flujo de "Asignar a cliente":
1. Entrenador ve detalle de una rutina
2. Clic en "Asignar a cliente"
3. Redirige a `PanelEntrenadorAsignarRutina.aspx?idRutinaBase=X`
4. La página muestra dropdown de clientes + ejercicios precargados
5. Entrenador elige cliente, puede modificar y guardar

### Estado: Archivos creados, pendiente probar funcionalidad completa

### Nota importante:
El dropdown de clientes usa `Nombre` como DataTextField porque no se puede modificar la clase `Cliente` para agregar un `NombreCompleto`. Esto significa que si hay clientes con el mismo nombre, no se distinguen por apellido. Posible mejora futura: concatenar en el code-behind con un foreach.

---

## Próxima Tarea: Terminar Ver Detalle de Rutina + Ver Perfil Propio

### Tarea actual: Probar "Ver Detalle de Rutina"
Los archivos ya están creados pero falta probar que todo funcione:
- Navegar desde PanelEntrenadorClienteDetalle → Ver rutina → ver ejercicios
- Navegar desde PanelEntrenadorRutinas → Ver rutina → ver ejercicios
- Navegar desde PanelEntrenadorRutinasAsignadas → Ver rutina → ver ejercicios
- Probar botón "Usar como plantilla" (debe llevar a PanelEntrenadorAsignarRutina con ejercicios precargados)
- Probar botón "Asignar a cliente" (debe llevar a PanelEntrenadorAsignarRutina con ejercicios precargados y dropdown de clientes)
- Verificar que el dropdown de clientes muestre Nombre + Apellido

### Futuro: Ver Perfil Propio del Entrenador
- Qué datos mostrar
- Si se puede modificar o solo ver
- Qué patrón seguir (similar a perfil.aspx del cliente)

---

## Prompt para Retomar la Próxima Sesión

```
Necesito continuar con el desarrollo del panel de entrenador en GimnasioApp.

Leé primero el archivo .opencode/plans/PanelEntrenador-PlanDeTrabajo.md para tener todo el contexto.

Contexto:
- Proyecto ASP.NET Web Forms en C:\Home\Dev\csharp\programacion III\GimnasioApp
- Panel de entrenador con 5 funcionalidades completadas, 1 en progreso y 1 pendiente
- Completadas: ver lista de socios, asignar rutinas, ver detalle del cliente, ver sesiones/progreso, ver rutinas asignadas
- En progreso: ver detalle de rutina (archivos creados, falta probar)
- Pendientes: ver perfil propio
- Tareas menores: implementar btnQuitar_Click
- Seguridad completada: EN-01 (validación sesión/rol) y EN-02 (login entrenador) resueltos
- Site.Master actualizado con menú lateral para entrenador
- Base de datos local ya configurada en SQL Server Express (.\SQLEXPRESS)
- PanelEntrenador usa patrón Session consistente con Clientes/Admins

Lo que necesito hacer ahora:
- PRIMERO: Probar que funcione "Ver Detalle de Rutina" (navegación, botones "Usar como plantilla" y "Asignar a cliente")
- DESPUÉS: Implementar "Ver perfil propio" del entrenador

Referencias:
- Todo con try-catch (patrón inline: throw new Exception("mensaje", ex))
- Usar Session.Add() para guardar datos
- Mantener consistencia con el patrón Session ya implementado
- NO modificar clases del dominio (ej: no agregar propiedades a Cliente)

Soy estudiante, así que explicame claro y paso a paso lo que vamos haciendo. No hagas los cambios vos, explicame y yo los hago.
```

---

## Notas Importantes

- **Modo mentor:** El usuario es estudiante, necesita explicaciones claras y paso a paso. NO hacer los cambios, explicarlos para que el usuario los implemente.
- **Consistencia:** Basarse siempre en código existente del proyecto, no inventar patrones nuevos.
- **Try-catch:** Usar patrón inline `throw new Exception("mensaje (Archivo.Método())", ex)` en todos los métodos que interactúan con BD o Session.
- **Session:** Usar `Session.Add("clave", valor)` para guardar, `Session["clave"]` para leer.
- **Un cliente puede tener varias rutinas:** No validar "una sola activa" (puede tener rutina de mañana, tarde, diferentes días, etc.).
- **Comentarios PENDIENTE:** Los bloques marcados con `// PENDIENTE` son funcionalidades en desarrollo (para la auditoría de Claude).
- **Seguridad:** Todas las páginas del panel validan sesión/rol con patrón consistente con Cliente (SessionActiva + accesoYPermisos). Login maneja rol ENTRENADOR.
