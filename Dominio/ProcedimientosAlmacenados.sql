USE GestionGimnasio
GO

-- RUTINAS
-- Crear rutinas generales
CREATE PROCEDURE SP_Rutina_General
    @NOMBRE VARCHAR(150)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO Rutinas (Nombre, FechaCreacion) VALUES (@NOMBRE, GETDATE());
        COMMIT TRANSACTION;
        PRINT 'OK';
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;   
        PRINT 'Error al crear rutina: ' + ERROR_MESSAGE();
    END CATCH
END;

GO

-- Crear rutinas personalizada
CREATE PROCEDURE SP_Rutina_Personaliazda_Usuario
    @NOMBRE VARCHAR(150),
    @ID_USUARIO BIGINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
            INSERT INTO Rutinas (Nombre,IdUsuario, FechaCreacion) VALUES (@NOMBRE, @ID_USUARIO, GETDATE());
        COMMIT TRANSACTION;
        PRINT 'OK';
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;   
        PRINT 'Error al crear rutina personalizada: ' + ERROR_MESSAGE();
    END CATCH
END;

GO

-- Modificar rutina general
CREATE PROCEDURE SP_Modificar_Rutina_General
    @ID INT,
    @NOMBRE VARCHAR(150)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
            UPDATE Rutinas SET Nombre = @NOMBRE WHERE IdRutinas = @ID;
        COMMIT TRANSACTION;
        PRINT 'OK';
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;   
        PRINT 'Error al modificar la rutina: ' + ERROR_MESSAGE();
    END CATCH
END;

GO

-- Modificar rutina personalizada
CREATE PROCEDURE SP_Modificar_Rutina_Personaliazda_Usuario
    @ID INT,
    @ID_USUARIO BIGINT,
    @NOMBRE VARCHAR(150)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
            UPDATE Rutinas SET Nombre = @NOMBRE, IdUsuario = @ID_USUARIO WHERE IdRutinas = @ID;;
        COMMIT TRANSACTION;
        PRINT 'OK';
    END TRY
    BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;   
        PRINT 'Error al modificar la rutina personalizada: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-- Obtener los ejercicios (con detalle) de una rutina
CREATE PROCEDURE sp_EjerciciosDeRutina (@IdRutina INT)
AS
BEGIN
	SELECT
		RE.IdRutinasEjercicios  AS IdRutinaEjercicio,
		RE.ObjetivoKG           AS ObjetivoKG,
		RE.ObjetivoSeries       AS ObjetivoSeries,
		RE.ObjetivoRepeticiones AS ObjetivoRepeticiones,
		RE.OrdenEjercicio       AS OrdenEjercicio,
		E.IdEjercicios          AS IdEjercicio,
		E.Nombre                AS NombreEjercicio,
		E.LinkExplicacion       AS LinkExplicacion,
		GM.IdGruposMusculares   AS IdGrupoMuscular,
		GM.Nombre               AS NombreGrupoMuscular
	FROM RutinaEjercicios RE
	INNER JOIN Ejercicios E
		ON RE.IdEjercicio = E.IdEjercicios
	LEFT JOIN GruposMusculares GM
		ON E.IdGrupoMuscular = GM.IdGruposMusculares
	WHERE RE.IdRutina = @IdRutina
	ORDER BY RE.OrdenEjercicio
END
GO

-- SUSCRIPCIONES
-- Crear suscripcion
CREATE PROCEDURE sp_CrearSuscripcion (
	@IdUsuario INT,
	@IdPlan SMALLINT,
	@IdEstado TINYINT,
	@FechaInicio DATE,
	@FechaVencimiento DATE
)
AS
BEGIN
	INSERT INTO Suscripciones (IdUsuario, IdPlan, IdEstado, FechaInicio, FechaVencimiento)
	VALUES (@IdUsuario, @IdPlan, @IdEstado, @FechaInicio, @FechaVencimiento)
END
GO

-- Modificar suscripcion
CREATE PROCEDURE sp_ModificarSuscripcion (
	@IdUsuario INT,
	@IdPlan SMALLINT,
	@IdEstado TINYINT,
	@FechaInicio DATE,
	@FechaVencimiento DATE,
	@IdSuscripcion INT
)
AS
BEGIN
	UPDATE Suscripciones SET
		IdUsuario = @IdUsuario,
		IdPlan = @IdPlan,
		IdEstado = @IdEstado,
		FechaInicio = @FechaInicio,
		FechaVencimiento = @FechaVencimiento
	WHERE IdSuscripciones = @IdSuscripcion
END
GO

-- Actualizar el estado de las suscripciones vencidas
Create PROCEDURE SP_Suscripciones_actualizar_estado_vencidas
AS
BEGIN 
    UPDATE Suscripciones
    SET IdEstado = 2
    WHERE FechaVencimiento < GETDATE() AND IdEstado = 1;
END;
GO

-- Obtener datos completos de la suscripcion de un usuario
CREATE PROCEDURE sp_SuscripcionCompleta (@ID INTEGER, @IdEstado INTEGER)
AS
BEGIN
	SELECT TOP 1
		S.IdSuscripciones   as IdSuscripcion,
		S.FechaInicio       as FechaInicio,
		S.FechaVencimiento  as FechaVencimiento,
		S.IdPlan            as IdPlan,
		S.IdEstado          as IdEstado,
		P.Nombre            as NombrePlan,
		P.PrecioMensual     as PrecioPlan,
		P.DuracionDias      as DuracionPlan
	FROM Suscripciones S
	LEFT JOIN Planes P
		ON S.IdPlan = P.IdPlanes
	WHERE S.IdUsuario = @ID AND S.IdEstado = @IdEstado
	ORDER BY S.FechaVencimiento ASC
END
GO

-- USUARIOS
-- Creacion
CREATE PROCEDURE sp_CrearUsuario (
	@Nombre VARCHAR(70),
	@Apellido VARCHAR(70),
	@Email VARCHAR(150),
	@FechaNacimiento DATETIME,
	@PesoCorporalKG DECIMAL(5, 2),
	@IdRol TINYINT,
	@FechaIngreso DATETIME,
	@Pass VARCHAR(200)
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

-- Modificacion
CREATE PROCEDURE sp_ModificarUsuario (
	@Nombre VARCHAR(70),
	@Apellido VARCHAR(70),
	@Email VARCHAR(150),
	@FechaNacimiento DATETIME,
	@PesoCorporal DECIMAL(5, 2),
	@IdRol TINYINT,
	@FechaIngreso DATETIME,
	@Activo BIT,
	@IdUsuario INT,
	@Pass VARCHAR(200)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			UPDATE Usuarios SET
				Nombre = @Nombre, 
				Apellido = @Apellido, 
				Email = @Email, 
				FechaNacimiento = @FechaNacimiento,
				PesoCorporalKG = @PesoCorporal, 
				IdRol = @IdRol, 
				FechaIngreso = @FechaIngreso,
				Activo = @Activo
			WHERE IdUsuarios = @IdUsuario
			IF @Pass IS NOT NULL AND @Pass != ''
			BEGIN
				UPDATE AccesoUsuarios SET
					Pass = @Pass
				WHERE IdUsuarios = @IdUsuario
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO

-- Eliminar cliente (baja logica del usuario + borrado de sus dependencias)
CREATE PROCEDURE sp_EliminarCliente (
	@IdUsuario INTEGER
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			UPDATE Usuarios SET Activo = 0 WHERE IdUsuarios = @IdUsuario;
			DELETE FROM SeriesCompletadas WHERE IdSesion IN (SELECT IdSesionesEntrenamiento FROM SesionesEntrenamiento WHERE IdUsuario = @IdUsuario);
			DELETE FROM SesionesEntrenamiento WHERE IdUsuario = @IdUsuario;
			UPDATE Rutinas SET Activo = 0 WHERE IdUsuario = @IdUsuario;
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO

-- Obtener usuarios dependiendo del rol. Acepta la cadena 'TODOS' para obtener todos los usuarios sin filtro
CREATE PROCEDURE sp_ObtenerUsuarios (@Rol VARCHAR(50))
AS
BEGIN
	IF @Rol = 'TODOS'
		BEGIN
			SELECT * FROM Usuarios
		END
	ELSE
		BEGIN
			SELECT IdUsuarios, Usuarios.Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso, Activo
			FROM Usuarios
			LEFT JOIN Roles
				ON Roles.IdRoles = Usuarios.IdRol
			WHERE Roles.Nombre = @Rol
		END
END
GO

-- RECORDS
-- Obtener records personales del usuario: una fila por ejercicio = la serie record de mayor peso
-- (en caso de empate, la mas reciente). Filtra por el usuario dueño de la sesion.
CREATE PROCEDURE sp_RecordsPersonales (@ID INTEGER)
AS
BEGIN
	WITH RecordsRankeados AS (
		SELECT
			SC.PesoLevantadoKG		as PesoKG,
			SC.RepeticionesLogradas	as Repeticiones,
			SE.FechaHoraInicio		as FechaRecord,
			E.IdEjercicios			as IdEjercicio,
			E.Nombre				as EjercicioNombre,
			GM.IdGruposMusculares	as IdGrupoMuscular,
			GM.Nombre				as GrupoMuscularNombre,
			ROW_NUMBER() OVER (
				PARTITION BY E.IdEjercicios
				ORDER BY SC.PesoLevantadoKG DESC, SE.FechaHoraInicio DESC
			) as rn
		FROM SeriesCompletadas AS SC
		INNER JOIN SesionesEntrenamiento AS SE
			ON SE.IdSesionesEntrenamiento = SC.IdSesion
		INNER JOIN Ejercicios as E
			ON E.IdEjercicios = SC.IdEjercicio
		LEFT JOIN GruposMusculares as GM
			ON GM.IdGruposMusculares = E.IdGrupoMuscular
		WHERE SE.IdUsuario = @ID AND SC.EsRecordPersonal = 1
	)
	SELECT
		PesoKG,
		Repeticiones,
		FechaRecord,
		IdEjercicio,
		EjercicioNombre,
		IdGrupoMuscular,
		GrupoMuscularNombre
	FROM RecordsRankeados
	WHERE rn = 1
	ORDER BY EjercicioNombre
END
GO

-- SESIONES DE ENTRENAMIENTO
-- Creacion
CREATE PROCEDURE sp_CrearSesionEntrenamiento (
	@IdUsuario INT,
	@IdRutina INT,
	@FechaHoraInicio DATETIME,
	@FechaHoraFin DATETIME
)
AS
BEGIN
	SET NOCOUNT ON;
    INSERT INTO SesionesEntrenamiento (IdUsuario, IdRutina, FechaHoraInicio, FechaHoraFin)
    VALUES (@IdUsuario, @IdRutina, @FechaHoraInicio, @FechaHoraFin);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdSesion;
END
GO

-- Modificacion
CREATE PROCEDURE sp_ModificarSesionEntrenamiento (
	@IdSesion INT,
	@IdUsuario INT,
	@IdRutina INT,
	@FechaHoraInicio DATETIME,
	@FechaHoraFin DATETIME
)
AS
BEGIN
	UPDATE SesionesEntrenamiento SET
		IdUsuario = @IdUsuario,
		IdRutina = @IdRutina,
		FechaHoraInicio = @FechaHoraInicio,
		FechaHoraFin = @FechaHoraFin
	WHERE IdSesionesEntrenamiento = @IdSesion
END
GO

-- SERIES COMPLETADAS
-- Creacion (se inserta una fila por cada serie que el cliente completa durante la sesion)
CREATE PROCEDURE sp_CrearSerieCompletada (
	@IdSesion INT,
	@IdEjercicio INT,
	@PesoLevantadoKG DECIMAL(6, 2),
	@RepeticionesLogradas SMALLINT,
	@RIR TINYINT,
	@EsRecordPersonal BIT
)
AS
BEGIN
	INSERT INTO SeriesCompletadas (IdSesion, IdEjercicio, PesoLevantadoKG, RepeticionesLogradas, RIR, EsRecordPersonal)
	VALUES (@IdSesion, @IdEjercicio, @PesoLevantadoKG, @RepeticionesLogradas, @RIR, @EsRecordPersonal)
END
GO

-- Obtener todas las sesiones de un cliente, con el nombre de la rutina y la cantidad de series registradas.
-- Liviano: una fila por sesion (la metadata). Las series se cargan aparte y bajo demanda.
CREATE PROCEDURE sp_SesionesDeCliente (@IdUsuario INT)
AS
BEGIN
	SELECT
		SE.IdSesionesEntrenamiento	AS IdSesion,
		SE.FechaHoraInicio			AS FechaHoraInicio,
		SE.FechaHoraFin				AS FechaHoraFin,
		SE.IdRutina					AS IdRutina,
		R.Nombre					AS NombreRutina,
		(SELECT COUNT(*) FROM SeriesCompletadas SC WHERE SC.IdSesion = SE.IdSesionesEntrenamiento) AS CantidadSeries
	FROM SesionesEntrenamiento SE
	LEFT JOIN Rutinas R
		ON SE.IdRutina = R.IdRutinas
	WHERE SE.IdUsuario = @IdUsuario
	ORDER BY SE.FechaHoraInicio DESC
END
GO

-- Obtener la sesion de entrenamiento en curso de un cliente (si existe).
-- Una sesion se considera "en curso" mientras no fue finalizada: sp_CrearSesionEntrenamiento
-- la crea con FechaHoraFin = FechaHoraInicio, y FinSesionEntrenamiento recien ahi actualiza FechaHoraFin.
CREATE PROCEDURE sp_SesionActivaDeCliente (@IdUsuario INT)
AS
BEGIN
	SELECT TOP 1
		SE.IdSesionesEntrenamiento	AS IdSesion,
		SE.FechaHoraInicio			AS FechaHoraInicio,
		SE.FechaHoraFin				AS FechaHoraFin,
		SE.IdRutina					AS IdRutina,
		R.Nombre					AS NombreRutina
	FROM SesionesEntrenamiento SE
	LEFT JOIN Rutinas R
		ON SE.IdRutina = R.IdRutinas
	WHERE SE.IdUsuario = @IdUsuario
		AND SE.FechaHoraFin = SE.FechaHoraInicio
	ORDER BY SE.FechaHoraInicio DESC
END
GO

-- Obtener las series completadas de una sesion, con el ejercicio y su grupo muscular.
-- Ordenadas por nombre de ejercicio para que el agrupado quede contiguo.
CREATE PROCEDURE sp_SeriesDeSesionAgrupadas (@IdSesion INT)
AS
BEGIN
	SELECT
		E.IdEjercicios			AS IdEjercicio,
		E.Nombre				AS NombreEjercicio,
		GM.Nombre				AS NombreGrupoMuscular,
		SC.PesoLevantadoKG		AS PesoLevantadoKG,
		SC.RepeticionesLogradas	AS RepeticionesLogradas,
		SC.RIR					AS RIR,
		SC.EsRecordPersonal		AS EsRecordPersonal
	FROM SeriesCompletadas SC
	INNER JOIN Ejercicios E
		ON SC.IdEjercicio = E.IdEjercicios
	LEFT JOIN GruposMusculares GM
		ON E.IdGrupoMuscular = GM.IdGruposMusculares
	WHERE SC.IdSesion = @IdSesion
	ORDER BY E.Nombre, SC.IdSeriesCompletadas
END
GO

--Busca al usuario por Email y trae sus atributos junto con el hash de la contrasenia.
--La validacion de la contrasenia se hace en la capa de aplicacion (PBKDF2), no en SQL.
CREATE PROCEDURE sp_logueo
	@Email VARCHAR(100)
AS
BEGIN
SELECT IdUsuarios,
Nombre,
Apellido,
Email,
FechaNacimiento,
PesoCorporalKG,
IdRol,
[Rol Nombre],
FechaIngreso,
IdPlan, [Plan],
Activo,
PrecioMensual,
DuracionDias,
IdEstado, Estado,
FechaInicio,
FechaVencimiento,
Pass
FROM VW_Usuarios
WHERE Email = @Email AND Activo = 1
END
GO

--Recuperacion de contraseña: verifica si existe un usuario activo con ese email,
--sin revelar el resultado al usuario final (se usa solo para decidir si se manda el mail).
CREATE PROCEDURE sp_ExisteEmail
	@Email VARCHAR(150)
AS
BEGIN
	SELECT IdUsuarios FROM Usuarios WHERE Email = @Email AND Activo = 1
END
GO

--Guarda el hash del codigo de recuperacion de 6 digitos y su vencimiento para el usuario con ese email.
CREATE PROCEDURE sp_GuardarCodigoReset
	@Email VARCHAR(150),
	@CodigoHash VARCHAR(200),
	@Expira DATETIME
AS
BEGIN
	UPDATE AccesoUsuarios
	SET CodigoReset = @CodigoHash, CodigoResetExpira = @Expira, CodigoResetIntentos = 0
	WHERE IdUsuarios = (SELECT IdUsuarios FROM Usuarios WHERE Email = @Email AND Activo = 1)
END
GO

--Trae el hash del codigo de recuperacion vigente para el email.
CREATE PROCEDURE sp_ObtenerCodigoReset
	@Email VARCHAR(150)
AS
BEGIN
	SELECT a.IdUsuarios, a.CodigoReset, a.CodigoResetExpira, a.CodigoResetIntentos
	FROM AccesoUsuarios a
	INNER JOIN Usuarios u ON u.IdUsuarios = a.IdUsuarios
	WHERE u.Email = @Email AND u.Activo = 1
END
GO

--Registra un intento fallido de codigo de recuperacion (para cortar por fuerza bruta).
CREATE PROCEDURE sp_IncrementarIntentosCodigoReset
	@IdUsuarios INT
AS
BEGIN
	UPDATE AccesoUsuarios SET CodigoResetIntentos = CodigoResetIntentos + 1 WHERE IdUsuarios = @IdUsuarios
END
GO

--Aplica la nueva contrasenia (ya hasheada) y limpia el codigo de recuperacion usado.
CREATE PROCEDURE sp_ActualizarPasswordConCodigo
	@IdUsuarios INT,
	@PassHash VARCHAR(200)
AS
BEGIN
	UPDATE AccesoUsuarios
	SET Pass = @PassHash, CodigoReset = NULL, CodigoResetExpira = NULL, CodigoResetIntentos = 0
	WHERE IdUsuarios = @IdUsuarios
END
GO

--Crear Usuario (Admin, Entrenador)
CREATE PROCEDURE sp_CrearUsuario (
	@Nombre VARCHAR(70),
	@Apellido VARCHAR(70),
	@Email VARCHAR(150),
	@FechaNacimiento DATETIME,
	@PesoCorporalKG DECIMAL(5, 2),
	@IdRol TINYINT,
	@FechaIngreso DATETIME,
	@Pass VARCHAR(40)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			DECLARE @IdUsuario INT; 
			INSERT INTO Usuarios (Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso)
			VALUES (@Nombre, @Apellido, @Email, @FechaNacimiento, @PesoCorporalKG, @IdRol, @FechaIngreso)
			SET @IdUsuario = (SELECT SCOPE_IDENTITY());
			INSERT INTO AccesoUsuarios (IdUsuarios, Pass)
			VALUES (@IdUsuario, @Pass)
		COMMIT TRANSACTION
		SELECT @IdUsuario
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;   
        PRINT 'Error al registrar usuario: ' + ERROR_MESSAGE();
	END CATCH
END;
GO


--Modificar Usuario (Admin, Entrenador)
CREATE PROCEDURE sp_ModificarUsuario (
	@Nombre VARCHAR(70),
	@Apellido VARCHAR(70),
	@Email VARCHAR(150),
	@FechaNacimiento DATETIME,
	@PesoCorporal DECIMAL(5, 2),
	@IdRol TINYINT,
	@FechaIngreso DATETIME,
	@Activo BIT,
	@IdUsuario INT,
	@Pass VARCHAR(150)
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION
			UPDATE Usuarios SET
				Nombre = @Nombre, 
				Apellido = @Apellido, 
				Email = @Email, 
				FechaNacimiento = @FechaNacimiento,
				PesoCorporalKG = @PesoCorporal, 
				IdRol = @IdRol, 
				FechaIngreso = @FechaIngreso,
				Activo = @Activo
			WHERE IdUsuarios = @IdUsuario
			IF @Pass IS NOT NULL AND @Pass != ''
			BEGIN
				UPDATE AccesoUsuarios SET
					Pass = @Pass
				WHERE IdUsuarios = @IdUsuario
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
	END CATCH
END;
GO

--Trae todos los usuarios con rol de Admin
CREATE PROCEDURE sp_Traer_Admins
AS
  BEGIN
    SELECT U.IdUsuarios, 
    U.Nombre, 
    U.Apellido, 
    U.Email, 
    U.FechaNacimiento AS [Fecha de Nacimiento],
    U.IdRol,
    R.Nombre AS [Rol],
    U.FechaIngreso AS [Fecha de Ingreso],
    U.Activo
    FROM Usuarios U
    INNER JOIN Roles R ON U.IdRol = R.IdRoles
    WHERE U.IdRol = 1
END;
GO

--Trae todos los usuarios con rol de Cliente
CREATE PROCEDURE sp_Traer_Clientes
AS
  BEGIN
    SELECT U.IdUsuarios, 
    U.Nombre, 
    U.Apellido, 
    U.Email, 
    U.FechaNacimiento AS [Fecha de Nacimiento],
    U.IdRol,
    R.Nombre AS [Rol],
    U.FechaIngreso AS [Fecha de Ingreso],
    U.PesoCorporalKG,
    S.IdEstado,
    SE.Nombre,
    U.Activo
    FROM Usuarios U
    LEFT JOIN Roles R ON U.IdRol = R.IdRoles
    LEFT JOIN Suscripciones S ON S.IdUsuario = U.IdUsuarios
    LEFT JOIN SuscripcionesEstados SE ON SE.IdSuscripcionesEstados = S.IdEstado
    WHERE U.IdRol = 3
END;
GO

--Trae todos los usuarios con rol de Entrenador
CREATE PROCEDURE sp_Traer_Entrenadores
AS
  BEGIN
    SELECT U.IdUsuarios, 
    U.Nombre, 
    U.Apellido, 
    U.Email, 
    U.FechaNacimiento AS [Fecha de Nacimiento],
    U.IdRol,
    R.Nombre AS [Rol],
    U.FechaIngreso AS [Fecha de Ingreso],
    U.Activo
    FROM Usuarios U
    INNER JOIN Roles R ON U.IdRol = R.IdRoles
    WHERE U.IdRol = 4
END;
GO


--Trae todos los ejercicios con su grupo muscular
CREATE PROCEDURE sp_Traer_Ejercicios
AS
BEGIN
SELECT E.IdEjercicios, E.Nombre, E.IdGrupoMuscular, GM.Nombre AS GrupoMuscular, E.LinkExplicacion, E.Activo FROM Ejercicios E
INNER JOIN GruposMusculares GM ON E.IdGrupoMuscular = GM.IdGruposMusculares
END;
GO


--Trae todos los estados de suscripciones
CREATE PROCEDURE sp_Traer_Estados_De_Suscripciones
AS 
BEGIN
SELECT IdSuscripcionesEstados, Nombre FROM SuscripcionesEstados
END;
GO


--Trae todos los grupos musculares
CREATE PROCEDURE sp_Traer_Grupos_Musculares
AS
BEGIN
SELECT IdGruposMusculares, Nombre, Activo FROM GruposMusculares
END;
GO


--Trae todos los planes
CREATE PROCEDURE sp_Traer_Planes
AS
BEGIN
SELECT IdPlanes, Nombre, PrecioMensual, DuracionDias, Activo FROM Planes
END;
GO


--Inactiva un usuario (baja logica)
CREATE PROCEDURE sp_Inactivar_Usuario
  @IdUsuario INT
AS
  BEGIN
	UPDATE Usuarios SET Activo = 0 WHERE IdUsuarios = @IdUsuario
END;
GO


--Activa un usuario (alta logica)
CREATE PROCEDURE sp_Activa_Usuario
  @IdUsuario INT
AS
  BEGIN
    UPDATE Usuarios SET Activo = 1 WHERE IdUsuarios = @IdUsuario
END;
GO


--Activa e inactiva un usuario dependiendo de su estado actual (si esta activo lo inactiva y viceversa)
CREATE PROCEDURE sp_Activar_Inactivar_Usuario
  @IdUsuario INT
AS
  BEGIN
	DECLARE @Activo INT
    SET @Activo = (SELECT Activo FROM Usuarios WHERE IdUsuarios = @IdUsuario)
	IF @Activo = 1
	  BEGIN
	    UPDATE Usuarios SET Activo = 0 WHERE IdUsuarios = @IdUsuario
	  END
	ELSE
	  BEGIN
	    UPDATE Usuarios SET Activo = 1 WHERE IdUsuarios = @IdUsuario
	  END
END;
GO

--Activa e inactiva ejercicios segun su estado
CREATE PROCEDURE sp_Activar_Inactivar_Ejercicio
  @IdEjercicios INT
AS
  BEGIN
	DECLARE @Activo INT
    SET @Activo = (SELECT Activo FROM Ejercicios WHERE IdEjercicios = @IdEjercicios)
	IF @Activo = 1
	  BEGIN
	    UPDATE Ejercicios SET Activo = 0 WHERE IdEjercicios = @IdEjercicios
	  END
	ELSE
	  BEGIN
	    UPDATE Ejercicios SET Activo = 1 WHERE IdEjercicios = @IdEjercicios
	  END
END;
GO


--Activa e inactiva planes segun su estado
CREATE PROCEDURE sp_Activar_Inactivar_Planes
  @IdPlanes INT
AS
  BEGIN
	DECLARE @Activo INT
    SET @Activo = (SELECT Activo FROM Planes WHERE IdPlanes = @IdPlanes)
	IF @Activo = 1
	  BEGIN
	    UPDATE Planes SET Activo = 0 WHERE IdPlanes = @IdPlanes
	  END
	ELSE
	  BEGIN
	    UPDATE Planes SET Activo = 1 WHERE IdPlanes = @IdPlanes
	  END
END;
GO


--Activa e inactiva grupos musculares segun su estado
CREATE PROCEDURE sp_Activar_Inactivar_Grupos_Musculares
  @IdGruposMusculares INT
AS
  BEGIN
	DECLARE @Activo INT
    SET @Activo = (SELECT Activo FROM GruposMusculares WHERE IdGruposMusculares = @IdGruposMusculares)
	IF @Activo = 1
	  BEGIN
	    UPDATE GruposMusculares SET Activo = 0 WHERE IdGruposMusculares = @IdGruposMusculares
	  END
	ELSE
	  BEGIN
	    UPDATE GruposMusculares SET Activo = 1 WHERE IdGruposMusculares = @IdGruposMusculares
	  END
END;
GO


CREATE PROCEDURE sp_Crear_Plan
  @Nombre VARCHAR (50),
  @PrecioMensual DECIMAL(8,2),
  @DuracionDias INT
AS
  BEGIN
    INSERT INTO Planes (Nombre, PrecioMensual, DuracionDias) 
	       VALUES(@Nombre, @PrecioMensual, @DuracionDias)
END;
GO


CREATE PROCEDURE sp_Crear_Grupo_Muscular
  @Nombre VARCHAR (50)
AS
  BEGIN
    INSERT INTO GruposMusculares(Nombre) 
	       VALUES(@Nombre)
END;
GO


CREATE PROCEDURE sp_Crear_Ejercicios
  @Nombre VARCHAR (50),
  @IdGrupoMuscular INT,
  @LinkExplicacion VARCHAR(250)
AS
  BEGIN
    INSERT INTO Ejercicios (Nombre, IdGrupoMuscular, LinkExplicacion) 
	       VALUES(@Nombre, @IdGrupoMuscular, @LinkExplicacion)
END;
GO


CREATE PROCEDURE sp_Modificar_Grupo_Muscular
  @IdGrupoMuscular INT,
  @Nombre VARCHAR(50)
AS
  BEGIN
    UPDATE GruposMusculares SET Nombre = @Nombre WHERE IdGruposMusculares = @IdGrupoMuscular
END;
GO


CREATE PROCEDURE sp_Modificar_Ejercicio
  @IdEjercicio INT,
  @Nombre VARCHAR(50),
  @IdGrupoMuscular INT,
  @LinkExplicacion VARCHAR(250)
AS
  BEGIN
    UPDATE Ejercicios SET Nombre = @Nombre, IdGrupoMuscular = @IdGrupoMuscular, LinkExplicacion = @LinkExplicacion WHERE IdEjercicios = @IdEjercicio
END;
GO


CREATE PROCEDURE sp_Modificar_Plan
  @IdPlan INT,
  @Nombre VARCHAR(50),
  @PrecioMensual DECIMAL (8,2),
  @DuracionDias VARCHAR(250)
AS
  BEGIN
    UPDATE Planes SET Nombre = @Nombre, PrecioMensual = @PrecioMensual, DuracionDias = @DuracionDias WHERE IdPlanes = @IdPlan
END;
GO