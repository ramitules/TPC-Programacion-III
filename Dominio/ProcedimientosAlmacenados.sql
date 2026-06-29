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
	END CATCH
END
GO

-- Eliminar cliente (baja logica del usuario + borrado de sus dependencias)
CREATE PROCEDURE sp_EliminarCliente (
	@IdUsuario INTEGER
)
AS
BEGIN
	UPDATE Usuarios SET Activo = 0 WHERE IdUsuarios = @IdUsuario;
	DELETE FROM SeriesCompletadas WHERE IdSesion IN (SELECT IdSesionesEntrenamiento FROM SesionesEntrenamiento WHERE IdUsuario = @IdUsuario);
	DELETE FROM SesionesEntrenamiento WHERE IdUsuario = @IdUsuario;
	DELETE FROM RutinaEjercicios WHERE IdRutina IN (SELECT IdRutinas FROM Rutinas WHERE IdUsuario = @IdUsuario);
	DELETE FROM Rutinas WHERE IdUsuario = @IdUsuario;
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