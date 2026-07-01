USE GestionGimnasio;
GO

CREATE VIEW VW_Usuarios
AS
SELECT
    U.IdUsuarios,
    U.Nombre,
    U.Apellido,
    U.Email,
    U.FechaNacimiento,
    U.PesoCorporalKG,
    U.IdRol,
    R.Nombre AS [Rol Nombre],
    U.FechaIngreso,
    S.IdPlan,
    U.Activo,
    P.Nombre AS [Plan],
    P.PrecioMensual,
    P.DuracionDias,
    S.IdEstado,
    SE.Nombre AS [Estado],
    S.FechaInicio,
    S.FechaVencimiento,
    AU.Pass
FROM Usuarios U
LEFT JOIN Suscripciones S
    ON U.IdUsuarios = S.IdUsuario
LEFT JOIN Planes P
    ON P.IdPlanes = S.IdPlan
LEFT JOIN SuscripcionesEstados SE
    ON SE.IdSuscripcionesEstados = S.IdEstado
LEFT JOIN Roles R
    ON U.IdRol = R.IdRoles
LEFT JOIN AccesoUsuarios AU
    ON U.IdUsuarios = AU.IdUsuarios;
GO