# Auditoría — Pilar Entrenador

> Funcionalidades objetivo: crear rutinas estándar; crear rutinas y asignarlas a un cliente;
> seleccionar/modificar/asignar rutinas; asignar rutinas a clientes; listar ejercicios con filtros;
> buscar cliente y asignarle rutina; ver rutinas realizadas del cliente; ver perfil del cliente.
> **Leer primero `00-Transversal.md`**.

## Leyenda de severidades
🔴 Crítico · 🟠 Alto · 🟡 Medio · ⚪ Bajo

## Mapa funcionalidad → estado

| Funcionalidad | Página / Negocio | Estado |
|---|---|---|
| Crear rutina estándar | `PanelEntrenadorCrearRutina.aspx` → `RutinasNegocio.CrearRutinaGeneral` | Implementado (con defectos) |
| Crear rutina y asignar a cliente | `PanelEntrenadorAsignarRutina.aspx` → `CrearRutinaParaCliente` | Implementado (con defectos) |
| Seleccionar, **modificar** y asignar rutinas | `PanelEntrenadorRutinas.aspx` | **Modificar no existe** (solo listar/eliminar) |
| Asignar rutina a cliente | `PanelEntrenador.aspx` + `PanelEntrenadorAsignarRutina.aspx` | Implementado |
| Listar ejercicios con filtros | `Ejercicios.aspx` / `EjerciciosNegocio.ListarEjercicios` | **No implementado** (página vacía, sin filtros) |
| Buscar cliente y asignarle rutina | `PanelEntrenador.aspx` (búsqueda en memoria) | Implementado |
| Ver rutinas realizadas del cliente | `HistorialDeEntrenamiento.aspx` | **No implementado** (página vacía) |
| Ver perfil del cliente | `perfil.aspx` (valida rol CLIENTE) | **No disponible para el entrenador** |

## Resumen de hallazgos

| ID | Severidad | Título | Categoría |
|----|-----------|--------|-----------|
| EN-01 | 🔴 Crítico | Ninguna página del panel valida sesión/rol | Seguridad |
| EN-02 | 🟠 Alto | El entrenador no puede iniciar sesión (login no maneja el rol) | Bug |
| EN-03 | 🟠 Alto | Creación de rutina + ejercicios sin transacción | Bug |
| EN-04 | 🟠 Alto | `int.Parse` de peso/series/reps sin validar | Robustez |
| EN-05 | 🟠 Alto | "Listar ejercicios con filtros" no implementado | Funcional |
| EN-06 | 🟠 Alto | "Modificar rutinas" no implementado | Funcional |
| EN-07 | 🟡 Medio | Listar ejercicios rompe si un ejercicio no tiene grupo muscular | Bug |
| EN-08 | 🟡 Medio | Ver perfil / rutinas realizadas del cliente no disponible para el entrenador | Funcional |
| EN-09 | 🟡 Medio | Cliente inexistente no se valida al asignar rutina | Bug |
| EN-10 | 🟡 Medio | `Session["EjerciciosRutina"]` no se limpia (se arrastra entre flujos) | Bug |
| EN-11 | ⚪ Bajo | Eliminar rutina sin verificar uso ni confirmación | Calidad |

> **Nota:** `PanelEntrenadorAsignarRutina.btnQuitar_Click` está marcado `PENDIENTE` → no se audita.

---

## 🔴 EN-01 — Ninguna página del panel de Entrenador valida sesión/rol
- **Severidad:** 🔴 Crítico
- **Categoría:** Seguridad (control de acceso)
- **Ubicación:** `PanelEntrenador.aspx.cs:14`, `PanelEntrenadorCrearRutina.aspx.cs:14`,
  `PanelEntrenadorRutinas.aspx.cs:14`, `PanelEntrenadorAsignarRutina.aspx.cs:14`
- **Descripción:** Ninguno de los `Page_Load` valida `Seguridad.SessionActiva` /
  `accesoYPermisos(..., Roles.ENTRENADOR)`. Cualquier visitante (anónimo, o un cliente logueado)
  puede acceder por URL directa, **listar todos los clientes** (`PanelEntrenador`), crear rutinas y
  **asignar rutinas a cualquier cliente** (`PanelEntrenadorAsignarRutina?idCliente=...`).
- **Impacto:** Exposición del padrón de clientes y manipulación de sus rutinas por cualquiera. Es el
  hallazgo más grave del pilar. (Las páginas equivalentes del Cliente —`Rutinas`, `DetalleRutina`,
  `perfil`, `Records`, `SesionesEntrenamiento`— sí validan rol; acá falta por completo.)
- **Corrección sugerida:** agregar al inicio de cada `Page_Load`:
  ```csharp
  if (!Seguridad.SessionActiva(Session["usuario"]) ||
      !Seguridad.accesoYPermisos((Usuario)Session["usuario"], Roles.ENTRENADOR))
  { Response.Redirect("Default", false); return; }
  ```

## 🟠 EN-02 — El entrenador no puede iniciar sesión
- **Severidad:** 🟠 Alto
- **Categoría:** Bug / Lógica
- **Ubicación:** `Gimnasio-app/Login.aspx.cs:34-47` (ver T-02 en `00-Transversal.md`)
- **Descripción:** El `switch` del login solo contempla ADMIN y CLIENTE; para ENTRENADOR cae en
  `default: break;` y no crea sesión ni redirige. Aunque las credenciales sean válidas, el entrenador
  se queda en el login.
- **Impacto:** El módulo del entrenador no es accesible por la vía esperada. Combinado con EN-01,
  queda la paradoja de un módulo "inaccesible por login pero abierto por URL".
- **Corrección sugerida:** agregar `case (int)Roles.ENTRENADOR:` que guarde `Session["usuario"]` y
  redirija a `PanelEntrenador.aspx`.

## 🟠 EN-03 — Creación de rutina con ejercicios sin transacción
- **Severidad:** 🟠 Alto
- **Categoría:** Bug / Integridad de datos
- **Ubicación:** `Negocio/RutinasNegocio.cs:140-192` (`CrearRutinaGeneral`) y `:194-243`
  (`CrearRutinaParaCliente`)
- **Descripción:** Se inserta primero la rutina (con `OUTPUT INSERTED.IdRutinas`) y luego, en un bucle,
  cada `RutinaEjercicio` con **una conexión nueva por iteración**, sin transacción que abarque todo.
  Si falla la inserción de un ejercicio intermedio, queda una rutina creada con solo parte de sus
  ejercicios (o ninguno), y no hay rollback.
- **Impacto:** Rutinas corruptas/incompletas de difícil detección. (Existen SP transaccionales
  —`SP_Rutina_General`— pero el código no los usa; ver T-15.)
- **Corrección sugerida:** envolver la cabecera + todos los detalles en una única transacción
  (`SqlTransaction` compartida, o un SP con `BEGIN TRAN ... COMMIT` que reciba todo). Hacer rollback
  ante cualquier fallo.

## 🟠 EN-04 — `int.Parse` de peso/series/repeticiones sin validar
- **Severidad:** 🟠 Alto
- **Categoría:** Robustez
- **Ubicación:** `PanelEntrenadorCrearRutina.aspx.cs:49-51` y `PanelEntrenadorAsignarRutina.aspx.cs:68-70`
- **Descripción:** `rutinaEjercicio.ObjetivoKG = int.Parse(txtPeso.Text);` (idem series y
  repeticiones) sobre el texto crudo del input. Si el campo está vacío o no es numérico, `int.Parse`
  lanza `FormatException`. En `CrearRutina` no hay `try/catch` (el error sube sin manejar); en
  `AsignarRutina` se captura pero se re-lanza envuelto.
- **Impacto:** El entrenador que deje un campo vacío o escriba texto provoca un error no amigable.
- **Corrección sugerida:** usar `int.TryParse` y validar rangos (peso ≥ 0, series/reps ≥ 1) antes de
  agregar, mostrando un toast de advertencia. Idealmente, validadores en el markup.

## 🟠 EN-05 — "Listar ejercicios con filtros" no implementado
- **Severidad:** 🟠 Alto
- **Categoría:** Funcional
- **Ubicación:** `Gimnasio-app/Ejercicios.aspx.cs:12-15` (vacío); `Negocio/EjerciciosNegocio.cs:17-51`
  (`ListarEjercicios` sin parámetros de filtro)
- **Descripción:** La página `Ejercicios.aspx` tiene el `Page_Load` vacío y `ListarEjercicios` trae
  todo el catálogo sin filtrar por ejercicio ni por grupo muscular. No existe el filtrado pedido.
- **Impacto:** La funcionalidad "Ejercicios: Listar con filtros (ejercicios, grupos musculares)" no
  está disponible.
- **Corrección sugerida:** implementar la página con búsqueda por nombre y un dropdown de grupo
  muscular, y agregar sobrecargas/condiciones de filtro a la consulta (parametrizadas).

## 🟠 EN-06 — "Seleccionar rutina, modificarla y asignarla" no está implementado
- **Severidad:** 🟠 Alto
- **Categoría:** Funcional
- **Ubicación:** `Gimnasio-app/PanelEntrenadorRutinas.aspx.cs` (solo `GetRutinasGenerales` +
  `EliminarRutina`)
- **Descripción:** La página lista las rutinas generales y permite eliminarlas (baja lógica), pero
  **no hay edición** de una rutina existente ni un camino para "seleccionar y reasignar". La
  asignación (`PanelEntrenadorAsignarRutina`) solo permite **crear** una rutina nueva desde cero para
  el cliente, no reutilizar/modificar una existente.
- **Impacto:** Dos de las funcionalidades de rutinas del entrenador (modificar; seleccionar+asignar
  una existente) no se pueden cumplir.
- **Corrección sugerida:** agregar edición de rutina (cargar sus `RutinaEjercicios`, permitir
  cambios, persistir) y un flujo para asignar una rutina existente a un cliente.

## 🟡 EN-07 — Listar ejercicios rompe si un ejercicio no tiene grupo muscular
- **Severidad:** 🟡 Medio
- **Categoría:** Bug / Robustez
- **Ubicación:** `Negocio/EjerciciosNegocio.cs:35`
- **Descripción:** `ejercicio.GrupoMuscular.IdGrupoMuscular = int.Parse(datos.Lector["IdGrupoMuscular"].ToString());`
  La columna `Ejercicios.IdGrupoMuscular` es `TINYINT` **nullable**. Para un ejercicio sin grupo
  (`NULL`), `datos.Lector["IdGrupoMuscular"].ToString()` es `""` → `int.Parse("")` lanza
  `FormatException`, abortando el listado completo.
- **Impacto:** Los dropdowns de ejercicios de "crear/asignar rutina" (que usan `ListarEjercicios`)
  dejan de cargar si existe un solo ejercicio sin grupo muscular.
- **Corrección sugerida:** chequear `DBNull` antes de convertir y dejar el grupo en 0/null
  (como ya hace `RutinasNegocio.GetEjerciciosDeRutina:277`).

## 🟡 EN-08 — Ver perfil / rutinas realizadas del cliente: no disponible para el entrenador
- **Severidad:** 🟡 Medio
- **Categoría:** Funcional
- **Ubicación:** `Gimnasio-app/perfil.aspx.cs:20-21` (valida `Roles.CLIENTE`);
  `Gimnasio-app/HistorialDeEntrenamiento.aspx.cs:12-15` (vacío)
- **Descripción:** "Ver perfil del cliente" del entrenador no tiene página propia: `perfil.aspx` está
  restringida a CLIENTE, así que un entrenador no puede consultarlo. "Buscar rutinas realizadas"
  del cliente tampoco: `HistorialDeEntrenamiento.aspx` está vacío (la pantalla de sesiones existe,
  pero `SesionesEntrenamiento.aspx` es del propio cliente, filtrada por su sesión).
- **Impacto:** Dos funcionalidades del entrenador (ver perfil del cliente; ver sus entrenamientos
  realizados) no existen.
- **Corrección sugerida:** crear vistas de solo lectura para el entrenador que reciban el `idCliente`
  (validando rol ENTRENADOR) y reutilicen `ClienteNegocio.Get(id, full:true)` y
  `SesionEntrenamientoNegocio.GetSesionesDeCliente(idCliente)`.

## 🟡 EN-09 — Cliente inexistente no se valida al asignar rutina
- **Severidad:** 🟡 Medio
- **Categoría:** Bug / Robustez
- **Ubicación:** `PanelEntrenadorAsignarRutina.aspx.cs:28-30` + `Negocio/ClienteNegocio.cs:19-61`
- **Descripción:** `Cliente cliente = clienteNegocio.Get(idCliente);` y luego `cliente.Nombre`. Si el
  `idCliente` del query string no existe, `Get` **no devuelve `null`** sino un `Cliente` vacío
  (`IdUsuario = 0`). Así, la página muestra un nombre en blanco y, al guardar, intenta
  `CrearRutinaParaCliente(0, ...)`, que fallará por la FK contra `Usuarios`.
- **Impacto:** Flujo confuso (cliente "fantasma") y error de FK no amigable al guardar.
- **Corrección sugerida:** que `Get` devuelva `null` cuando no encuentra (o exponer un método
  `Existe(id)`), y validar en la página antes de continuar; redirigir a `PanelEntrenador` si el
  cliente no existe.

## 🟡 EN-10 — `Session["EjerciciosRutina"]` no se limpia al entrar al flujo
- **Severidad:** 🟡 Medio
- **Categoría:** Bug / Lógica
- **Ubicación:** `PanelEntrenadorCrearRutina.aspx.cs:36-55,84` y `PanelEntrenadorAsignarRutina.aspx.cs:55-74,110`
- **Descripción:** La lista de ejercicios en construcción se guarda en `Session["EjerciciosRutina"]`,
  **compartida** entre la creación de rutina general y la asignación a cliente. Solo se limpia al
  guardar con éxito. Si el entrenador empieza a armar una rutina y abandona la página (sin guardar),
  los ejercicios quedan en `Session` y **aparecen al iniciar la siguiente rutina** (incluso en la otra
  pantalla).
- **Impacto:** Rutinas con ejercicios "heredados" de un intento anterior.
- **Corrección sugerida:** inicializar/limpiar `Session["EjerciciosRutina"]` en el `Page_Load` de
  cada pantalla cuando `!IsPostBack`, o usar una clave distinta por pantalla.

## ⚪ EN-11 — Eliminar rutina sin verificar uso ni confirmación
- **Severidad:** ⚪ Bajo
- **Categoría:** Calidad
- **Ubicación:** `PanelEntrenadorRutinas.aspx.cs:24-35` + `RutinasNegocio.cs:319-339`
- **Descripción:** `EliminarRutina` hace baja lógica (`Activo = 0`) sin confirmación del usuario ni
  verificación de si la rutina está asignada a clientes o referida por sesiones de entrenamiento.
- **Impacto:** Posible desactivación silenciosa de una rutina en uso.
- **Corrección sugerida:** pedir confirmación (modal/JS) y, opcionalmente, advertir si la rutina está
  asignada o tiene sesiones asociadas.

---

### Bien resuelto
- La capa `RutinasNegocio` cierra conexiones con `try/finally` correctamente y parametriza las
  consultas. El problema principal es de **control de acceso** (EN-01) y de **integridad
  transaccional** (EN-03), no de SQL injection.
