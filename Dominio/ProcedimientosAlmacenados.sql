-- Obtener usuarios dependiendo del rol. Acepta la cadena 'TODOS' para obtener todos los usuarios sin filtro
CREATE PROCEDURE sp_ObtenerUsuarios (@Rol VARCHAR(50))
AS
BEGIN
	IF @Rol = 'TODOS'
		BEGIN
			SELECT IdUsuarios, Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso, Activo
			FROM Usuarios
		END
	ELSE
		BEGIN
			SELECT IdUsuarios, Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso, Activo
			FROM Usuarios
			INNER JOIN Roles
			ON Roles.ID = Usuarios.IdRol
			WHERE Roles.Rol = @Rol
		END
END
GO

-- Obtener datos completos de las suscripciones de un usuario
CREATE PROCEDURE sp_SuscripcionCompleta (@ID INTEGER)
AS
BEGIN
	SELECT TOP 1
		S.IdSuscripciones as IdSuscripcion,
		S.FechaInicio as FechaInicio,
		S.FechaVencimiento as FechaVencimiento,
		S.IdPlan as IdPlan,
		S.IdEstado as IdEstado,
		P.Nombre as NombrePlan,
		P.PrecioMensual as PrecioPlan,
		P.DuracionDias as DuracionPlan
	FROM Suscripciones S
	LEFT JOIN Planes P
		ON Suscripciones.IdPlan = P.IdPlanes
	WHERE S.IdUsuario = @ID
	ORDER BY S.FechaVencimiento ASC
END
GO

-- Crear usuario
CREATE PROCEDURE sp_CrearUsuario (
	@Nombre VARCHAR(70),
	@Apellido VARCHAR(70),
	@Email VARCHAR(150),
	@FechaNacimiento DATETIME,
	@PesoCorporalKG DECIMAL(5, 2),
	@IdRol TINYINT,
	@FechaIngreso DATETIME
)
AS
BEGIN
	INSERT INTO Usuarios (Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso)
	VALUES (@Nombre, @Apellido, @Email, DATE(@FechaNacimiento), @PesoCorporalKG, @IdRol, @FechaIngreso)
END