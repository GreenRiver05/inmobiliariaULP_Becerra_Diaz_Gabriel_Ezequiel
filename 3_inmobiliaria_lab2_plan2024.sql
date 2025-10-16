-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 16-10-2025 a las 05:52:22
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria_lab2_plan2024`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `inquilino_id` int(11) NOT NULL,
  `inmueble_id` int(11) NOT NULL,
  `id` int(11) NOT NULL,
  `monto` varchar(200) NOT NULL,
  `desde` date NOT NULL,
  `hasta` date NOT NULL,
  `estado` varchar(255) NOT NULL COMMENT 'vigente/finalizado'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`inquilino_id`, `inmueble_id`, `id`, `monto`, `desde`, `hasta`, `estado`) VALUES
(1, 1, 1, '120000', '2025-01-01', '2026-01-01', 'Vigente'),
(2, 2, 2, '95000', '2024-06-01', '2025-06-01', 'Vigente'),
(3, 3, 3, '80000', '2025-03-15', '2026-03-15', 'Rescindido'),
(4, 4, 4, '150000', '2025-05-01', '2026-05-01', 'Vigente'),
(5, 5, 5, '130000', '2025-07-01', '2026-07-01', 'Vigente'),
(6, 6, 6, '100000', '2025-08-01', '2026-08-01', 'Rescindido'),
(7, 7, 7, '180000', '2025-09-01', '2026-09-01', 'Vigente'),
(8, 8, 8, '70000', '2025-10-01', '2026-10-01', 'Vigente'),
(9, 9, 9, '110000', '2025-11-01', '2026-11-01', 'Vigente'),
(10, 10, 10, '85000', '2025-12-01', '2026-12-01', 'Rescindido'),
(11, 11, 11, '140000', '2026-01-01', '2027-01-01', 'Vigente'),
(12, 12, 12, '125000', '2026-02-01', '2027-02-01', 'Vigente'),
(13, 13, 13, '90000', '2026-03-01', '2027-03-01', 'Vigente'),
(14, 14, 14, '98000', '2026-04-01', '2027-04-01', 'Vigente'),
(15, 15, 15, '160000', '2026-05-01', '2027-05-01', 'Rescindido'),
(1, 2, 16, '95000', '2026-06-01', '2027-06-01', 'Vigente'),
(2, 3, 17, '80000', '2026-07-01', '2027-07-01', 'Vigente'),
(3, 4, 18, '150000', '2026-08-01', '2027-08-01', 'Vigente'),
(4, 5, 19, '130000', '2026-09-01', '2027-09-01', 'Vigente'),
(5, 6, 20, '100000', '2026-10-01', '2027-10-01', 'Vigente'),
(6, 7, 21, '180000', '2026-11-01', '2027-11-01', 'Rescindido'),
(7, 8, 22, '70000', '2026-12-01', '2027-12-01', 'Vigente'),
(8, 9, 23, '110000', '2027-01-01', '2028-01-01', 'Rescindido'),
(9, 10, 24, '85000', '2027-02-01', '2028-02-01', 'Vigente'),
(10, 11, 25, '140000', '2027-03-01', '2028-03-01', 'Rescindido');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `gestion`
--

CREATE TABLE `gestion` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `entidad_id` int(11) NOT NULL COMMENT 'Id de multa/pago o contrato',
  `entidad_tipo` varchar(255) NOT NULL COMMENT 'multa/pago/contrato',
  `accion` varchar(255) NOT NULL COMMENT 'Pago/Anulacion/Inicio-Fin Contrato/Inicio-Fin Multa',
  `fecha` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `gestion`
--

INSERT INTO `gestion` (`id`, `usuario_id`, `entidad_id`, `entidad_tipo`, `accion`, `fecha`) VALUES
(134, 1, 1, 'Contrato', 'Alta', '2025-01-01 10:00:00'),
(135, 3, 1, 'Contrato', 'Modificación', '2025-01-01 11:00:00'),
(136, 1, 2, 'Contrato', 'Alta', '2024-06-01 12:00:00'),
(137, 2, 2, 'Contrato', 'Modificación', '2024-06-01 13:00:00'),
(138, 3, 3, 'Contrato', 'Alta', '2025-03-15 14:00:00'),
(139, 1, 3, 'Contrato', 'Modificación', '2025-03-15 15:00:00'),
(140, 1, 3, 'Contrato', 'Cambio de estado a Rescindido', '2025-05-01 16:00:00'),
(141, 3, 4, 'Contrato', 'Alta', '2025-05-01 17:00:00'),
(142, 1, 4, 'Contrato', 'Modificación', '2025-05-01 18:00:00'),
(143, 3, 5, 'Contrato', 'Alta', '2025-07-01 19:00:00'),
(144, 1, 5, 'Contrato', 'Modificación', '2025-07-01 20:00:00'),
(145, 1, 6, 'Contrato', 'Alta', '2025-08-01 21:00:00'),
(146, 3, 6, 'Contrato', 'Modificación', '2025-08-01 22:00:00'),
(147, 1, 6, 'Contrato', 'Cambio de estado a Rescindido', '2025-10-01 23:00:00'),
(148, 1, 7, 'Contrato', 'Alta', '2025-09-01 00:00:00'),
(149, 2, 7, 'Contrato', 'Modificación', '2025-09-01 01:00:00'),
(150, 1, 8, 'Contrato', 'Alta', '2025-10-01 02:00:00'),
(151, 3, 8, 'Contrato', 'Modificación', '2025-10-01 03:00:00'),
(152, 3, 9, 'Contrato', 'Alta', '2025-11-01 04:00:00'),
(153, 3, 9, 'Contrato', 'Modificación', '2025-11-01 05:00:00'),
(154, 1, 10, 'Contrato', 'Alta', '2025-12-01 06:00:00'),
(155, 1, 10, 'Contrato', 'Modificación', '2025-12-01 07:00:00'),
(156, 3, 10, 'Contrato', 'Cambio de estado a Rescindido', '2026-02-01 08:00:00'),
(157, 1, 11, 'Contrato', 'Alta', '2026-01-01 09:00:00'),
(158, 2, 11, 'Contrato', 'Modificación', '2026-01-01 10:00:00'),
(159, 1, 12, 'Contrato', 'Alta', '2026-02-01 11:00:00'),
(160, 3, 12, 'Contrato', 'Modificación', '2026-02-01 12:00:00'),
(161, 1, 13, 'Contrato', 'Alta', '2026-03-01 13:00:00'),
(162, 3, 13, 'Contrato', 'Modificación', '2026-03-01 14:00:00'),
(163, 1, 14, 'Contrato', 'Alta', '2026-04-01 15:00:00'),
(164, 3, 14, 'Contrato', 'Modificación', '2026-04-01 16:00:00'),
(165, 3, 15, 'Contrato', 'Alta', '2026-05-01 17:00:00'),
(166, 1, 15, 'Contrato', 'Modificación', '2026-05-01 18:00:00'),
(167, 3, 15, 'Contrato', 'Cambio de estado a Rescindido', '2026-06-01 19:00:00'),
(168, 3, 16, 'Contrato', 'Alta', '2026-06-01 20:00:00'),
(169, 1, 16, 'Contrato', 'Modificación', '2026-06-01 21:00:00'),
(170, 3, 17, 'Contrato', 'Alta', '2026-07-01 22:00:00'),
(171, 1, 17, 'Contrato', 'Modificación', '2026-07-01 23:00:00'),
(172, 1, 18, 'Contrato', 'Alta', '2026-08-01 00:00:00'),
(173, 3, 18, 'Contrato', 'Modificación', '2026-08-01 01:00:00'),
(174, 1, 19, 'Contrato', 'Alta', '2026-09-01 02:00:00'),
(175, 1, 19, 'Contrato', 'Modificación', '2026-09-01 03:00:00'),
(176, 1, 20, 'Contrato', 'Alta', '2026-10-01 04:00:00'),
(177, 1, 20, 'Contrato', 'Modificación', '2026-10-01 05:00:00'),
(178, 3, 21, 'Contrato', 'Alta', '2026-11-01 06:00:00'),
(179, 3, 21, 'Contrato', 'Modificación', '2026-11-01 07:00:00'),
(180, 1, 21, 'Contrato', 'Cambio de estado a Rescindido', '2026-12-01 08:00:00'),
(181, 3, 22, 'Contrato', 'Alta', '2026-12-01 09:00:00'),
(182, 1, 22, 'Contrato', 'Modificación', '2026-12-01 10:00:00'),
(183, 2, 23, 'Contrato', 'Alta', '2027-01-01 11:00:00'),
(184, 1, 23, 'Contrato', 'Modificación', '2027-01-01 12:00:00'),
(185, 1, 23, 'Contrato', 'Cambio de estado a Rescindido', '2027-02-01 13:00:00'),
(186, 3, 24, 'Contrato', 'Alta', '2027-02-01 14:00:00'),
(187, 1, 24, 'Contrato', 'Modificación', '2027-02-01 15:00:00'),
(188, 1, 25, 'Contrato', 'Alta', '2027-03-01 16:00:00'),
(189, 3, 25, 'Contrato', 'Modificación', '2027-03-01 17:00:00'),
(190, 1, 25, 'Contrato', 'Cambio de estado a Rescindido', '2027-04-01 18:00:00'),
(191, 1, 1, 'Pago', 'Alta', '2025-02-01 10:02:00'),
(192, 1, 1, 'Pago', 'Modificación', '2025-02-01 10:12:00'),
(193, 3, 2, 'Pago', 'Alta', '2025-03-01 10:04:00'),
(194, 3, 2, 'Pago', 'Modificación', '2025-03-01 10:14:00'),
(195, 1, 3, 'Pago', 'Alta', '2025-04-01 10:06:00'),
(196, 1, 3, 'Pago', 'Modificación', '2025-04-01 10:16:00'),
(197, 1, 3, 'Pago', 'Cambio de estado a Pendiente', '2025-04-01 10:26:00'),
(198, 3, 4, 'Pago', 'Alta', '2024-07-01 10:08:00'),
(199, 3, 4, 'Pago', 'Modificación', '2024-07-01 10:18:00'),
(200, 1, 5, 'Pago', 'Alta', '2024-08-01 10:10:00'),
(201, 1, 5, 'Pago', 'Modificación', '2024-08-01 10:20:00'),
(202, 1, 5, 'Pago', 'Cambio de estado a Anulado', '2024-08-01 10:30:00'),
(203, 3, 6, 'Pago', 'Alta', '2025-04-01 10:12:00'),
(204, 3, 6, 'Pago', 'Modificación', '2025-04-01 10:22:00'),
(205, 1, 7, 'Pago', 'Alta', '2025-05-01 10:14:00'),
(206, 1, 7, 'Pago', 'Modificación', '2025-05-01 10:24:00'),
(207, 1, 7, 'Pago', 'Cambio de estado a Pendiente', '2025-05-01 10:34:00'),
(208, 3, 8, 'Pago', 'Alta', '2025-06-01 10:16:00'),
(209, 3, 8, 'Pago', 'Modificación', '2025-06-01 10:26:00'),
(210, 1, 9, 'Pago', 'Alta', '2025-07-01 10:18:00'),
(211, 1, 9, 'Pago', 'Modificación', '2025-07-01 10:28:00'),
(212, 3, 10, 'Pago', 'Alta', '2025-08-01 10:20:00'),
(213, 3, 10, 'Pago', 'Modificación', '2025-08-01 10:30:00'),
(214, 1, 11, 'Pago', 'Alta', '2025-09-01 10:22:00'),
(215, 1, 11, 'Pago', 'Modificación', '2025-09-01 10:32:00'),
(216, 1, 11, 'Pago', 'Cambio de estado a Pendiente', '2025-09-01 10:42:00'),
(217, 3, 12, 'Pago', 'Alta', '2025-09-01 10:24:00'),
(218, 3, 12, 'Pago', 'Modificación', '2025-09-01 10:34:00'),
(219, 1, 13, 'Pago', 'Alta', '2025-10-01 10:26:00'),
(220, 1, 13, 'Pago', 'Modificación', '2025-10-01 10:36:00'),
(221, 3, 14, 'Pago', 'Alta', '2025-10-01 10:28:00'),
(222, 3, 14, 'Pago', 'Modificación', '2025-10-01 10:38:00'),
(223, 1, 15, 'Pago', 'Alta', '2025-11-01 10:30:00'),
(224, 1, 15, 'Pago', 'Modificación', '2025-11-01 10:40:00'),
(225, 1, 15, 'Pago', 'Cambio de estado a Pendiente', '2025-11-01 10:50:00'),
(226, 3, 16, 'Pago', 'Alta', '2025-11-01 10:32:00'),
(227, 3, 16, 'Pago', 'Modificación', '2025-11-01 10:42:00'),
(228, 1, 17, 'Pago', 'Alta', '2025-12-01 10:34:00'),
(229, 1, 17, 'Pago', 'Modificación', '2025-12-01 10:44:00'),
(230, 3, 18, 'Pago', 'Alta', '2026-01-01 10:36:00'),
(231, 3, 18, 'Pago', 'Modificación', '2026-01-01 10:46:00'),
(232, 1, 19, 'Pago', 'Alta', '2026-02-01 10:38:00'),
(233, 1, 19, 'Pago', 'Modificación', '2026-02-01 10:48:00'),
(234, 1, 19, 'Pago', 'Cambio de estado a Pendiente', '2026-02-01 10:58:00'),
(235, 1, 1, 'Multa', 'Alta', '2025-05-01 10:02:00'),
(236, 1, 1, 'Multa', 'Modificación', '2025-05-01 10:12:00'),
(237, 3, 2, 'Multa', 'Alta', '2025-10-01 10:04:00'),
(238, 3, 2, 'Multa', 'Modificación', '2025-10-01 10:14:00'),
(239, 1, 3, 'Multa', 'Alta', '2026-02-01 10:06:00'),
(240, 1, 3, 'Multa', 'Modificación', '2026-02-01 10:16:00'),
(241, 3, 4, 'Multa', 'Alta', '2026-06-01 10:08:00'),
(242, 3, 4, 'Multa', 'Modificación', '2026-06-01 10:18:00'),
(243, 1, 5, 'Multa', 'Alta', '2026-12-01 10:10:00'),
(244, 1, 5, 'Multa', 'Modificación', '2026-12-01 10:20:00'),
(245, 3, 6, 'Multa', 'Alta', '2027-04-01 10:12:00'),
(246, 3, 6, 'Multa', 'Modificación', '2027-04-01 10:22:00'),
(247, 1, 18, 'Pago', 'Cambio de estado a Pendiente', '2025-10-16 00:12:36');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `propietario_id` int(11) NOT NULL,
  `tipo_id` int(11) NOT NULL COMMENT 'local/deposito/casa/departamento',
  `direccion` varchar(255) DEFAULT NULL,
  `localidad` varchar(255) NOT NULL,
  `longitud` varchar(40) NOT NULL,
  `latitud` varchar(40) NOT NULL,
  `uso` varchar(255) NOT NULL COMMENT 'comercial/residendical',
  `ambientes` int(11) NOT NULL COMMENT 'cantidad de ambientes',
  `observacion` text DEFAULT NULL,
  `estado` tinyint(255) NOT NULL COMMENT 'disponible/no disponible',
  `precio` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id`, `propietario_id`, `tipo_id`, `direccion`, `localidad`, `longitud`, `latitud`, `uso`, `ambientes`, `observacion`, `estado`, `precio`) VALUES
(1, 1, 1, 'Av. Belgrano 123', 'San Luis', '-65.0001', '-33.0001', 'Comercial', 2, 'Local céntrico con vidriera amplia', 1, '120000'),
(2, 1, 4, 'Mitre 456', 'Villa Mercedes', '-65.0002', '-33.0002', 'Residencial', 3, 'Departamento interno con patio compartido', 1, '95000'),
(3, 2, 2, 'España 789', 'San Luis', '-65.0003', '-33.0003', 'Comercial', 1, 'Depósito techado con acceso vehicular', 1, '80000'),
(4, 3, 3, 'Rivadavia 321', 'Villa Mercedes', '-65.0004', '-33.0004', 'Residencial', 4, 'Casa familiar con patio y cochera', 1, '150000'),
(5, 4, 1, 'Junín 654', 'San Luis', '-65.0005', '-33.0005', 'Comercial', 2, 'Local en esquina con doble acceso', 1, '130000'),
(6, 5, 4, 'Colón 987', 'Villa Mercedes', '-65.0006', '-33.0006', 'Residencial', 3, 'Departamento luminoso en segundo piso', 1, '100000'),
(7, 6, 3, 'San Martín 111', 'San Luis', '-65.0007', '-33.0007', 'Residencial', 5, 'Casa con jardín y pileta', 1, '180000'),
(8, 7, 2, 'Lavalle 222', 'Villa Mercedes', '-65.0008', '-33.0008', 'Comercial', 1, 'Depósito pequeño para mercadería', 1, '70000'),
(9, 8, 1, 'Catamarca 333', 'San Luis', '-65.0009', '-33.0009', 'Comercial', 2, 'Local con entrepiso y baño privado', 1, '110000'),
(10, 9, 4, 'Belgrano 444', 'Villa Mercedes', '-65.0010', '-33.0010', 'Residencial', 2, 'Departamento monoambiente', 1, '85000'),
(11, 2, 3, 'Sarmiento 555', 'San Luis', '-65.0011', '-33.0011', 'Residencial', 4, 'Casa antigua refaccionada', 1, '140000'),
(12, 3, 1, 'Mitre 666', 'Villa Mercedes', '-65.0012', '-33.0012', 'Comercial', 3, 'Local amplio con depósito trasero', 1, '125000'),
(13, 4, 2, 'España 777', 'San Luis', '-65.0013', '-33.0013', 'Comercial', 1, 'Depósito con portón metálico', 0, '90000'),
(14, 5, 4, 'Rivadavia 888', 'Villa Mercedes', '-65.0014', '-33.0014', 'Residencial', 3, 'Departamento en planta baja', 1, '98000'),
(15, 6, 3, 'Junín 999', 'San Luis', '-65.0015', '-33.0015', 'Residencial', 5, 'Casa de dos plantas con cochera doble', 0, '160000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`id`, `dni`, `estado`) VALUES
(1, 20000001, 0),
(2, 20000002, 1),
(3, 20000003, 1),
(4, 20000004, 1),
(5, 20000005, 1),
(6, 20000006, 0),
(7, 20000007, 1),
(8, 20000008, 1),
(9, 20000009, 1),
(10, 20000010, 0),
(11, 20000011, 1),
(12, 20000012, 1),
(13, 20000013, 1),
(14, 20000014, 0),
(15, 20000015, 1),
(16, 20000016, 0),
(17, 20000017, 1),
(18, 20000018, 1),
(19, 20000019, 1),
(20, 20000020, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `multa`
--

CREATE TABLE `multa` (
  `id` int(11) NOT NULL,
  `contrato_id` int(11) NOT NULL,
  `fechaAviso` date NOT NULL,
  `fechaTerminacion` date NOT NULL,
  `monto` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `multa`
--

INSERT INTO `multa` (`id`, `contrato_id`, `fechaAviso`, `fechaTerminacion`, `monto`) VALUES
(1, 3, '2025-05-01', '2025-06-01', '160000'),
(2, 6, '2025-10-01', '2025-11-01', '200000'),
(3, 10, '2026-02-01', '2026-03-01', '170000'),
(4, 15, '2026-06-01', '2026-07-01', '320000'),
(5, 21, '2026-12-01', '2027-01-01', '360000'),
(6, 25, '2027-04-01', '2027-05-01', '280000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `contrato_id` int(11) NOT NULL,
  `numero_pago` int(11) NOT NULL,
  `monto` varchar(20) NOT NULL,
  `fecha` date NOT NULL,
  `detalle` text NOT NULL,
  `tipo` varchar(255) NOT NULL COMMENT 'parcial/total/multa',
  `estado` varchar(255) NOT NULL COMMENT 'pago/anulado/pendiente'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`id`, `contrato_id`, `numero_pago`, `monto`, `fecha`, `detalle`, `tipo`, `estado`) VALUES
(1, 1, 1, '120000', '2025-02-01', 'Pago mes febrero', 'Total', 'Pago'),
(2, 1, 2, '120000', '2025-03-01', 'Pago mes marzo', 'Total', 'Pago'),
(3, 1, 3, '60000', '2025-04-01', 'Pago parcial abril', 'Parcial', 'Pendiente'),
(4, 2, 1, '95000', '2024-07-01', 'Pago inicial', 'Total', 'Pago'),
(5, 2, 2, '95000', '2024-08-01', 'Pago agosto', 'Total', 'Anulado'),
(6, 3, 1, '80000', '2025-04-01', 'Pago abril', 'Total', 'Pago'),
(7, 3, 2, '160000', '2025-05-01', 'Pago Mayo', 'Parcial', 'Pendiente'),
(8, 4, 1, '150000', '2025-06-01', 'Pago junio', 'Total', 'Pago'),
(9, 4, 2, '150000', '2025-07-01', 'Pago julio', 'Total', 'Pago'),
(10, 5, 1, '130000', '2025-08-01', 'Pago agosto', 'Total', 'Pago'),
(11, 5, 2, '130000', '2025-09-01', 'Pago septiembre', 'Total', 'Pendiente'),
(12, 6, 1, '100000', '2025-09-01', 'Pago septiembre', 'Total', 'Pago'),
(13, 6, 2, '200000', '2025-10-01', 'Pago Octubre', 'Total', 'Pago'),
(14, 7, 1, '180000', '2025-10-01', 'Pago octubre', 'Total', 'Pago'),
(15, 7, 2, '180000', '2025-11-01', 'Pago noviembre', 'Total', 'Pendiente'),
(16, 8, 1, '70000', '2025-11-01', 'Pago noviembre', 'Total', 'Pago'),
(17, 9, 1, '110000', '2025-12-01', 'Pago diciembre', 'Total', 'Pago'),
(18, 10, 1, '85000', '2026-01-01', 'Pago enero', 'Total', 'Pendiente'),
(19, 10, 2, '170000', '2026-02-01', 'Pago febrero', 'Parcial', 'Pendiente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `persona`
--

CREATE TABLE `persona` (
  `dni` int(11) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `apellido` varchar(255) NOT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `localidad` varchar(255) DEFAULT NULL,
  `correo` varchar(200) DEFAULT NULL,
  `telefono` bigint(20) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `persona`
--

INSERT INTO `persona` (`dni`, `nombre`, `apellido`, `direccion`, `localidad`, `correo`, `telefono`, `estado`) VALUES
(10000001, 'Carlos', 'Gómez', 'Belgrano 123', 'San Luis', 'carlos.gomez@mail.com', 2664000001, 1),
(10000002, 'Laura', 'Martínez', 'Sarmiento 456', 'Villa Mercedes', 'laura.martinez@mail.com', 2664000002, 1),
(10000003, 'Jorge', 'Pérez', 'Mitre 789', 'San Luis', 'jorge.perez@mail.com', 2664000003, 1),
(10000004, 'Ana', 'Rodríguez', 'Rivadavia 321', 'Villa Mercedes', 'ana.rodriguez@mail.com', 2664000004, 1),
(10000005, 'Luis', 'Fernández', 'Av. España 654', 'San Luis', 'luis.fernandez@mail.com', 2664000005, 1),
(10000006, 'María', 'López', 'Junín 987', 'Villa Mercedes', 'maria.lopez@mail.com', 2664000006, 1),
(10000007, 'Ricardo', 'Sánchez', 'Colón 111', 'San Luis', 'ricardo.sanchez@mail.com', 2664000007, 1),
(10000008, 'Valeria', 'Torres', 'San Martín 222', 'Villa Mercedes', 'valeria.torres@mail.com', 2664000008, 1),
(10000009, 'Federico', 'Ramírez', 'Lavalle 333', 'San Luis', 'federico.ramirez@mail.com', 2664000009, 1),
(10000010, 'Cecilia', 'Molina', 'Catamarca 444', 'Villa Mercedes', 'cecilia.molina@mail.com', 2664000010, 1),
(20000001, 'Martín', 'Castro', 'Belgrano 101', 'San Luis', 'martin.castro@mail.com', 2664100001, 1),
(20000002, 'Lucía', 'Gutiérrez', 'Sarmiento 202', 'Villa Mercedes', 'lucia.gutierrez@mail.com', 2664100002, 1),
(20000003, 'Pablo', 'Silva', 'Mitre 303', 'San Luis', 'pablo.silva@mail.com', 2664100003, 1),
(20000004, 'Sofía', 'Ríos', 'Rivadavia 404', 'Villa Mercedes', 'sofia.rios@mail.com', 2664100004, 1),
(20000005, 'Diego', 'Vega', 'España 505', 'San Luis', 'diego.vega@mail.com', 2664100005, 1),
(20000006, 'Camila', 'Navarro', 'Junín 606', 'Villa Mercedes', 'camila.navarro@mail.com', 2664100006, 1),
(20000007, 'Andrés', 'Moreno', 'Colón 707', 'San Luis', 'andres.moreno@mail.com', 2664100007, 1),
(20000008, 'Florencia', 'Cabrera', 'San Martín 808', 'Villa Mercedes', 'florencia.cabrera@mail.com', 2664100008, 1),
(20000009, 'Gonzalo', 'Luna', 'Lavalle 909', 'San Luis', 'gonzalo.luna@mail.com', 2664100009, 1),
(20000010, 'Julieta', 'Paredes', 'Catamarca 1001', 'Villa Mercedes', 'julieta.paredes@mail.com', 2664100010, 1),
(20000011, 'Tomás', 'Aguilar', 'Belgrano 1101', 'San Luis', 'tomas.aguilar@mail.com', 2664100011, 1),
(20000012, 'Melina', 'Sosa', 'Sarmiento 1202', 'Villa Mercedes', 'melina.sosa@mail.com', 2664100012, 1),
(20000013, 'Iván', 'Quiroga', 'Mitre 1303', 'San Luis', 'ivan.quiroga@mail.com', 2664100013, 1),
(20000014, 'Carla', 'Benítez', 'Rivadavia 1404', 'Villa Mercedes', 'carla.benitez@mail.com', 2664100014, 1),
(20000015, 'Emiliano', 'Ortiz', 'España 1505', 'San Luis', 'emiliano.ortiz@mail.com', 2664100015, 1),
(20000016, 'Agustina', 'Reynoso', 'Junín 1606', 'Villa Mercedes', 'agustina.reynoso@mail.com', 2664100016, 1),
(20000017, 'Matías', 'Delgado', 'Colón 1707', 'San Luis', 'matias.delgado@mail.com', 2664100017, 1),
(20000018, 'Brenda', 'Mansilla', 'San Martín 1808', 'Villa Mercedes', 'brenda.mansilla@mail.com', 2664100018, 1),
(20000019, 'Facundo', 'Correa', 'Lavalle 1909', 'San Luis', 'facundo.correa@mail.com', 2664100019, 1),
(20000020, 'Rocío', 'Giménez', 'Catamarca 2001', 'Villa Mercedes', 'rocio.gimenez@mail.com', 2664100020, 1),
(90000001, 'Gabriel Ezequiel', 'Becerra', 'Libertad 1223', 'Villa Mercedes', 'Gabriel@mail.com', 2657545140, 1),
(90000002, 'ULP', 'Tuds', 'Ficticia 123', 'La Punta', 'Ulp@mail.com', 2664232323, 1),
(90000003, 'Usuario', 'Suspendido', NULL, NULL, 'Suspendido@mail.com', 0, 1),
(90000004, 'Eliminar', 'Primero', NULL, NULL, 'Eliminar@primero', 0, 1),
(90000005, 'Eliminar', 'Segundo', NULL, NULL, 'eliminar@segundo', 0, 1),
(90000006, 'Eliminar', 'Tercero', NULL, NULL, 'eliminar@tercero', 0, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`id`, `dni`, `estado`) VALUES
(1, 10000001, 0),
(2, 10000002, 1),
(3, 10000003, 1),
(4, 10000004, 1),
(5, 10000005, 1),
(6, 10000006, 1),
(7, 10000007, 0),
(8, 10000008, 1),
(9, 10000009, 1),
(10, 10000010, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `tipo` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_inmueble`
--

INSERT INTO `tipo_inmueble` (`id`, `descripcion`, `tipo`) VALUES
(1, 'Espacio comercial, diseñado para atención al público o actividades comerciales. Frente vidriado y acceso directo', 'Local'),
(2, 'Inmueble destinado al almacenamiento de mercadería, materiales o equipos. Amplio, techado y con acceso vehicular.', 'Deposito'),
(3, 'Vivienda independiente, ideal para uso familiar. Puede incluir patio, cochera y espacios exteriores.', 'Casa'),
(4, 'Unidad habitacional, con acceso compartido. Pensado para uso residencial, con ambientes distribuidos en planta única.', 'Departamento');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `contraseña` varchar(255) NOT NULL,
  `rol` int(255) NOT NULL,
  `Avatar` varchar(500) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `dni`, `contraseña`, `rol`, `Avatar`, `estado`) VALUES
(1, 90000001, '2ipOb6R/EBhXNhR3KuPdVqK8HHLeVpZ65NnfPT3eqOw=', 1, 'uploads\\avatar_1_.jpg', 1),
(2, 90000002, 'jMxLrNSbaOuz0yGIRQ7IqC+malRd7rhFP4WF8V/bmkg=', 2, 'uploads\\avatar_2_.jpg', 1),
(3, 90000003, 'NIQwSbyi9v48ePVMgxxgL6nqRLhdNQk6b468lYDLvi8=', 2, '', 0),
(4, 90000004, 'Nf6zhkDBmBpBNynkyVLPZSNSehi1dl64hCKQnlqZj8M=', 2, '', 0),
(5, 90000005, 'BV1eKHUP0Ubz4JzxR4Wbv/oOzGIUwq0y5QbA/oOtDds=', 2, '', 0),
(6, 90000006, 'dqssk0lxIDaNodIvldSfmAH/A/D0kca6Y8k0Ib+ATkY=', 1, '', 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inquilino_id` (`inquilino_id`) USING BTREE,
  ADD KEY `inmueble_id` (`inmueble_id`) USING BTREE;

--
-- Indices de la tabla `gestion`
--
ALTER TABLE `gestion`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`) USING BTREE,
  ADD KEY `gestion_ibfk_2` (`entidad_id`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `propietario_id` (`propietario_id`) USING BTREE,
  ADD KEY `tipo_id` (`tipo_id`) USING BTREE;

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD KEY `dni` (`dni`) USING BTREE;

--
-- Indices de la tabla `multa`
--
ALTER TABLE `multa`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_id` (`contrato_id`) USING BTREE;

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_id` (`contrato_id`) USING BTREE;

--
-- Indices de la tabla `persona`
--
ALTER TABLE `persona`
  ADD PRIMARY KEY (`dni`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD KEY `dni` (`dni`) USING BTREE;

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`),
  ADD KEY `dni` (`dni`) USING BTREE;

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT de la tabla `gestion`
--
ALTER TABLE `gestion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=248;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `multa`
--
ALTER TABLE `multa`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `fk_contrato_inmueble_id` FOREIGN KEY (`inmueble_id`) REFERENCES `inmueble` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_contrato_inquilino_id` FOREIGN KEY (`inquilino_id`) REFERENCES `inquilino` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `gestion`
--
ALTER TABLE `gestion`
  ADD CONSTRAINT `fk_gestion_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_inmueble_propietario_id` FOREIGN KEY (`propietario_id`) REFERENCES `propietario` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_inmueble_tipo_id` FOREIGN KEY (`tipo_id`) REFERENCES `tipo_inmueble` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD CONSTRAINT `fk_inquilino_dni` FOREIGN KEY (`dni`) REFERENCES `persona` (`dni`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `multa`
--
ALTER TABLE `multa`
  ADD CONSTRAINT `fk_multa_contrato_id` FOREIGN KEY (`contrato_id`) REFERENCES `contrato` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_contrato_id` FOREIGN KEY (`contrato_id`) REFERENCES `contrato` (`id`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD CONSTRAINT `fk_propietario_dni` FOREIGN KEY (`dni`) REFERENCES `persona` (`dni`) ON UPDATE CASCADE;

--
-- Filtros para la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD CONSTRAINT `fk_usuario_dni` FOREIGN KEY (`dni`) REFERENCES `persona` (`dni`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
