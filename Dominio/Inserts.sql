USE GestionGimnasio;
GO

--DATOS PARA CARGAR EN LAS TABLAS--

INSERT INTO GruposMusculares (Nombre)
VALUES 
('Pecho / Pectorales'),
('Espalda / Dorsales'),
('Piernas / Tren Inferior'),
('Hombros / Deltoides'),
('Brazos (Bíceps/Tríceps)'),
('Core / Abdominales');
GO

INSERT INTO SuscripcionesEstados (Nombre)
VALUES 
('Activa'),
('Vencida'),
('Cancelada'),
('Activa pendiente');
GO

INSERT INTO Roles 
	(Nombre) 
VALUES
	('Admin'),
	('Administrativo'),
	('Cliente'),
	('Entrenador');
GO

INSERT INTO Planes (Nombre, PrecioMensual, DuracionDias)
VALUES 
('Pase Diario', 2500.00, 1),
('Plan Mensual Estándar', 18000.00, 30),
('Plan Mensual Pase Libre', 22000.00, 30),
('Trimestre Promocional', 15000.00, 90),
('Pase Libre Semestral', 13500.00, 180);
GO

INSERT INTO Usuarios (Nombre, Apellido, Email, FechaNacimiento, PesoCorporalKG, IdRol, FechaIngreso, Activo)
VALUES 

-- Administradores (IdRol = 1, Peso = 0)
('Alejandro', 'Rossi', 'a.rossi@gym.com', '1988-04-12', 0.00, 1, '2025-01-10 09:00:00', 1),
('Mariana', 'Fernández', 'm.fernandez@gym.com', '1992-09-25', 0.00, 1, '2025-02-15 14:30:00', 1),
-- Profesores / Entrenadores (IdRol = 4, Peso = 0)
('Carlos', 'Mendoza', 'c.mendoza@gym.com', '1985-11-05', 0.00, 4, '2025-03-01 08:15:00', 1),
('Laura', 'Gómez', 'l.gomez@gym.com', '1990-06-20', 0.00, 4, '2025-06-10 16:00:00', 1),
('Christian', 'Díaz', 'c.diaz@gym.com', '1993-03-14', 0.00, 4, '2025-08-22 07:30:00', 1),
-- Administrativos / Recepción (IdRol = 2, Peso = 0)
('Florencia', 'Herrera', 'f.herrera@gym.com', '1995-05-14', 0.00, 2, '2026-01-20 11:45:00', 1),
('Diego', 'Romero', 'd.romero@gym.com', '1991-12-01', 0.00, 2, '2026-02-02 16:00:00', 1),
-- Clientes (IdRol = 3, Poseen Peso Corporal Real)
('Sofía', 'Benítez', 'sofia.b@email.com', '1998-07-19', 58.70, 3, '2026-01-05 10:00:00', 1),
('Lucas', 'Giménez', 'lucas.g@email.com', '1994-02-28', 78.40, 3, '2026-01-12 18:20:00', 1),
('Camila', 'Maidana', 'camila.m@email.com', '2000-03-22', 55.20, 3, '2026-02-10 09:30:00', 1),
('Martín', 'Silva', 'martin.s@email.com', '1987-08-30', 95.10, 3, '2026-02-18 20:15:00', 1),
('Valentina', 'Ríos', 'vale.rios@email.com', '1996-10-10', 64.80, 3, '2026-03-01 07:00:00', 1),
('Facundo', 'Castro', 'facu.c@email.com', '1993-01-15', 83.90, 3, '2026-03-05 15:30:00', 1),
('Julieta', 'Acosta', 'juli.acosta@email.com', '1997-11-23', 61.30, 3, '2026-03-12 08:00:00', 1),
('Gonzalo', 'Pereyra', 'gonza.p@email.com', '1991-04-05', 89.50, 3, '2026-03-18 19:45:00', 1),
('Agustina', 'Sánchez', 'agus.s@email.com', '1999-09-02', 53.40, 3, '2026-03-25 10:30:00', 1),
('Ezequiel', 'López', 'eze.lopez@email.com', '1990-02-17', 102.60, 3, '2026-04-02 21:00:00', 1),
('Micaela', 'Suárez', 'mica.s@email.com', '2001-01-28', 57.00, 3, '2026-04-10 14:15:00', 1),
('Tomás', 'Verón', 'tomas.v@email.com', '1989-06-11', 76.20, 3, '2026-04-15 17:00:00', 1),
('Natalia', 'Blanco', 'naty.b@email.com', '1994-10-05', 68.90, 3, '2026-04-20 09:15:00', 1),
('Bruno', 'Navarro', 'bruno.n@email.com', '1996-04-18', 81.50, 3, '2026-05-01 09:00:00', 1),
('Melina', 'Quiroga', 'melina.q@email.com', '1998-12-03', 59.80, 3, '2026-05-03 18:30:00', 1),
('Kevin', 'Morales', 'kevin.m@email.com', '1992-07-25', 88.10, 3, '2026-05-05 11:00:00', 1),
('Rocío', 'Peralta', 'rocio.p@email.com', '2000-02-11', 54.60, 3, '2026-05-10 17:45:00', 1),
('Nicolás', 'Vega', 'nico.v@email.com', '1995-09-08', 77.30, 3, '2026-05-12 20:00:00', 1),
('Carla', 'Figueroa', 'carla.f@email.com', '1997-06-21', 62.40, 3, '2026-05-15 08:15:00', 1),
('Matías', 'Ruiz', 'matias.r@email.com', '1993-11-17', 84.70, 3, '2026-05-18 19:20:00', 1),
('Luciana', 'Paz', 'luciana.p@email.com', '1999-08-09', 56.90, 3, '2026-05-20 10:00:00', 1),
('Federico', 'Sosa', 'fede.s@email.com', '1991-03-30', 91.20, 3, '2026-05-22 18:40:00', 1),
('Antonella', 'Molina', 'anto.m@email.com', '2001-01-14', 52.80, 3, '2026-05-25 14:30:00', 1);
GO

INSERT INTO Ejercicios (Nombre, IdGrupoMuscular, LinkExplicacion)
VALUES 
('Press de Banca con Barra', 1, 'https://www.simplyfitness.com/es/pages/barbell-bench-press'),
('Press Inclinado con Mancuernas', 1, 'https://www.simplyfitness.com/es/pages/incline-dumbbell-bench-press'),
('Aperturas en Peck Deck', 1, 'https://www.simplyfitness.com/es/pages/peck-deck'),
('Press Declinado con Barra', 1, 'https://www.simplyfitness.com/es/pages/barbell-declined-bench-press'),
('Fondos para Pecho', 1, 'https://www.simplyfitness.com/es/pages/'),

('Dominadas Pronas', 2, 'https://www.simplyfitness.com/es/pages/pull-up'),
('Remo con Barra', 2, 'https://www.simplyfitness.com/es/pages/barbell-row'),
('Jalón al Pecho en Polea', 2, 'https://www.simplyfitness.com/es/pages/wide-grip-pulldown'),
('Remo en Máquina', 2, 'https://www.simplyfitness.com/es/pages/seated-cable-row'),
('Pullover en Polea', 2, 'https://www.simplyfitness.com/es/pages/barbell-pullover'),

('Sentadilla Libre con Barra', 3, 'https://www.simplyfitness.com/es/pages/squat'),
('Prensa de Piernas 45 Grados', 3, 'https://www.simplyfitness.com/es/pages/leg-press'),
('Sillón de Extensiones de Cuádriceps', 3, 'https://www.simplyfitness.com/es/pages/leg-extension'),
('Peso Muerto Rumano con Mancuernas', 3, 'https://www.simplyfitness.com/es/pages/dumbbell-stiff-leg-deadlift'),
('Elevación de Gemelos de Pie', 3, 'https://www.simplyfitness.com/es/pages/standing-calf-raise'),

('Press Militar con Barra', 4, 'https://www.simplyfitness.com/es/pages/barbell-push-press'),
('Vuelos Laterales con Mancuernas', 4, 'https://www.simplyfitness.com/es/pages/dumbbell-lateral-raise'),
('Pájaros en Polea (Deltoides Posterior)', 4, 'https://www.simplyfitness.com/es/pages/bent-over-lateral-raise'),
('Elevaciones Frontales con Mancuernas', 4, 'https://www.simplyfitness.com/es/pages/dumbbell-front-raise'),

('Curl de Bíceps con Barra W', 5, 'https://www.simplyfitness.com/es/pages/ez-barbell-curl'),
('Curl de Bíceps en Banco Scott', 5, 'https://www.simplyfitness.com/es/pages/ez-barbell-preacher-curl'),
('Extensiones de Tríceps en Polea Alta', 5, 'https://www.simplyfitness.com/es/pages/triceps-pressdown'),
('Fondos en Paralelas para Tríceps', 5, 'https://www.simplyfitness.com/es/pages/parallel-dip-bar'),
('Curl Martillo', 5, 'https://www.simplyfitness.com/es/pages/hammer-curl'),
('Press Francés', 5, 'https://www.simplyfitness.com/es/pages/seated-barbell-french-press'),

('Plancha Abdominal Isométrica', 6, 'https://www.simplyfitness.com/es/pages/plank'),
('Elevaciones de Piernas Colgado', 6, 'https://www.simplyfitness.com/es/pages/hanging-leg-raise');
GO

INSERT INTO Rutinas (Nombre, IdUsuario, FechaCreacion, Dia, Activo)
VALUES 
-- Rutinas generales (IdUsuario es NULL)
('Rutina Fullbody Principiantes', NULL, '2025-01-15 10:00:00', 'Lunes', 1),
('Torso / Pierna Avanzado', NULL, '2025-02-20 11:30:00', 'Martes', 1),
('Rutina de Fuerza (5x5)', NULL, '2025-03-05 09:15:00', NULL, 1),

----
('Push Pull Legs', NULL, '2026-04-01 10:00:00', NULL, 1),
('Hipertrofia General', NULL, '2026-04-05 09:00:00', NULL, 1),
('Entrenamiento Funcional', NULL, '2026-04-10 18:00:00', NULL, 1),
('Pérdida de Peso', NULL, '2026-04-15 16:00:00', NULL, 1),
('Ganancia de Masa Muscular', NULL, '2026-04-20 11:00:00', NULL, 1),
('Fullbody Intermedio', NULL, '2026-04-25 10:30:00', NULL, 1),
('Entrenamiento Express 45 Minutos', NULL, '2026-05-01 17:00:00', NULL, 1),

-- Rutinas personalizadas (IDs del script de Usuarios)
('Hipertrofia Piernas - Sofía', 8, '2026-01-06 18:00:00', 'Lunes', 1),
('Acondicionamiento General - Lucas', 5, '2026-01-13 19:45:00', NULL, 1),
('Definición / Quema Calórica - Florencia', 6, '2026-01-22 15:30:00', NULL, 1),
('Fuerza Máxima - Diego', 7, '2026-02-03 20:00:00', NULL, 1),
('Rutina Adaptada - Martín', 9, '2026-02-19 10:20:00', NULL, 1),

----
('Hipertrofia Tren Superior - Bruno', 21, '2026-05-03 18:00:00', 'Lunes', 1),
('Fuerza y Potencia - Kevin', 23, '2026-05-08 20:00:00', 'Miércoles', 1),
('Definición Muscular - Rocío', 24, '2026-05-12 17:00:00', 'Viernes', 1),
('Plan Integral - Nicolás', 25, '2026-05-15 19:00:00', 'Martes', 1),
('Ganancia Muscular - Carla', 26, '2026-05-18 18:00:00', 'Lunes', 1),
('Preparación Física - Matías', 27, '2026-05-20 20:00:00', 'Jueves', 1),
('Tonificación General - Luciana', 28, '2026-05-22 17:30:00', 'Miércoles', 1),
('Entrenamiento de Fuerza - Federico', 29, '2026-05-24 19:00:00', 'Martes', 1),
('Acondicionamiento Inicial - Antonella', 30, '2026-05-26 16:00:00', 'Viernes', 1);
GO

INSERT INTO Suscripciones (IdUsuario, IdPlan, IdEstado, FechaInicio, FechaVencimiento)
VALUES 
(9, 5, 1, '2026-02-18', '2026-08-17'), 
(10, 2, 1, '2026-04-25', '2026-05-25'),
(11, 3, 1, '2026-05-05', '2026-06-04'), 
(12, 2, 1, '2026-05-12', '2026-06-11'), 
(13, 2, 1, '2026-05-15', '2026-06-14'), 
(14, 1, 2, '2026-04-02', '2026-04-03'),
(15, 1, 2, '2026-04-10', '2026-04-11'),

--
(21, 3, 1, '2026-05-01', '2026-06-30'),
(22, 2, 1, '2026-05-03', '2026-07-02'),
(23, 5, 1, '2026-05-05', '2026-11-01'),
(24, 2, 4, '2026-06-20', '2026-07-20'),
(25, 3, 3, '2026-05-10', '2026-06-09'),
(26, 2, 1, '2026-05-15', '2026-06-14'),
(27, 3, 1, '2026-05-18', '2026-06-17'),
(28, 4, 1, '2026-05-20', '2026-08-18'),
(29, 5, 1, '2026-05-22', '2026-11-18'),
(30, 2, 2, '2026-04-01', '2026-05-01');
GO


INSERT INTO RutinaEjercicios
(IdEjercicio, IdRutina, ObjetivoKG, ObjetivoSeries, ObjetivoRepeticiones, OrdenEjercicio)
VALUES
(7,  3, 10,	4, 10, 1),
(1,  3, 10,	4, 12, 2),
(6,  3, 10,	4, 10, 3),
(12, 2, 10,	4, 12, 4),
(18, 3, 10,	4, 45, 5),
(1,  4, 10,	4, 8,  1),
(5,  4, 10,	4, 8,  2),
(11, 3, 10,	4, 10, 3),
(14, 3, 10,	4, 12, 4),
(16, 3, 10,	4, 12, 5),
(7,  4, 10,	4, 8,  1),
(8,  3, 10,	4, 12, 2),
(10, 4, 10,	4, 10, 3),
(9,  3, 10,	4, 15, 4),
(7,  5, 10,	4, 5,  1),
(1,  5, 10,	4, 5,  2),
(5,  5, 10,	4, 5,  3),

(1, 9, 60, 4, 8, 1),
(5, 9, 50, 4, 10, 2),
(11, 9, 35, 4, 10, 3),
(14, 9, 25, 3, 12, 4),
(18, 9, 0, 3, 60, 5),

(2, 10, 22, 4, 12, 1),
(6, 10, 50, 4, 12, 2),
(8, 10, 140, 4, 10, 3),
(12, 10, 10, 4, 15, 4),
(15, 10, 20, 4, 12, 5),

(7, 11, 40, 3, 15, 1),
(4, 11, 0, 3, 10, 2),
(20, 11, 15, 3, 15, 3),

(1, 16, 70, 4, 8, 1),
(2, 16, 24, 4, 10, 2),
(11, 16, 40, 4, 8, 3),
(23, 16, 16, 3, 12, 4),

(7, 17, 100, 5, 5, 1),
(10, 17, 40, 5, 8, 2),
(5, 17, 80, 5, 5, 3),
(1, 17, 90, 5, 5, 4),

(8, 19, 180, 4, 10, 1),
(9, 19, 45, 4, 12, 2),
(24, 19, 35, 4, 10, 3);
GO


INSERT INTO SesionesEntrenamiento (IdUsuario, IdRutina, FechaHoraInicio, FechaHoraFin)
VALUES 
(9, 8, '2026-02-22 09:30:00', '2026-02-22 10:45:00'),
(10, NULL, '2026-04-26 08:00:00', '2026-04-26 09:00:00'),
(11, 2, '2026-05-08 17:00:00', '2026-05-08 18:20:00'),
(12, 2, '2026-05-14 19:15:00', '2026-05-14 20:30:00'),
(13, 9, '2026-06-01 18:00:00', '2026-06-01 19:20:00'),
(14, 10, '2026-06-03 19:00:00', '2026-06-03 20:15:00'),
(15, 11, '2026-06-05 17:00:00', '2026-06-05 18:10:00'),

(21, 16, '2026-05-04 18:00:00', '2026-05-04 19:20:00'),
(21, 16, '2026-05-08 18:10:00', '2026-05-08 19:35:00'),
(21, 16, '2026-05-12 18:00:00', '2026-05-12 19:15:00'),
(21, 16, '2026-05-16 18:00:00', '2026-05-16 19:20:00'),

(22, 10, '2026-05-05 17:00:00', '2026-05-05 18:10:00'),
(22, 10, '2026-05-09 17:10:00', '2026-05-09 18:30:00'),

(23, 17, '2026-05-07 20:00:00', '2026-05-07 21:30:00'),
(23, 17, '2026-05-11 20:15:00', '2026-05-11 21:40:00'),
(23, 17, '2026-05-15 20:00:00', '2026-05-15 21:20:00'),
(23, 17, '2026-05-18 20:00:00', '2026-05-18 21:25:00'),

(24, 18, '2026-05-12 17:00:00', '2026-05-12 18:10:00'),
(24, 18, '2026-05-16 17:00:00', '2026-05-16 18:15:00'),

(25, 19, '2026-05-15 19:00:00', '2026-05-15 20:20:00'),
(25, 19, '2026-05-20 19:00:00', '2026-05-20 20:15:00'),

(26, 20, '2026-05-18 18:00:00', '2026-05-18 19:15:00'),
(27, 21, '2026-05-20 20:00:00', '2026-05-20 21:20:00'),
(28, 22, '2026-05-22 17:30:00', '2026-05-22 18:45:00'),
(29, 23, '2026-05-24 19:00:00', '2026-05-24 20:25:00'),
(30, 24, '2026-05-26 16:00:00', '2026-05-26 17:00:00');
GO

INSERT INTO SeriesCompletadas
(IdSesion, IdEjercicio, PesoLevantadoKG, RepeticionesLogradas, RIR, EsRecordPersonal)
VALUES
(3, 1, 60, 8, 2, 0),
(3, 1, 65, 8, 2, 0),
(3, 1, 70, 8, 1, 0),
(3, 1, 70, 7, 0, 0),
(3, 5, 50, 10, 2, 0),
(3, 5, 55, 10, 1, 0),
(3, 5, 60, 8, 0, 0),

(5, 1, 60, 10, 2, 0),
(5, 1, 65, 8, 1, 1),
(5, 11, 35, 10, 2, 0),
(5, 11, 40, 8, 1, 1),

(6, 2, 22, 12, 2, 0),
(6, 2, 24, 10, 1, 1),
(6, 6, 50, 12, 2, 0),
(6, 6, 55, 10, 1, 1),

(7, 1, 70, 8, 1, 0),
(7, 5, 65, 8, 1, 1),

(8, 14, 20, 15, 2, 0),
(8, 14, 25, 12, 1, 0),
(8, 27, 18, 12, 1, 1),

(9, 14, 25, 12, 2, 0),
(9, 15, 30, 10, 1, 1),

(10, 7, 90, 5, 2, 0),
(10, 7, 100, 5, 1, 1),
(10, 1, 80, 5, 1, 1),

(11, 7, 100, 5, 1, 0),
(11, 10, 40, 8, 1, 1),

(12, 23, 100, 10, 2, 0),
(12, 23, 120, 8, 1, 1),

(13, 23, 120, 10, 1, 0),
(13, 24, 45, 12, 1, 1),

(14, 8, 180, 10, 1, 1),
(14, 9, 50, 12, 1, 1),

(15, 2, 26, 12, 2, 0),
(15, 2, 28, 10, 1, 1),

(16, 7, 120, 5, 1, 1),
(16, 5, 85, 5, 1, 1),

(17, 12, 12, 15, 2, 0),
(17, 12, 15, 12, 1, 1),

(18, 14, 35, 10, 1, 1),
(18, 16, 35, 10, 1, 1),

(19, 1, 75, 6, 1, 1),
(19, 5, 80, 6, 1, 1),

(20, 11, 45, 8, 1, 1),
(20, 26, 30, 10, 1, 1),

(21, 8, 200, 8, 1, 1),
(21, 23, 140, 8, 1, 1),

(22, 18, 0, 60, 2, 0),
(22, 25, 0, 20, 1, 1),

(23, 1, 65, 10, 1, 0),
(23, 5, 60, 10, 1, 0),

(24, 7, 80, 8, 1, 0),
(24, 23, 110, 10, 1, 1);
GO

-- Contraseñas semilla
-- Admins          -> Admin123!
-- Entrenadores    → Profe123!
-- Administrativos → Recep123!
-- Clientes        → Cliente123!
INSERT INTO AccesoUsuarios (IdUsuarios, Pass)
VALUES
-- Admins (IdRol = 1)
(1,  '100000.s1gZGwXaK9IXfjFUDLfD1A==.83iEJhswZcxY7VAEeHeeqj9O6cNJmqvZvFz8hYaeVys='),
(2,  '100000.KCjLwoMmF331PDrAMV4BSw==.lbZhkVmxHFWK80fa19CSJhUYwrG7OYavfI4vjhxQJPA='),
-- Entrenadores (IdRol = 4)
(3,  '100000.O7qnXOBk0rU9VwR80a43dw==.rgZIdCCePaHnLIeAvKL9ZUCrOzXYY5SCtWrY6CADJww='),
(4,  '100000.TV/XoddFrAcvjLxNPhpqrQ==.IBTPLUIW17TpyLqVkA1+fxWk+9vGM5dqYhAyOOSn7Z4='),
(5,  '100000.qD/hgCapv0n1P4lIR5ixKw==.FoGIdLx7Hn33Y5bEoNHhXjT5g4Vzh7HqzBSm2yXO8is='),
-- Administrativos (IdRol = 2)
(6,  '100000.lN0wZlc3ee1dZu1POVB7Cw==.92P3r3xVA5dvnll3A/jTOi8fY+ciBG4RJkRC8PjJskQ='),
(7,  '100000.xEvgDHoP3QIhlUrI88cpsw==.csyHX9+95YlNo/45KqBw/Y9YXFHWNH3x+i87EjWStQI='),
-- Clientes (IdRol = 3)
(8,  '100000./f2Re75PP9mzDgwik+69/w==.p54Q4HNT3ZCyyEvCxN6hsCmgTXbh0OEHnUTm7TBGYX0='),
(9,  '100000.GUmxo/rvVtJ7KxlXm0Jqng==.pwqVtEYO/l4oIDPdbWmQTtsr3uTmEQ0/UYWim/kpENc='),
(10, '100000.fTYBUImMu8Qix/i3JRMS+A==.HXzFkhuKgzePFZU/FaC5tt4SWS/P+wLhXGVNxTI70W4='),
(11, '100000.uxZPuhyes7zn5eqxNQs+CA==.HzFpli5mxT0aavbZtmfNPR34rlZr9QbqIFjU5MdE7EY='),
(12, '100000.2vGTqbM12HRoylSKA9zbPA==.C+nxZKag4+V5X8pE4aHrz1u9L+aaadVUKlfswhc7OvQ='),
(13, '100000.2eFigehiCmyo4AhqVIbnJA==.yH6JyZvVi12IcPgPLgrNHOnnrzxJ/M1De+l+XAq7NYM='),
(14, '100000.5v/VDTINVBeTBlkJVL37QA==.lU1mOaMLQ9/aS4dydvoRaGOlCdb/Z7tQhnl7IOxVyis='),
(15, '100000.A48vh/e9y+umuxuTMZyWEg==.KP+Ei1sicUlFLcZEnk/SPeRWW9/F2G2ikW++MGquw9k='),
(16, '100000.fber1YFpsqNEQk5neovi0Q==.MaxOC1XBg1S/lRLxj70i1h+atQ/YAz5pBrXI0LVyeBc='),
(17, '100000.JBfP973Ty0CrlhruJnWYiw==.efnlqpMI586aR/NYoHcxEWuK8z2i6cFJl2dw/sEUhSk='),
(18, '100000.yrsfOHHuFGg1+0hijMY5Hw==.y4CaF6okxVj/pk8nzT4uxRa8dPBRbMPU6QC9fFOGpoE='),
(19, '100000.uRZeMhwz7m3iTNCdkswIPA==.eJj+gRgRSvZnQrgHb2CS5NYgE3/4NA8f20pkqNBLV00='),
(20, '100000.mHSIoZeXFFtcRR+9mPB5TA==.QH6ByA8J0fo+Zkl0KG1dLQ8ek9SfpdX8+AMLfcrs1+Y='),
(21, '100000.8hWeXzZHDdTjn8I4NLXlBA==.ExU3tayAj3io633Sl1BsgdkIq0Fb6KEtAoWlYtG0RA8='),
(22, '100000.DtZxhJidnsI2EAC3V6KKpA==.gFc17drmPXBx81AP5lMvc5oG/2UtSZQvyn4OKOTL3Pg='),
(23, '100000.t40g9Q6vYv94FTuZTXmdow==.H6cCKwhcl7NIJ4ctHiyZeQfhJMdKj/vkpLTGplcz1T4='),
(24, '100000.3LOfkmK+HGIm6Gx9nN0Vyw==.TRmw7vrtMwrkzsKGPfejxgyJXKnSZ8GRYVcfjvCJNJs='),
(25, '100000.5t0tiztch43SavPjsuPExQ==.pGbzYxHs1RE1J2EteLyddSLsm28ioPUISzkTP5KPlyA='),
(26, '100000.x+wyutt22xHlNKT9S+Nqsw==.3uwoV1HKGkzP841cBMddCPdNrPZT689F5Yd4MHJFW6Q='),
(27, '100000.f09OZ0AYHK8C+x3oLgG+cg==.46GISz1PVkbL2afNAsiiFHV0J7GOMvjI68JNlThnWws='),
(28, '100000.s5mwo2R34u0VQvTo+KYZNw==.DB6Q1SrMGHsWCiDsRi0PdsU+ybBYEtz6YAQnlLvnW48='),
(29, '100000.DH/O3xuSSkuen31wHY71EQ==.uQo8Hgp0BvlhLlLWmdwlWiO+K5kqvbU/EI88dhbl/c0='),
(30, '100000.lKVev2Ga6Zf814jPbZlfgg==.11WHI1iQKmthWuTC0V984SHEKksEmibMka2euWKm0bM=');
GO