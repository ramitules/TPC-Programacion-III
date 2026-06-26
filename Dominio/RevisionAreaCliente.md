# Revisión del área de Cliente — Bugs y mejoras

> Revisión de la vista del usuario con rol **Cliente**: `perfil.aspx`, `DetalleRutina.aspx`,
> `Records.aspx`, `RegistrarUsuario.aspx`, `Rutinas.aspx`, `SesionesEntrenamiento.aspx`,
> `Site.Master` y sus dependencias en `Dominio` y `Negocio`.
>
> Este documento **no modifica código**: lista cada hallazgo con su ubicación
> (`archivo:línea`) y la corrección sugerida. La sección **Base de datos** trae SQL listo
> para aplicar a mano. Se excluyó todo lo relativo a Admin/Administrativo/Entrenador.
>
> Fecha de revisión: 2026-06-26

---

## Resumen

| # | Severidad | Hallazgo | Archivo principal |
|---|-----------|----------|-------------------|
| A1 | 🔴 Alto | Validación nombre/apellido con `OR` en vez de `AND` | `Perfil.aspx.cs`, `RegistrarUsuario.aspx.cs` |
| A2 | 🔴 Alto | NRE en Perfil si el cliente no tiene suscripción ACTIVA | `Perfil.aspx.cs` |
| A3 | 🔴 Alto | Condición de carrera al iniciar sesión de entrenamiento | `SesionEntrenamientoNegocio.cs` |
| A4 | 🔴 Alto | Alta de usuario con fallo silencioso (parece éxito sin crear cuenta) | `RegistrarUsuario.aspx.cs`, `sp_CrearUsuario` |
| A5 | 🔴 Alto | Site.Master: menú cruzado, faltan iconos, nombre hardcodeado | `Site.Master` |
| A6 | 🔴 Alto | El login no cablea al Cliente; las páginas dependen de `// TESTING` | `Login.aspx.cs`, varias |
| A7 | 🟠 Medio | `is null` vs `is DBNull` al leer `IdRutina` | `SesionEntrenamientoNegocio.cs` |
| A8 | 🟠 Medio | `throw ex;` y excepciones tragadas en la capa de datos | `AccesoADatos.cs`, otros |
| A9 | 🟡 Bajo | Inputs de serie sin validar (pesos/reps negativos) | `DetalleRutina.aspx.cs` |
| A10 | 🟡 Bajo | Valores persistidos sin normalizar | `RegistrarUsuario.aspx.cs`, `Perfil.aspx.cs` |
| B1–B5 | 🟡 Bajo | Mejoras de UX/código no bloqueantes | varias |
| D1–D6 | 🟠/🟡 | Mejoras de base de datos (SQL incluido) | scripts SQL |
| Apéndice | 🔴 Alto | `Inserts.sql` no carga ningún ejercicio (mismatch de columnas) | `Inserts.sql` |

---

## A. Bugs de código

### A1. 🔴 Validación nombre/apellido usa `OR` en vez de `AND`

**Ubicación:** [`Perfil.aspx.cs:85`](Gimnasio-app/Perfil.aspx.cs:85) y [`RegistrarUsuario.aspx.cs:32`](Gimnasio-app/RegistrarUsuario.aspx.cs:32)

```csharp
if (!(Validaciones.validarNombre(nombre) || Validaciones.validarNombre(apellido)))
{
    Toasts.ToastAdvertencia(this, "Por favor complete todos los campos.", "Atencion");
    return;
}
```

`validarNombre` devuelve `true` cuando el texto no está vacío. La expresión `!(A || B)` es
verdadera **solo cuando ambos están vacíos**. Si el usuario completa el nombre pero deja el
apellido en blanco (o viceversa), la validación pasa y se guarda un registro incompleto.

**Corrección:**

```csharp
if (!(Validaciones.validarNombre(nombre) && Validaciones.validarNombre(apellido)))
```

---

### A2. 🔴 NullReferenceException en Perfil si el cliente no tiene suscripción ACTIVA

**Ubicación:** [`Perfil.aspx.cs:47-51`](Gimnasio-app/Perfil.aspx.cs:47)

```csharp
ddlPlan.SelectedValue = cliente.SuscripcionCliente.Plan.IdPlan.ToString();
txtVencimiento.Text = cliente.SuscripcionCliente.FechaFin.ToString("yyyy-MM-dd");
int vencimiento = (cliente.SuscripcionCliente.FechaFin - DateTime.Now).Days;
```

`SuscripcionNegocio.GetSuscripcionCliente` ([`SuscripcionNegocio.cs:17-56`](Negocio/SuscripcionNegocio.cs:17))
devuelve `new Suscripcion()` cuando el cliente no tiene una suscripción del estado pedido. Ese
objeto tiene `IdSuscripcion == 0`, **`Plan == null`** y `FechaFin == default(DateTime)`.
Por lo tanto `cliente.SuscripcionCliente.Plan.IdPlan` lanza **NullReferenceException**.

Esto ocurre para **cualquier cliente sin suscripción ACTIVA**: recién registrado (el alta de
cliente no crea suscripción), cancelado o vencido. Hoy se enmascara porque el código de TESTING
fuerza al usuario 9 (que sí tiene una suscripción activa en `Inserts.sql`), pero al cablear el
login real (ver A6) la página revienta.

**Corrección:** validar antes de leer y mostrar un estado "sin suscripción":

```csharp
Suscripcion susc = cliente.SuscripcionCliente;
bool tieneSuscripcion = susc != null && susc.IdSuscripcion != 0 && susc.Plan != null;

if (tieneSuscripcion)
{
    ddlPlan.SelectedValue = susc.Plan.IdPlan.ToString();
    txtVencimiento.Text = susc.FechaFin.ToString("yyyy-MM-dd");
    int vencimiento = (susc.FechaFin - DateTime.Now).Days;
    lblVencimiento.Text = "Vence en " + vencimiento + " dias";
    if (vencimiento < 5) btnRenovarPlan.Enabled = true;
}
else
{
    lblVencimiento.Text = "No tenés una suscripción activa.";
    btnCambiarPlan.Enabled = false;   // o redirigir a alta de suscripción
    btnRenovarPlan.Enabled = false;
}
```

---

### A3. 🔴 Condición de carrera al iniciar una sesión de entrenamiento

**Ubicación:** [`SesionEntrenamientoNegocio.cs:166-176`](Negocio/SesionEntrenamientoNegocio.cs:166) y [`:16-56`](Negocio/SesionEntrenamientoNegocio.cs:16)

```csharp
public SesionEntrenamiento IniciarSesionEntrenamiento(Cliente cliente, Rutina rutina = null)
{
    ...
    Agregar(sesion);
    return Get();          // <-- Get() sin id
}
```

`Get()` sin id ejecuta `SELECT TOP 1 * FROM SesionesEntrenamiento ORDER BY
IdSesionesEntrenamiento DESC`, es decir, **la última sesión insertada en toda la tabla**, no la
del usuario que acaba de iniciar. Con dos clientes iniciando casi a la vez, el cliente A puede
quedarse con la sesión del cliente B (`Session["sesionActiva"]` apuntando a otra persona). Además,
`Get()` dispara consultas adicionales para resolver `Cliente` y `Rutina` por cada fila.

**Corrección (código + DB):** que `sp_CrearSesionEntrenamiento` devuelva el `SCOPE_IDENTITY()`
(ver **D4**) y que el negocio capture ese id directamente:

```csharp
// Nuevo método en SesionEntrenamientoNegocio
public int AgregarYDevolverId(SesionEntrenamiento nuevo)
{
    AccesoADatos datos = new AccesoADatos();
    try
    {
        datos.SetearConsultaSP("sp_CrearSesionEntrenamiento");
        datos.setearParametro("@IdUsuario", nuevo.Cliente.IdUsuario);
        datos.setearParametro("@IdRutina", (object)nuevo.Rutina?.IdRutina ?? DBNull.Value);
        datos.setearParametro("@FechaHoraInicio", nuevo.FechaHoraInicio);
        datos.setearParametro("@FechaHoraFin", DBNull.Value);   // ver D3
        return datos.EjecutarScalar();
    }
    finally { datos.cerrarConexion(); }
}
```

`IniciarSesionEntrenamiento` setea `sesion.IdSesion` con ese id en vez de llamar `Get()`.

---

### A4. 🔴 Alta de usuario con fallo silencioso (parece éxito sin crear la cuenta)

**Ubicación:** [`RegistrarUsuario.aspx.cs:69-74`](Gimnasio-app/RegistrarUsuario.aspx.cs:69), [`ClienteNegocio.cs:106-150`](Negocio/ClienteNegocio.cs:106), `sp_CrearUsuario` ([`ProcedimientosAlmacenados.sql:179-207`](ProcedimientosAlmacenados.sql:179))

Flujo del problema:

1. `ClienteNegocio.Agregar` → `AltaOModificacion` ejecuta el SP con `ejecutarAccion()`
   (`ExecuteNonQuery`), ignorando el `SELECT @IdUsuario` que el SP devuelve.
2. `sp_CrearUsuario`, en su `CATCH`, hace `ROLLBACK` + `PRINT 'Error...'` y **no relanza** el error.
3. Ante un email duplicado (la columna `Email` es `UNIQUE`) u otro fallo, el SP "termina bien"
   desde el punto de vista de ADO.NET: no se lanza excepción.
4. `RegistrarUsuario` oculta el formulario y muestra el panel de éxito ("¡Cuenta creada con
   éxito!"), pero **no se creó ningún usuario**.

**Corrección (dos partes):**

- **DB:** que `sp_CrearUsuario` relance el error con `THROW;` en el `CATCH` (ver **D5**).
- **Código:** detectar el caso de email duplicado y avisar. Opción simple, validando antes:

```csharp
// Antes de crear, verificar email único
if (new ClienteNegocio().ExisteEmail(email))   // método nuevo: SELECT COUNT(*) ... WHERE Email=@Email
{
    Toasts.ToastError(this, "Ya existe una cuenta con ese correo electronico.");
    return;
}
```

  Y/o capturar la excepción del SP (cuando ya use `THROW`) e inspeccionar
  `SqlException.Number == 2627/2601` (violación de UNIQUE) para mostrar el mensaje específico.

---

### A5. 🔴 Site.Master: menú de Cliente cruzado, faltan iconos y nombre hardcodeado

**Ubicación:** [`Site.Master`](Gimnasio-app/Site.Master)

1. **Items invertidos** ([`Site.Master:83-92`](Gimnasio-app/Site.Master:83)):

   ```html
   <a ... href="~/Ejercicios"> ...Mis rutinas </a>   <!-- apunta a Ejercicios -->
   <a ... href="~/Rutinas">    ...Mis ejercicios </a> <!-- apunta a Rutinas -->
   ```

   Los textos y destinos están cruzados. Además el Cliente no tiene una página propia de
   "ejercicios" (`Ejercicios.aspx` es ABM de Admin). **Corrección:** "Mis rutinas" → `~/Rutinas`
   y eliminar (o reapuntar) el item que va a `~/Ejercicios`.

2. **Faltan los iconos de Bootstrap Icons.** En el `<head>` solo se enlazan `bootstrap.min.css`
   y `toastr.css` ([`Site.Master:12-13`](Gimnasio-app/Site.Master:12)), pero toda la app usa
   clases `bi bi-*` (`bi-house-door`, `bi-trophy`, `bi-chevron-down`, etc.) que **no se renderizan**
   sin la hoja de iconos. **Corrección:** agregar en el `<head>`:

   ```html
   <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet">
   ```

3. **"Nombre Usuario" hardcodeado** ([`Site.Master:111`](Gimnasio-app/Site.Master:111)): el dropdown
   siempre muestra el texto literal "Nombre Usuario". **Corrección:** exponer una propiedad en
   `Site.master.cs` que lea `Session["cliente"]` y bindearla (`<%= NombreUsuario %>`).

4. **jQuery duplicado:** se carga por ScriptManager ([`:21`](Gimnasio-app/Site.Master:21)) y de nuevo
   por CDN al final ([`:141`](Gimnasio-app/Site.Master:141)). Dejar una sola fuente.

> Nota de contexto (no es del área Cliente, pero explica por qué el menú de cliente "no
> aparece"): [`Default.aspx.cs:16`](Gimnasio-app/Default.aspx.cs:16) hace `Session["admin"] = true`
> incondicionalmente, y `Site.Master` muestra el menú según `Session["admin"]`/`Session["cliente"]`.
> Mientras eso esté así, siempre se ve el menú de Admin.

---

### A6. 🔴 El login no cablea al Cliente; las páginas dependen de `// TESTING`

**Ubicación:** [`Login.aspx.cs:30-44`](Gimnasio-app/Login.aspx.cs:30), [`Seguridad.cs:46-87`](Integraciones/Seguridad.cs:46)

- El login guarda el usuario en `Session["usuario"]` (no `Session["cliente"]`), no hidrata
  peso/suscripción/records, y solo redirige al Admin (los clientes quedan en el login).
- `Seguridad.logueo` construye un `Cliente` "pelado" (sin `PesoCorporal`, `SuscripcionCliente`
  ni `RecordsPersonales`).

Para compensar, **cada página de Cliente fuerza al usuario 9** con una línea de pruebas:

| Página | Línea |
|--------|-------|
| Perfil | [`Perfil.aspx.cs:20-22`](Gimnasio-app/Perfil.aspx.cs:20) |
| Records | [`Records.aspx.cs:15`](Gimnasio-app/Records.aspx.cs:15) |
| Rutinas | [`Rutinas.aspx.cs:18`](Gimnasio-app/Rutinas.aspx.cs:18) |
| SesionesEntrenamiento | [`SesionesEntrenamiento.aspx.cs:49`](Gimnasio-app/SesionesEntrenamiento.aspx.cs:49) |

```csharp
//  TESTING
Session.Add("cliente", new ClienteNegocio().Get("9", true));
//  TESTING
```

Esto **sobrescribe la sesión real** y reconsulta la BD en **cada postback** (en Records, Rutinas
y Sesiones la línea está incluso antes del guard `!IsPostBack`, así que pega a la base siempre).

**Corrección documentada (cableado del login + remoción de TESTING):**

1. En `Login.aspx.cs`, tras `Seguridad.logueo`, si el rol es `CLIENTE`, hidratar el cliente
   completo y dejarlo en `Session["cliente"]`, y redirigir al área de cliente:

   ```csharp
   if (usuario.Rol.IdRol == (int)Roles.CLIENTE)
   {
       Cliente cliente = new ClienteNegocio().Get(usuario.IdUsuario.ToString(), full: true);
       Session["cliente"] = cliente;
       Response.Redirect("~/Rutinas.aspx", false);
       return;
   }
   ```

   (Alternativa: que `Seguridad.logueo` ya devuelva el `Cliente` hidratado cuando el rol es CLIENTE.)

2. **Eliminar** las 4 líneas `// TESTING`. Las páginas deben usar el `Session["cliente"]` real.
3. Corregir **A2** en el mismo paso, porque al usar el login real aparecen clientes sin
   suscripción activa.

---

### A7. 🟠 `is null` vs `is DBNull` al leer `IdRutina`

**Ubicación:** [`SesionEntrenamientoNegocio.cs:43`](Negocio/SesionEntrenamientoNegocio.cs:43)

```csharp
if (!(datos.Lector["IdRutina"] is null))
    sesion.Rutina = new RutinasNegocio().Get(datos.Lector["IdRutina"].ToString());
```

Una columna `NULL` leída del `SqlDataReader` devuelve `DBNull.Value`, **nunca** `null` de C#.
Para una sesión libre (`IdRutina` NULL) la condición es siempre verdadera y dispara
`RutinasNegocio.Get("")` de forma innecesaria (funciona de casualidad devolviendo `null`).

**Corrección:** `if (!(datos.Lector["IdRutina"] is DBNull))`.

---

### A8. 🟠 `throw ex;` y excepciones tragadas en la capa de datos

**Ubicación:** [`AccesoADatos.cs`](AccesoDB/AccesoADatos.cs) y otros

- [`AccesoADatos.ejecutarLectura():51-63`](AccesoDB/AccesoADatos.cs:51) atrapa la excepción y solo
  hace `Console.WriteLine`. El `lector` queda en `null`, de modo que el llamador revienta con un
  `NullReferenceException` **lejos de la causa real** (la consulta que falló). Debería relanzar,
  igual que hace `ejecutarAccion`.
- `throw ex;` **resetea el stack trace** (se pierde el punto de origen). Aparece en
  [`EjecutarScalar():48`](AccesoDB/AccesoADatos.cs:48), [`ejecutarAccion():78`](AccesoDB/AccesoADatos.cs:78),
  [`Login.aspx.cs:53`](Gimnasio-app/Login.aspx.cs:53),
  [`SuscripcionNegocio.BajaSuscripcionCliente:129`](Negocio/SuscripcionNegocio.cs:129).

**Corrección:** en `ejecutarLectura` relanzar (`throw;`) tras —opcionalmente— loguear; y
reemplazar todos los `throw ex;` por `throw;`.

---

### A9. 🟡 Inputs de serie sin validar (pesos/reps negativos)

**Ubicación:** [`DetalleRutina.aspx.cs:170-179`](Gimnasio-app/DetalleRutina.aspx.cs:170), markup [`DetalleRutina.aspx:101-109`](Gimnasio-app/DetalleRutina.aspx:101)

`ParsearEntero` usa `float.TryParse` y devuelve `0` ante texto inválido; los `TextBox`
(`TextMode="Number"`) no tienen `min`, así que aceptan negativos. Se pueden registrar series con
peso/reps negativos.

**Corrección:** agregar `min="0"` (y `step` si se permiten decimales) en el markup y validar en
servidor (`peso >= 0`, `reps >= 1`, `RIR >= 0`) antes de `Agregar`.

---

### A10. 🟡 Valores persistidos sin normalizar

**Ubicación:** [`RegistrarUsuario.aspx.cs:60-67`](Gimnasio-app/RegistrarUsuario.aspx.cs:60), [`Perfil.aspx.cs:80,105`](Gimnasio-app/Perfil.aspx.cs:80)

- En el registro se **valida** sobre las variables normalizadas (`nombre`, `email` con `Trim()`
  / `ToLower()` en `:25-27`), pero se **persiste** el texto crudo del control
  (`Nombre = txtNombre.Text`, `Email = txtEmail.Text`). Se guardan espacios sobrantes y el email
  sin pasar a minúsculas, lo que afecta la unicidad real del correo.
- En Perfil, el apellido se lleva a minúsculas pero el nombre no (criterio inconsistente) y
  `PesoCorporal = float.Parse(txtPeso.Text)` se parsea sin validar (puede lanzar excepción /
  guardar valores absurdos).

**Corrección:** persistir siempre las variables ya normalizadas y unificar el criterio de casing;
validar el peso (numérico y rango razonable) antes de asignarlo.

---

## B. Mejoras de UX/código (no bloqueantes)

- **B1.** [`RutinasNegocio.GetRutinasUsuario:33`](Negocio/RutinasNegocio.cs:33) y
  [`GetRutinasGenerales:74`](Negocio/RutinasNegocio.cs:74) filtran `Activo` en C# con `continue`
  en vez de `WHERE Activo = 1`. Mover el filtro al SQL (trae menos filas y evita la lógica extra).
- **B2.** [`RutinasNegocio.AgruparPorDia:251`](Negocio/RutinasNegocio.cs:251) compara los días con
  `StringComparison.OrdinalIgnoreCase` contra `"Miércoles"` / `"Sábado"` (con acento). La BD es
  `Latin1_General_CI_AI` (accent-insensitive): si algún `Dia` se guarda sin acento, la rutina cae
  en "Sin día". Comparar normalizando acentos (o normalizar el valor al guardar).
- **B3.** Perfil no refresca la UI de suscripción tras pagar/cambiar/cancelar: los labels
  (`txtVencimiento`, `lblVencimiento`, `txtProximoPlan`) solo se cargan en `!IsPostBack`. Tras
  `CrearPlan` / `btnCancelarSuscripcion_click` conviene recargar esos datos.
- **B4.** El "pago" del modal **siempre se aprueba** si el formato es válido
  ([`Perfil.aspx.cs:206-207`](Gimnasio-app/Perfil.aspx.cs:206)). Es un mockup de pasarela; dejarlo
  documentado como tal para que no se confunda con un cobro real.
- **B5.** Contraseñas en texto plano: `AccesoUsuarios.Pass VARCHAR(40)`, y `sp_logueo` compara el
  texto plano. Recomendación de seguridad: hashear (p. ej. PBKDF2/bcrypt) y ampliar la columna.
  Afecta el registro de cliente; es una mejora secundaria pero importante.

---

## C. Base de datos — cambios recomendados (aplicar a mano)

> SQL listo para ejecutar contra `GestionGimnasio`. Probar primero en una copia.
> Estados de suscripción: `ACTIVA = 1`, `VENCIDA = 2`, `CANCELADA = 3`, `VIGENTE_PENDIENTE = 4`.

### D1. Mantenimiento diario de suscripciones (promover el "próximo plan")

**Problema:** al cambiar/renovar plan, `Perfil.CrearPlan` ([`Perfil.aspx.cs:274-305`](Gimnasio-app/Perfil.aspx.cs:274))
crea una suscripción `VIGENTE_PENDIENTE (4)` que debería activarse al vencer la actual. Pero
**nada la promueve a ACTIVA**: el SP existente `SP_Suscripciones_actualizar_estado_vencidas`
([`ProcedimientosAlmacenados.sql:147-154`](ProcedimientosAlmacenados.sql:147)) solo pasa ACTIVA→VENCIDA.
El "próximo plan pagado" nunca entra en vigencia automáticamente.

```sql
USE GestionGimnasio;
GO

ALTER PROCEDURE SP_Suscripciones_MantenimientoDiario
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Vencer las activas cuya fecha de vencimiento ya pasó
    UPDATE Suscripciones
       SET IdEstado = 2                      -- VENCIDA
     WHERE IdEstado = 1                       -- ACTIVA
       AND FechaVencimiento < CAST(GETDATE() AS DATE);

    -- 2) Activar las pendientes cuyo inicio ya llegó, si el usuario no tiene otra activa
    UPDATE S
       SET IdEstado = 1                       -- ACTIVA
      FROM Suscripciones S
     WHERE S.IdEstado = 4                      -- VIGENTE_PENDIENTE
       AND S.FechaInicio      <= CAST(GETDATE() AS DATE)
       AND S.FechaVencimiento >= CAST(GETDATE() AS DATE)
       AND NOT EXISTS (
             SELECT 1 FROM Suscripciones A
              WHERE A.IdUsuario = S.IdUsuario
                AND A.IdEstado  = 1);
END
GO
```

> Si ya existe `SP_Suscripciones_actualizar_estado_vencidas` y preferís conservar el nombre,
> reemplazá `ALTER PROCEDURE SP_Suscripciones_MantenimientoDiario` por
> `ALTER PROCEDURE SP_Suscripciones_actualizar_estado_vencidas` y dejá el cuerpo igual.

**Ejecución diaria — SQL Server Agent Job:**

```sql
USE msdb;
GO
EXEC dbo.sp_add_job @job_name = N'Gimnasio_MantenimientoSuscripciones';
EXEC dbo.sp_add_jobstep
     @job_name   = N'Gimnasio_MantenimientoSuscripciones',
     @step_name  = N'Actualizar estados',
     @subsystem  = N'TSQL',
     @database_name = N'GestionGimnasio',
     @command    = N'EXEC dbo.SP_Suscripciones_MantenimientoDiario;';
EXEC dbo.sp_add_schedule
     @schedule_name = N'Diario_0100',
     @freq_type = 4, @freq_interval = 1,      -- diario
     @active_start_time = 010000;             -- 01:00
EXEC dbo.sp_attach_schedule
     @job_name = N'Gimnasio_MantenimientoSuscripciones',
     @schedule_name = N'Diario_0100';
EXEC dbo.sp_add_jobserver @job_name = N'Gimnasio_MantenimientoSuscripciones';
GO
```

> **Si no hay SQL Server Agent** (p. ej. SQL Express): llamar al SP desde el arranque de la app
> (`Global.asax → Application_Start`) y/o al cargar Perfil, comparando contra una marca de
> "última ejecución", o programarlo con el Programador de tareas de Windows vía `sqlcmd`.

---

### D2. Trigger de record personal en `SeriesCompletadas`

**Problema:** hoy el flag `EsRecordPersonal` se calcula en C#
([`DetalleRutina.aspx.cs:183-184`](Gimnasio-app/DetalleRutina.aspx.cs:183)) leyendo el máximo
histórico con `RecordsNegocio.GetMaxPesoEjercicio` y comparando antes de insertar. Es un patrón
TOCTOU (dos round-trips, sin atomicidad) y mezcla la regla de negocio con la UI.

Un trigger `AFTER INSERT` lo resuelve de forma atómica y consistente:

```sql
USE GestionGimnasio;
GO

CREATE TRIGGER TR_SeriesCompletadas_Record
ON SeriesCompletadas
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SC
       SET EsRecordPersonal = 1
      FROM SeriesCompletadas SC
      INNER JOIN inserted I
              ON I.IdSeriesCompletadas = SC.IdSeriesCompletadas
      INNER JOIN SesionesEntrenamiento SE
              ON SE.IdSesionesEntrenamiento = SC.IdSesion
     WHERE SC.PesoLevantadoKG > ISNULL((
               SELECT MAX(SC2.PesoLevantadoKG)
                 FROM SeriesCompletadas SC2
                 INNER JOIN SesionesEntrenamiento SE2
                         ON SE2.IdSesionesEntrenamiento = SC2.IdSesion
                WHERE SE2.IdUsuario   = SE.IdUsuario
                  AND SC2.IdEjercicio = SC.IdEjercicio
                  AND SC2.IdSeriesCompletadas <> SC.IdSeriesCompletadas), 0);
END
GO
```

**Caveat (insert multi-fila):** si en una sola sentencia se insertan varias series del mismo
ejercicio que superan el máximo histórico, cada una se compara contra el histórico **excluyéndose
a sí misma**, por lo que más de una podría marcarse como record. No es un problema para la vista:
`sp_RecordsPersonales` ([`ProcedimientosAlmacenados.sql:286-322`](ProcedimientosAlmacenados.sql:286))
ya toma, por ejercicio, la de mayor peso (y la más reciente en caso de empate). La app inserta de
a una serie por postback, así que en la práctica no se da.

**Cambio de código asociado:** con el trigger activo, `DetalleRutina` puede dejar de calcular
`EsRecordPersonal` y `sp_CrearSerieCompletada` puede ignorar el parámetro (o mantenerlo y dejar
que el trigger lo corrija). Se puede eliminar `RecordsNegocio.GetMaxPesoEjercicio` si no se usa en
otro lado.

---

### D3. `FechaHoraFin` NULLABLE = "sesión en curso"

**Problema:** una sesión nace con `FechaHoraFin = FechaHoraInicio`
([`SesionEntrenamientoNegocio.cs:166-176`](Negocio/SesionEntrenamientoNegocio.cs:166)), por lo que en
la BD **parece finalizada** apenas se crea. El estado "en curso" vive solo en
`Session["sesionActiva"]` (memoria del servidor). Si esa sesión se pierde (timeout, reciclado del
app pool, otro dispositivo) queda una **sesión huérfana**: aparece en el historial con duración
"-" ([`SesionesEntrenamiento.aspx.cs:259-268`](Gimnasio-app/SesionesEntrenamiento.aspx.cs:259)) y el
cliente puede iniciar otra encima.

Hacer `FechaHoraFin` nullable (NULL = en curso) permite detectar/rehidratar la sesión abierta
desde la base:

```sql
USE GestionGimnasio;
GO

-- 1) Quitar el DEFAULT de FechaHoraFin (nombre autogenerado: lo resolvemos dinámicamente)
DECLARE @def SYSNAME;
SELECT @def = dc.name
  FROM sys.default_constraints dc
  JOIN sys.columns c
    ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
 WHERE dc.parent_object_id = OBJECT_ID('dbo.SesionesEntrenamiento')
   AND c.name = 'FechaHoraFin';
IF @def IS NOT NULL
    EXEC('ALTER TABLE dbo.SesionesEntrenamiento DROP CONSTRAINT ' + @def);
GO

-- 2) Permitir NULL
ALTER TABLE dbo.SesionesEntrenamiento ALTER COLUMN FechaHoraFin DATETIME NULL;
GO
```

> El `CHECK (FechaHoraFin >= FechaHoraInicio)` sigue siendo válido: con `FechaHoraFin` NULL el
> predicado evalúa a UNKNOWN y **no bloquea** el insert.

Consulta para detectar la sesión en curso (para rehidratar `Session["sesionActiva"]` o impedir
iniciar dos):

```sql
SELECT TOP 1 *
  FROM SesionesEntrenamiento
 WHERE IdUsuario = @IdUsuario AND FechaHoraFin IS NULL
 ORDER BY FechaHoraInicio DESC;
```

**Cambios de código asociados:** al crear la sesión, pasar `FechaHoraFin = NULL` (ver D4);
`FinSesionEntrenamiento` la setea a `GETDATE()`; `DetalleRutina.Page_Load` puede preguntar a la BD
por una sesión abierta en lugar de depender solo de `Session["sesionActiva"]`; `FormatearDuracion`
debe contemplar `FechaHoraFin` nulo (mostrar "En curso").

---

### D4. `sp_CrearSesionEntrenamiento` devuelve el Id (resuelve A3)

```sql
USE GestionGimnasio;
GO

ALTER PROCEDURE sp_CrearSesionEntrenamiento (
    @IdUsuario INT,
    @IdRutina INT,
    @FechaHoraInicio DATETIME,
    @FechaHoraFin DATETIME = NULL          -- NULL = sesión en curso (ver D3)
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO SesionesEntrenamiento (IdUsuario, IdRutina, FechaHoraInicio, FechaHoraFin)
    VALUES (@IdUsuario, @IdRutina, @FechaHoraInicio, @FechaHoraFin);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdSesion;
END
GO
```

El C# captura el id con `EjecutarScalar()` (ver el método sugerido en **A3**) en vez de `Get()`.

---

### D5. Los SP de alta deben relanzar el error (`THROW`)

**Problema:** varios SP con `BEGIN TRY/CATCH` hacen `ROLLBACK` + `PRINT` y **no relanzan**, de modo
que la app cree que la operación salió bien (raíz de **A4**). Afecta a `sp_CrearUsuario`,
`sp_ModificarUsuario`, `SP_Rutina_General`, `SP_Rutina_Personaliazda_Usuario`,
`SP_Modificar_Rutina_*`.

Patrón a aplicar en cada `CATCH`:

```sql
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;     -- en vez de PRINT 'Error...'
    END CATCH
```

Ejemplo completo para el alta de usuario (resuelve A4):

```sql
USE GestionGimnasio;
GO

ALTER PROCEDURE sp_CrearUsuario (
    @Nombre VARCHAR(70), @Apellido VARCHAR(70), @Email VARCHAR(150),
    @FechaNacimiento DATETIME, @PesoCorporalKG DECIMAL(5,2),
    @IdRol TINYINT, @FechaIngreso DATETIME, @Pass VARCHAR(40)
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
            DECLARE @IdUsuario INT;
            INSERT INTO Usuarios (Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso)
            VALUES (@Nombre, @Apellido, @Email, @FechaNacimiento, @PesoCorporalKG, @IdRol, @FechaIngreso);
            SET @IdUsuario = SCOPE_IDENTITY();
            INSERT INTO AccesoUsuarios (IdUsuarios, Pass) VALUES (@IdUsuario, @Pass);
        COMMIT TRANSACTION;
        SELECT @IdUsuario AS IdUsuario;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
```

> Con esto, el email duplicado (violación de `UNIQUE`) llega como `SqlException` a la app y se
> puede mostrar el mensaje correcto (ver A4).

---

### D6. Peso con decimales

**Problema:** `SeriesCompletadas.PesoLevantadoKG` es `SMALLINT` y `RutinaEjercicios.ObjetivoKG` es
`INTEGER` ([`ScriptDB.sql:98,125`](ScriptDB.sql:98)), pero los discos fraccionarios (2.5 kg, 1.25 kg)
son habituales y el dominio usa `float` (`SerieCompletada.PesoLevantadoKG`). Los valores decimales
se truncan o rechazan.

```sql
USE GestionGimnasio;
GO
ALTER TABLE SeriesCompletadas ALTER COLUMN PesoLevantadoKG DECIMAL(6,2) NOT NULL;
ALTER TABLE RutinaEjercicios  ALTER COLUMN ObjetivoKG      DECIMAL(6,2);
GO

-- Ajustar el parámetro del SP de alta de serie
ALTER PROCEDURE sp_CrearSerieCompletada (
    @IdSesion INT,
    @IdEjercicio INT,
    @PesoLevantadoKG DECIMAL(6,2),
    @RepeticionesLogradas SMALLINT,
    @RIR TINYINT,
    @EsRecordPersonal BIT
)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO SeriesCompletadas (IdSesion, IdEjercicio, PesoLevantadoKG, RepeticionesLogradas, RIR, EsRecordPersonal)
    VALUES (@IdSesion, @IdEjercicio, @PesoLevantadoKG, @RepeticionesLogradas, @RIR, @EsRecordPersonal);
END
GO
```

> Acompañar con `step="0.25"` en los inputs de peso del markup
> ([`DetalleRutina.aspx:101`](Gimnasio-app/DetalleRutina.aspx:101)).

---

## Apéndice — arreglo de datos semilla (`Inserts.sql`)

🔴 **El INSERT de `Ejercicios` no carga ningún ejercicio.**
[`Inserts.sql:71`](Inserts.sql:71):

```sql
INSERT INTO Ejercicios (Nombre, IdGrupoMuscular)
VALUES
('Press de Banca con Barra', 1, 'https://www.simplyfitness.com/es/pages/barbell-bench-press'),
...
```

La lista declara **2 columnas** (`Nombre, IdGrupoMuscular`) pero cada fila aporta **3 valores**
(incluye el link). SQL Server rechaza todo el lote → **no se inserta ningún ejercicio**, lo que
deja vacía toda el área de Cliente (rutinas sin ejercicios, sesiones sin detalle, sin records).

**Corrección:** declarar la tercera columna:

```sql
INSERT INTO Ejercicios (Nombre, IdGrupoMuscular, LinkExplicacion)
VALUES
('Press de Banca con Barra', 1, 'https://www.simplyfitness.com/es/pages/barbell-bench-press'),
... ;
```

> Verificar que todos los links cumplan el `CHECK CK_Ejercicios_LinkExplicacion`
> (`LIKE 'https://www.simplyfitness.com/es/pages/%'`). El de "Fondos para Pecho"
> ([`Inserts.sql:77`](Inserts.sql:77)) usa solo el prefijo (página vacía) pero pasa el CHECK.

---

## Verificación sugerida

Como no se modificó código, la validación es de **revisión**:

1. Contrastar cada hallazgo contra el archivo y línea citados.
2. **DB:** ejecutar los scripts D1–D6 y el arreglo del apéndice en una copia de
   `GestionGimnasio` y comprobar:
   - `sp_CrearUsuario` propaga la excepción ante email duplicado (deja de "fallar en silencio").
   - El trigger `TR_SeriesCompletadas_Record` marca `EsRecordPersonal` al insertar una serie que
     supera el máximo histórico del usuario en ese ejercicio.
   - `SP_Suscripciones_MantenimientoDiario` promueve una `VIGENTE_PENDIENTE` cuya `FechaInicio`
     ya llegó.
   - El INSERT de `Ejercicios` corregido carga las 27 filas.
3. Los fixes de código (A1–A10, B1–B5) quedan listados para aplicarse en una próxima iteración
   si así se decide.
