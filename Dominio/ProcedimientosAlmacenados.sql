-- Obtener usuarios dependiendo del rol. Acepta la cadena 'TODOS' para obtener todos los usuarios sin filtro
CREATE PROCEDURE sp_ObtenerUsuarios (@Rol VARCHAR(50))
AS
BEGIN
	IF @Rol = 'TODOS'
		BEGIN
			SELECT ID, Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso
			FROM Usuarios
		END
	ELSE
		BEGIN
			SELECT ID, Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso
			FROM Usuarios
			INNER JOIN Roles
			ON Roles.ID = Usuarios.IdRol
			WHERE Roles.Rol = @Rol
		END
END
GO

-- Obtener un solo usuario
CREATE PROCEDURE sp_ObtenerUsuario (@ID INT)
AS
BEGIN
	SELECT ID, Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso
	FROM Usuarios
	WHERE ID = @ID
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