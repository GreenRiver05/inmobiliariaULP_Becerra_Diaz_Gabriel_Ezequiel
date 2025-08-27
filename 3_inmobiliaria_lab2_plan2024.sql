-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 27-08-2025 a las 01:05:16
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
  `monto` decimal(10,0) NOT NULL,
  `desde` date NOT NULL,
  `hasta` date NOT NULL,
  `estado` varchar(255) NOT NULL COMMENT 'vigente/finalizado'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `fecha` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `longitud` decimal(20,0) NOT NULL,
  `latitud` decimal(20,0) NOT NULL,
  `uso` varchar(255) NOT NULL COMMENT 'comercial/residendical',
  `ambientes` int(11) NOT NULL COMMENT 'cantidad de ambientes',
  `observacion` text DEFAULT NULL,
  `estado` varchar(255) NOT NULL COMMENT 'disponible/alquilado/no disponible',
  `precio` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `multa`
--

CREATE TABLE `multa` (
  `id` int(11) NOT NULL,
  `contrato_id` int(11) NOT NULL,
  `fechaAviso` date NOT NULL,
  `fechaTerminacion` date NOT NULL,
  `monto` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `contrato_id` int(11) NOT NULL,
  `numero_pago` int(11) NOT NULL,
  `monto` decimal(10,0) NOT NULL,
  `fecha` date NOT NULL,
  `detalle` text NOT NULL,
  `tipo` varchar(255) NOT NULL COMMENT 'parcial/total/multa',
  `estado` varchar(255) NOT NULL COMMENT 'pago/anulado/pendiente'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  `telefono` int(20) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `tipo` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `dni` int(11) NOT NULL,
  `contraseña` varchar(255) NOT NULL,
  `rol` varchar(255) NOT NULL,
  `estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
  ADD KEY `entidad_id` (`entidad_id`),
  ADD KEY `usuario_id` (`usuario_id`) USING BTREE;

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
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

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
  ADD CONSTRAINT `fk_gestion_entidad_id` FOREIGN KEY (`entidad_id`) REFERENCES `contrato` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_gestion_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `gestion_ibfk_1` FOREIGN KEY (`entidad_id`) REFERENCES `multa` (`id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `gestion_ibfk_2` FOREIGN KEY (`entidad_id`) REFERENCES `pago` (`id`) ON UPDATE CASCADE;

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
