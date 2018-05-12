-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: May 12, 2018 at 09:59 PM
-- Server version: 5.6.38
-- PHP Version: 7.2.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `band_tracker_test`
--
CREATE DATABASE IF NOT EXISTS `band_tracker_test` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `band_tracker_test`;

-- --------------------------------------------------------

--
-- Table structure for table `bands`
--

CREATE TABLE `bands` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `tracker_info`
--

CREATE TABLE `tracker_info` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `band_id` int(11) NOT NULL,
  `venue_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `tracker_info`
--

INSERT INTO `tracker_info` (`id`, `band_id`, `venue_id`) VALUES
(1, 2, 2),
(2, 4, 4),
(3, 6, 6),
(4, 8, 8),
(5, 10, 10),
(6, 12, 12),
(7, 14, 14),
(8, 16, 16),
(9, 18, 18),
(10, 20, 20),
(11, 21, 21),
(12, 23, 24),
(13, 24, 25),
(14, 27, 29),
(15, 28, 30),
(16, 29, 32),
(17, 31, 34),
(18, 32, 35),
(19, 33, 37),
(20, 35, 39),
(21, 36, 40),
(22, 37, 42),
(23, 39, 44),
(24, 40, 45),
(25, 41, 47),
(26, 43, 49),
(27, 44, 50),
(28, 45, 52),
(29, 47, 54),
(30, 48, 55),
(31, 49, 57),
(32, 51, 59),
(33, 52, 60),
(34, 53, 62),
(35, 55, 64),
(36, 56, 65),
(37, 57, 67),
(38, 58, 68),
(39, 61, 70),
(40, 62, 71),
(41, 63, 73),
(42, 64, 74),
(43, 67, 77),
(44, 68, 78),
(45, 69, 80),
(46, 70, 81),
(47, 73, 84),
(48, 74, 85),
(49, 75, 87),
(50, 76, 88);

-- --------------------------------------------------------

--
-- Table structure for table `venues`
--

CREATE TABLE `venues` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `location` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bands`
--
ALTER TABLE `bands`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tracker_info`
--
ALTER TABLE `tracker_info`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `venues`
--
ALTER TABLE `venues`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bands`
--
ALTER TABLE `bands`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tracker_info`
--
ALTER TABLE `tracker_info`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=51;

--
-- AUTO_INCREMENT for table `venues`
--
ALTER TABLE `venues`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
